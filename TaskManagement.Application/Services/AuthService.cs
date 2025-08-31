using AutoMapper;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly IJwtTokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public AuthService(IMapper mapper, IUserRepository repository, IPasswordService passwordService, IJwtTokenService tokenService)
        {
            _mapper = mapper;
            _repository = repository;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        //Register a new user
        public async Task<Result<UserDto>> RegisterUserAsync(RegisterRequestDto dto)
        {
            // Check if email already exists
            if (await _repository.GetUserByEmailAsync(dto.Email) != null)
                return Result<UserDto>.Fail(ErrorCodes.UserEmailAlreadyExists);

            // Map DTO to domain entity
            var newUser = _mapper.Map<User>(dto);

            // Set additional fields
            newUser.Role = UserRole.Admin;
            newUser.CreationMethod = UserCreationMethod.Registered;
            newUser.PasswordSalt = _passwordService.GenerateSalt();
            newUser.PasswordHash = _passwordService.HashPassword(dto.Password, newUser.PasswordSalt);

            // Save user
            await _repository.CreateUserAsync(newUser);

            // Return success result
            return Result<UserDto>.Success(_mapper.Map<UserDto>(newUser));
        }

        // Authenticate user and return login response with JWT token
        public async Task<Result<LoginResponseDto>> LoginUserAsync(LoginRequestDto dto)
        {
            // Find user by email
            var user = await _repository.GetUserByEmailAsync(dto.Email);

            // if user not found, return error
            if (user == null)
                return Result<LoginResponseDto>.Fail(ErrorCodes.UserNotFound);

            var isValidCreds = _passwordService.VerifyPassword(dto.Password, user.PasswordSalt, user.PasswordHash);

            //If password does not match, return error
            if (!isValidCreds)
                return Result<LoginResponseDto>.Fail(ErrorCodes.InvalidCredentials);

            var loggedInUser = _mapper.Map<LoginResponseDto>(user);

            loggedInUser.Token = _tokenService.GenerateToken(_mapper.Map<UserClaimsDto>(loggedInUser));

            // Return success result
            return Result<LoginResponseDto>.Success(_mapper.Map<LoginResponseDto>(loggedInUser));
        }

        // Change user password
        public Task<Result<ChangePasswordDto>> ChangePasswordAsync(ChangePasswordDto dto)
        {
            throw new NotImplementedException();
        }

        // Refresh JWT token
        public Task<Result<RefreshTokenRequestDto>> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            throw new NotImplementedException();
        }

        // Get user profile details
        public Task<Result<UserProfileDto>> GetUserProfileAsync()
        {
            throw new NotImplementedException();
        }
    }
}