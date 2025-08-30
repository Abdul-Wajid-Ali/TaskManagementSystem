using AutoMapper;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly IPasswordService _passwordService;

        public UserService(IMapper mapper, IUserRepository repository, IPasswordService passwordService)
        {
            _mapper = mapper;
            _repository = repository;
            _passwordService = passwordService;
        }

        public async Task<long> CreateUserAsync(CreateUserDto dto)
        {
            var newUser = _mapper.Map<User>(dto);

            newUser.Role = UserRole.Employee;
            newUser.CreationMethod = UserCreationMethod.CreatedByAdmin;
            newUser.PasswordSalt = _passwordService.GenerateSalt();
            newUser.PasswordHash = _passwordService.HashPassword(dto.Password, newUser.PasswordSalt);

            await _repository.CreateUserAsync(newUser);

            return newUser.Id;
        }

        public async Task<long> RegisterUserAsync(RegisterRequestDto dto)
        {
            var newUser = _mapper.Map<User>(dto);

            newUser.Role = UserRole.Employee;
            newUser.PasswordSalt = _passwordService.GenerateSalt();
            newUser.PasswordHash = _passwordService.HashPassword(dto.Password, newUser.PasswordSalt);

            await _repository.CreateUserAsync(newUser);

            return newUser.Id;
        }

        public async Task<IEnumerable<UserDto>?> GetAllUsersAsync()
        {
            var usersList = await _repository.GetAllUsersAsync()
                .ContinueWith(item => _mapper.Map<IEnumerable<UserDto>>(item.Result));

            if (usersList == null || !usersList.Any())
                return null;

            return usersList;
        }

        public async Task<UserDto?> GetUserByIdAsync(long id)
        {
            var user = await _repository.GetUserByIdAsync(id)
                .ContinueWith(item => _mapper.Map<UserDto?>(item.Result));

            if (user == null)
                return null;

            return user;
        }

        public async Task<bool> SoftDeleteUserAsync(long id)
        {
            var existingUser = await _repository.GetUserByIdAsync(id);

            if (existingUser == null)
                return false;

            existingUser.DeletedOn = DateTime.UtcNow;

            return await _repository.UpdateUserAsync(existingUser);
        }

        public async Task<bool> UpdateUserAsync(long userId, UpdateUserDto dto)
        {
            var existingUser = await _repository.GetUserByIdAsync(userId);

            if (existingUser == null)
                return false;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                existingUser.PasswordSalt = _passwordService.GenerateSalt();
                existingUser.PasswordHash = _passwordService.HashPassword(dto.Password, existingUser.PasswordSalt);
            }

            existingUser.Email = dto.Email ?? existingUser.Email;
            existingUser.Username = dto.Username ?? existingUser.Username;
            existingUser.Role = dto.Role ?? existingUser.Role;

            return await _repository.UpdateUserAsync(existingUser);
        }
    }
}