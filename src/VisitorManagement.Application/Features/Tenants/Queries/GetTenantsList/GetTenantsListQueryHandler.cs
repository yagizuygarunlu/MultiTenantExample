using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VisitorManagement.Application.Common.Interfaces;

namespace VisitorManagement.Application.Features.Tenants.Queries.GetTenantsList;

public class GetTenantsListQueryHandler : IRequestHandler<GetTenantsListQuery, List<GetTenantsListResponse>>
{
    private readonly IMasterDbContext _context;

    public GetTenantsListQueryHandler(IMasterDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetTenantsListResponse>> Handle(GetTenantsListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tenants
            .Select(t => new GetTenantsListResponse
            {
                Name = t.Name,
                IsActive = t.IsActive
            })
            .ToListAsync(cancellationToken);
    }
} 