using MediatR;
using VisitorManagement.Domain.Entities;

namespace VisitorManagement.Application.Features.Tenants.Commands.CreateTenant;

public record CreateTenantCommand : IRequest<Tenant>
{
    public required string Name { get; init; }
} 