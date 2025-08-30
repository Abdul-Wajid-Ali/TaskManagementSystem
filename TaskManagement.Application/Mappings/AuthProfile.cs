using AutoMapper;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappings
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            // RegisterRequestDto -> Entity
            CreateMap<RegisterRequestDto, User>();

            // Entity -> RegisterRequestDto
            CreateMap<User, RegisterRequestDto>();

            // Map User -> UserClaimsDto
            CreateMap<LoginResponseDto, UserClaimsDto>();

            // Map UserDto -> LoginResponseDto
            CreateMap<User, LoginResponseDto>();
        }
    }
}