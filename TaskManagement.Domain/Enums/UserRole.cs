namespace TaskManagement.Domain.Enums
{
    public enum UserRole
    {
        Admin = 1,     // Only via Signup
        Employee = 2   // Can be created by Admin or Signup
    }
}
