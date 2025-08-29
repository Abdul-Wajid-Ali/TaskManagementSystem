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

        public UserService(IMapper mapper, IUserRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<long> CreateUserAsync(CreateUserDto dto)
        {
            var newUser = _mapper.Map<User>(dto);

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

            existingUser.Email ??= dto.Email;
            existingUser.Username ??= dto.Username;

            //Update Password Logic For Later
            //existingUser.PasswordHash= dto.Status;
            //existingUser.PasswordHash= dto.Status;

            existingUser.Role = dto.Role;

            return await _repository.UpdateUserAsync(existingUser);
        }
    }
}