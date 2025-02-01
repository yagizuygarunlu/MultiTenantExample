using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VisitorManagement.Application.Common.Interfaces;
using VisitorManagement.Infrastructure.Data;

namespace VisitorManagement.Infrastructure.Services;

public class TenantDatabaseService : ITenantDatabaseService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public TenantDatabaseService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateDatabaseAsync(Guid tenantId, string connectionString)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        await context.Database.MigrateAsync();
    }

    public async Task DeleteDatabaseAsync(Guid tenantId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        await context.Database.EnsureDeletedAsync();
    }
} 