using TaskManagement.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Interfaces.Logging;

public interface IExceptionLogWriter
{
    Task WriteAsync(ExceptionLog log);
}
