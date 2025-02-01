using MediatR;
using Microsoft.AspNetCore.Http;
using VisitorManagement.Domain.Entities;
using VisitorManagement.Application.Common.Interfaces;

namespace VisitorManagement.Application.Features.Visitors.Commands.CreateVisitor;

public class CreateVisitorCommandHandler : IRequestHandler<CreateVisitorCommand, Guid>
{
    private readonly ITenantService _tenantService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateVisitorCommandHandler(ITenantService tenantService, IHttpContextAccessor httpContextAccessor)
    {
        _tenantService = tenantService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid> Handle(CreateVisitorCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _httpContextAccessor.HttpContext?.Items["TenantId"] as Guid? ?? throw new Exception("Tenant ID not found");
        var dbContext = await _tenantService.GetTenantDbContextAsync(tenantId);

        var visitor = new Visitor
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Company = request.Company,
            Purpose = request.Purpose,
            VisitDate = request.VisitDate,
            Notes = request.Notes,
            Status = "Scheduled",
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.Set<Visitor>().AddAsync(visitor, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return visitor.Id;
    }
} 