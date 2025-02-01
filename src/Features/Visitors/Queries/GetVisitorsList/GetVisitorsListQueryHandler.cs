using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using VisitorManagement.Domain.Entities;
using VisitorManagement.Application.Common.Interfaces;

namespace VisitorManagement.Application.Features.Visitors.Queries.GetVisitorsList;

public class GetVisitorsListQueryHandler : IRequestHandler<GetVisitorsListQuery, List<Visitor>>
{
    private readonly ITenantService _tenantService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetVisitorsListQueryHandler(ITenantService tenantService, IHttpContextAccessor httpContextAccessor)
    {
        _tenantService = tenantService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Visitor>> Handle(GetVisitorsListQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _httpContextAccessor.HttpContext?.Items["TenantId"] as Guid? ?? throw new Exception("Tenant ID not found");
        var dbContext = await _tenantService.GetTenantDbContextAsync(tenantId);
        var query = dbContext.Set<Visitor>().AsQueryable();

        if (request.FromDate.HasValue)
        {
            query = query.Where(v => v.VisitDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(v => v.VisitDate <= request.ToDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            query = query.Where(v => v.Status == request.Status);
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

        return await query.OrderByDescending(v => v.VisitDate)
            .ToListAsync(cancellationToken);
    }
} 