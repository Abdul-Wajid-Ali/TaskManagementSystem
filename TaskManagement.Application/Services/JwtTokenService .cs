using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Application.Config;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        // Generate JWT token based on user claims
        public string GenerateToken(UserClaimsDto dto)
        {
            // Build list of claims (user identity + metadata)
            var claims = new List<Claim>
{
            new Claim(ClaimTypes.Sid, dto.Id.ToString()),
            new Claim(ClaimTypes.Email, dto.Email),
            new Claim(ClaimTypes.Name, dto.Username),
            new Claim(ClaimTypes.Role, dto.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};

            // Create signing key & credentials (HMAC SHA-256)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate the JWT token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,              // Token issuer
                audience: _jwtSettings.Audience,          // Token audience
                claims: claims,                           // Payload claims
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes), // Expiration
                signingCredentials: creds                 // Digital signature
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generate a secure random refresh token
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}