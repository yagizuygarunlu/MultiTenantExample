using MediatR;
using Microsoft.EntityFrameworkCore;
using VisitorManagement.Domain.Entities;
using VisitorManagement.Application.Common.Interfaces;

namespace VisitorManagement.Application.Features.Tenants.Queries.GetTenantsList;

public class GetTenantsListQueryHandler : IRequestHandler<GetTenantsListQuery, List<Tenant>>
{
    private readonly IMasterDbContext _masterDbContext;

    public GetTenantsListQueryHandler(IMasterDbContext masterDbContext)
    {
        _masterDbContext = masterDbContext;
    }

    public async Task<List<Tenant>> Handle(GetTenantsListQuery request, CancellationToken cancellationToken)
    {
        return await _masterDbContext.Tenants
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }
} 