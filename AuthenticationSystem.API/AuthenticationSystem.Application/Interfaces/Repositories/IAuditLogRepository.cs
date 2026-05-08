using AuthenticationSystem.Domain.Entities;

namespace AuthenticationSystem.Application.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log);
    Task<IEnumerable<AuditLog>> GetByUserIdAsync(Guid userId);
}
