using AutoMapper;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;

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

            newUser.PasswordSalt = _passwordService.GenerateSalt();
            newUser.PasswordHash = _passwordService.HashPassword(dto.Password, newUser.PasswordSalt);

            await _repository.CreateUserAsync(newUser);

            return newUser.Id;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _repository.GetAllUsersAsync()
                .ContinueWith(item => _mapper.Map<IEnumerable<UserDto>>(item.Result));
        }

        public async Task<UserDto?> GetUserByIdAsync(long id)
        {
            return await _repository.GetUserByIdAsync(id)
                .ContinueWith(item => _mapper.Map<UserDto?>(item.Result));
        }

        public async Task<bool> SoftDeleteUserAsync(long id)
        {
            var existingUser = await _repository.GetUserByIdAsync(id);

            if (existingUser == null)
                return false;

            existingUser.DeletedOn = DateTime.UtcNow;

            return await _repository.SoftDeleteUserAsync(existingUser);
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

            existingUser.Email ??= dto.Email;
            existingUser.Username ??= dto.Username;
            existingUser.Role = dto.Role ?? existingUser.Role;

            return await _repository.UpdateUserAsync(existingUser);
        }
    }
}