using System.Collections.Generic;
using MediatR;

namespace VisitorManagement.Application.Features.Tenants.Queries.GetTenantsList;

public record GetTenantsListQuery : IRequest<List<GetTenantsListResponse>>;

public record GetTenantsListResponse
{
    public string Name { get; init; } = string.Empty;
    public bool IsActive { get; init; }
} 