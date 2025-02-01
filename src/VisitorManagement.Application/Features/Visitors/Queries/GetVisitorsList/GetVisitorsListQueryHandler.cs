using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VisitorManagement.Application.Common.Interfaces;
using VisitorManagement.Domain.Entities;

namespace VisitorManagement.Application.Features.Visitors.Queries.GetVisitorsList;

public class GetVisitorsListQueryHandler : IRequestHandler<GetVisitorsListQuery, List<GetVisitorsListResponse>>
{
    private readonly ITenantService _tenantService;

    public GetVisitorsListQueryHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public async Task<List<GetVisitorsListResponse>> Handle(GetVisitorsListQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantService.GetCurrentTenant();
        var context = await _tenantService.GetTenantDbContextAsync(tenantId);

        var query = context.Set<Visitor>().AsQueryable();

        if (request.FromDate.HasValue)
        {
            query = query.Where(v => v.VisitDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(v => v.VisitDate <= request.ToDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(v =>
                v.FirstName.ToLower().Contains(searchTerm) ||
                v.LastName.ToLower().Contains(searchTerm) ||
                v.Email.ToLower().Contains(searchTerm) ||
                v.Company.ToLower().Contains(searchTerm)
            );
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(v => v.Status == request.Status);
        }

        return await query
            .Select(v => new GetVisitorsListResponse
            {
                Id = v.Id,
                FirstName = v.FirstName,
                LastName = v.LastName,
                Email = v.Email,
                PhoneNumber = v.PhoneNumber,
                Company = v.Company,
                Purpose = v.Purpose,
                VisitDate = v.VisitDate,
                Notes = v.Notes,
                CreatedAt = v.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
} 