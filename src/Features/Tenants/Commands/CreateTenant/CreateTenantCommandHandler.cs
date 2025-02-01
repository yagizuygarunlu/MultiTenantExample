using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VisitorManagement.Domain.Entities;
using VisitorManagement.Application.Common.Interfaces;
using VisitorManagement.Application.Common.Exceptions;

namespace VisitorManagement.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Tenant>
{
    private readonly IMasterDbContext _masterDbContext;
    private readonly ITenantDatabaseService _tenantDatabaseService;
    private readonly IConfiguration _configuration;

    public CreateTenantCommandHandler(
        IMasterDbContext masterDbContext,
        ITenantDatabaseService tenantDatabaseService,
        IConfiguration configuration)
    {
        _masterDbContext = masterDbContext;
        _tenantDatabaseService = tenantDatabaseService;
        _configuration = configuration;
    }

    public async Task<Tenant> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Tenant name cannot be empty.");

        var tenantName = request.Name.ToLower();
        
        // Check if tenant already exists
        var existingTenant = await _masterDbContext.Tenants
            .FirstOrDefaultAsync(t => t.Name.ToLower() == tenantName, cancellationToken);
            
        if (existingTenant != null)
            throw new ConflictException($"Tenant with name '{request.Name}' already exists.");

        var tenantConnectionString = _tenantDatabaseService.BuildTenantConnectionString(tenantName);
        var tenant = await CreateTenantEntityAsync(request.Name, tenantConnectionString, cancellationToken);
        
        await _tenantDatabaseService.CreateTenantDatabaseAsync(tenantName, cancellationToken);
        await _tenantDatabaseService.InitializeTenantSchemaAsync(tenantConnectionString, cancellationToken);

        return tenant;
    }

    private async Task<Tenant> CreateTenantEntityAsync(string name, string connectionString, CancellationToken cancellationToken)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name,
            ConnectionString = connectionString,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _masterDbContext.Tenants.AddAsync(tenant, cancellationToken);
        await _masterDbContext.SaveChangesAsync(cancellationToken);

        return tenant;
    }
} 