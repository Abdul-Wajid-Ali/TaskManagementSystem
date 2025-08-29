using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs.Auth
{
    public class ChangePasswordDto
    {
        [MaxLength(1000)]
        public string CurrentPassword { get; set; } = default!;

        [MaxLength(1000)]
        public string NewPassword { get; set; } = default!;
    }
}