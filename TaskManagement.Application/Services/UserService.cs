using AutoMapper;
using TaskManagement.Application.Common;
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

        // Create a new user
        public async Task<Result<long>> CreateUserAsync(CreateUserDto dto, long Id)
        {
            if (await _repository.GetUserByEmailAsync(dto.Email) != null)
                return Result<long>.Fail(ErrorCodes.UserEmailAlreadyExists);

            var newUser = _mapper.Map<User>(dto);

            newUser.CreatedByUserId = Id;
            newUser.Role = UserRole.Employee;
            newUser.CreationMethod = UserCreationMethod.CreatedByAdmin;
            newUser.PasswordSalt = _passwordService.GenerateSalt();
            newUser.PasswordHash = _passwordService.HashPassword(dto.Password, newUser.PasswordSalt);

            await _repository.CreateUserAsync(newUser);

            return Result<long>.Success(newUser.Id, SuccessCodes.UserCreatedSuccessfully);
        }

        // Get all users created by specific user
        public async Task<Result<IEnumerable<UserDto>>> GetCreatedUsersAsync(long id)
        {
            var usersList = await _repository.GetCreatedUsersAsync(id)
                .ContinueWith(item => _mapper.Map<IEnumerable<UserDto>>(item.Result));

            return Result<IEnumerable<UserDto>>.Success(usersList);
        }

        // Get a user by Id
        public async Task<Result<UserDto>> GetUserByIdAsync(long id)
        {
            var user = await _repository.GetUserByIdAsync(id)
                .ContinueWith(item => _mapper.Map<UserDto?>(item.Result));

            if (user == null)
                return Result<UserDto>.Fail(ErrorCodes.UserNotFound);

            return Result<UserDto>.Success(user);
        }

        // Soft delete a user by setting DeletedOn timestamp
        public async Task<Result<bool>> SoftDeleteUserAsync(long id, long currentUserId)
        {
            var existingUser = await _repository.GetUserByIdAsync(id);

            // Check if user exists
            if (existingUser == null)
                return Result<bool>.Fail(ErrorCodes.UserNotFound);

            var IsUserCreated = await _repository.IsCreatedUser(id, currentUserId);

            // Check if user is created by current user
            if (!IsUserCreated)
                return Result<bool>.Fail(ErrorCodes.UserNotFound);

            existingUser.DeletedOn = DateTime.UtcNow;

            return Result<bool>.Success(await _repository.UpdateUserAsync(existingUser), SuccessCodes.UserDeletedSucessfully);
        }

        // Update user details
        public async Task<Result<UserDto>> UpdateUserAsync(long userId, UpdateUserDto dto, long currentUserId)
        {
            var existingUser = await _repository.GetUserByIdAsync(userId);

            // Check if user exists
            if (existingUser == null)
                return Result<UserDto>.Fail(ErrorCodes.UserNotFound);

            var IsUserCreated = await _repository.IsCreatedUser(userId, currentUserId);

            // Check if user is created by current user
            if (!IsUserCreated)
                return Result<UserDto>.Fail(ErrorCodes.UserNotFound);

            // Check for email uniqueness if email is being updated
            if (!string.IsNullOrWhiteSpace(dto.Username) && await _repository.GetUserByEmailAsync(dto.Email!) != null)
                return Result<UserDto>.Fail(ErrorCodes.UserEmailAlreadyExists);

            existingUser.UpdatedOn = DateTime.UtcNow;

            // Update fields if provided
            existingUser.Email = string.IsNullOrWhiteSpace(dto.Email) ? existingUser.Email : dto.Email;
            existingUser.Username = string.IsNullOrWhiteSpace(dto.Username) ? existingUser.Username : dto.Username;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                existingUser.PasswordSalt = _passwordService.GenerateSalt();
                existingUser.PasswordHash = _passwordService.HashPassword(dto.Password, existingUser.PasswordSalt);
            }

            await _repository.UpdateUserAsync(existingUser);

            return Result<UserDto>.Success(_mapper.Map<UserDto>(existingUser));
        }
    }
}