using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VisitorManagement.Application.Common.Exceptions;
using VisitorManagement.Application.Common.Interfaces;
using VisitorManagement.Domain.Entities;
using VisitorManagement.Infrastructure.Data;

namespace VisitorManagement.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly IConfiguration _configuration;
    private readonly IMasterDbContext _masterDbContext;
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private Guid? _currentTenantId;

    public TenantService(
        IConfiguration configuration,
        IMasterDbContext masterDbContext,
        IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _configuration = configuration;
        _masterDbContext = masterDbContext;
        _contextFactory = contextFactory;
    }

    public string? GetConnectionString()
    {
        return _configuration.GetConnectionString("MasterDatabase");
    }

    public void SetTenant(string tenant)
    {
        var tenantEntity = _masterDbContext.Tenants
            .FirstOrDefault(t => t.Name == tenant);

        if (tenantEntity == null)
        {
            throw new NotFoundException($"Tenant {tenant} not found");
        }

        _currentTenantId = tenantEntity.Id;
    }

    public Guid GetCurrentTenant()
    {
        if (_currentTenantId == null)
        {
            throw new UnauthorizedException("No tenant context set");
        }

        return _currentTenantId.Value;
    }

    public async Task<DbContext> GetTenantDbContextAsync(Guid tenantId)
    {
        var tenant = await GetTenantAsync(tenantId);
        return await _contextFactory.CreateDbContextAsync();
    }

    public async Task<Tenant> GetTenantAsync(Guid tenantId)
    {
        var tenant = await _masterDbContext.Tenants.FindAsync(tenantId);
        if (tenant == null)
        {
            throw new NotFoundException($"Tenant with ID {tenantId} not found");
        }
        return tenant;
    }
} 