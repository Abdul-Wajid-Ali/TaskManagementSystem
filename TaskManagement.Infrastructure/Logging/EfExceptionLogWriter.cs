using TaskManagement.Application.Interfaces.Logging;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Logging;

public class EfExceptionLogWriter : IExceptionLogWriter
{
    private readonly AppDbContext _context;

    public EfExceptionLogWriter(AppDbContext context) => _context = context;

    public async Task WriteAsync(ExceptionLog log)
    {
        try
        {
            await _context.ExceptionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            // Logging failure is non-critical, so we silently ignore any exceptions here.
            // In a real-world scenario, consider logging this to a file or external system.
        }
    }
}
