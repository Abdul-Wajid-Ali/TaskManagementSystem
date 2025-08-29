using AutoMapper;
using TaskManagement.Application.DTOs.Tasks;
using Task = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Mapping
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            // Entity -> DTO
            CreateMap<Task, TaskDto>()
                .ForMember(dest => dest.AssignedUserIds,opt => opt.MapFrom(src => src.UserTasks.Select(ut => ut.UserId).ToList()));

            // CreateTaskDto -> Entity
            CreateMap<CreateTaskDto, Task>();

            // UpdateTaskDto -> Entity
            CreateMap<UpdateTaskDto, Task>();
        }
    }
}