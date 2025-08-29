namespace TaskManagement.Domain.Entities
{
    public class UserTask
    {
        public long UserId { get; set; } = default!;
        public User User { get; set; } = null!;

        public long TaskId { get; set; } = default!;
        public Task Task { get; set; } = null!;
    }
}
