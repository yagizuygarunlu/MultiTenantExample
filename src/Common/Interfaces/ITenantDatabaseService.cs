namespace VisitorManagement.Application.Common.Interfaces;

public interface ITenantDatabaseService
{
    string BuildTenantConnectionString(string tenantName);
    Task CreateTenantDatabaseAsync(string tenantName, CancellationToken cancellationToken = default);
    Task InitializeTenantSchemaAsync(string connectionString, CancellationToken cancellationToken = default);
} 