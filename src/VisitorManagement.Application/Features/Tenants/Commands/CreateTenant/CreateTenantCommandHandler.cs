using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VisitorManagement.Application.Common.Exceptions;
using VisitorManagement.Application.Common.Interfaces;
using VisitorManagement.Domain.Entities;

namespace VisitorManagement.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
{
    private readonly IMasterDbContext _context;
    private readonly ITenantDatabaseService _tenantDatabaseService;

    public CreateTenantCommandHandler(IMasterDbContext context, ITenantDatabaseService tenantDatabaseService)
    {
        _context = context;
        _tenantDatabaseService = tenantDatabaseService;
    }

    public async Task<CreateTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        // Check if tenant name already exists
        if (await _context.Tenants.AnyAsync(t => t.Name == request.Name, cancellationToken))
        {
            throw new ConflictException($"Tenant with name '{request.Name}' already exists");
        }

        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ConnectionString = request.ConnectionString,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        // Create tenant database
        await _tenantDatabaseService.CreateDatabaseAsync(tenant.Id, tenant.ConnectionString);

        return new CreateTenantResponse
        {
            Id = tenant.Id,
            Name = tenant.Name,
            IsActive = tenant.IsActive,
            CreatedAt = tenant.CreatedAt
        };
    }
} 