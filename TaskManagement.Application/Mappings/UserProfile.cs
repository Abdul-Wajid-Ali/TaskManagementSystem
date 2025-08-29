using AutoMapper;
using TaskManagement.Application.DTOs.Users;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Entity -> DTO
            CreateMap<User, UserDto>();

            // CreateUserDto -> Entity
            CreateMap<CreateUserDto, User>();

            // UpdateUserDto -> Entity
            CreateMap<UpdateUserDto, User>();
        }
    }
}