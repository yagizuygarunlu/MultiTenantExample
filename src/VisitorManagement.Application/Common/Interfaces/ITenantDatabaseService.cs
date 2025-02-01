using System;
using System.Threading.Tasks;

namespace VisitorManagement.Application.Common.Interfaces;

public interface ITenantDatabaseService
{
    Task CreateDatabaseAsync(Guid tenantId, string connectionString);
    Task DeleteDatabaseAsync(Guid tenantId);
} 