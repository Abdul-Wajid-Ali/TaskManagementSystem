using AutoMapper;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _tokenService;
        private readonly IUserRepository _repository;
        private readonly IPasswordService _passwordService;

        public AuthService(IMapper mapper, IUserRepository repository, IPasswordService passwordService, IJwtTokenService tokenService)
        {
            _mapper = mapper;
            _repository = repository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        // Register a new user and return the registered user's details
        public async Task<RegisterRequestDto> RegisterUserAsync(RegisterRequestDto dto)
        {
            var newUser = _mapper.Map<User>(dto);

            newUser.Role = UserRole.Admin;
            newUser.CreationMethod = UserCreationMethod.Registered;
            newUser.PasswordSalt = _passwordService.GenerateSalt();
            newUser.PasswordHash = _passwordService.HashPassword(dto.Password, newUser.PasswordSalt);

            await _repository.CreateUserAsync(newUser);

            return _mapper.Map<RegisterRequestDto>(newUser);
        }

        // Authenticate user and return login response with JWT token
        public async Task<LoginResponseDto?> LoginUserAsync(LoginRequestDto dto)
        {
            var user = await _repository.GetUserByEmailAsync(dto.Email);

            var loginUser = _mapper.Map<LoginResponseDto>(user);

            if (user == null)
                return null;

            loginUser.Token = _tokenService.GenerateToken(_mapper.Map<UserClaimsDto>(loginUser));

            return loginUser;
        }

        // Change user password
        public Task<ChangePasswordDto> ChangePasswordAsync(ChangePasswordDto dto)
        {
            throw new NotImplementedException();
        }

        // Refresh JWT token
        public Task<RefreshTokenRequestDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            throw new NotImplementedException();
        }

        // Get user profile details
        public Task<UserProfileDto> GetUserProfileAsync()
        {
            throw new NotImplementedException();
        }
    }
}