namespace TaskManagement.Domain.Entities
{
    public class UserTask
    {
        public long UserId { get; set; }
        public User User { get; set; } = null!;

        public long TaskId { get; set; }
        public Task Task { get; set; } = null!;
    }
}
