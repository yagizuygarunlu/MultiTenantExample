using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VisitorManagement.Application.Common.Interfaces;
using VisitorManagement.Domain.Entities;

namespace VisitorManagement.Application.Features.Visitors.Commands.CreateVisitor;

public class CreateVisitorCommandHandler : IRequestHandler<CreateVisitorCommand, CreateVisitorResponse>
{
    private readonly ITenantService _tenantService;
    private readonly ITenantDatabaseService _tenantDatabaseService;

    public CreateVisitorCommandHandler(ITenantService tenantService, ITenantDatabaseService tenantDatabaseService)
    {
        _tenantService = tenantService;
        _tenantDatabaseService = tenantDatabaseService;
    }

    public async Task<CreateVisitorResponse> Handle(CreateVisitorCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantService.GetCurrentTenant();
        var context = await _tenantService.GetTenantDbContextAsync(tenantId);

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
            CreatedAt = DateTime.UtcNow
        };

        context.Set<Visitor>().Add(visitor);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateVisitorResponse
        {
            Id = visitor.Id,
            FirstName = visitor.FirstName,
            LastName = visitor.LastName,
            Email = visitor.Email,
            PhoneNumber = visitor.PhoneNumber,
            Company = visitor.Company,
            Purpose = visitor.Purpose,
            VisitDate = visitor.VisitDate,
            Notes = visitor.Notes,
            CreatedAt = visitor.CreatedAt
        };
    }
} 