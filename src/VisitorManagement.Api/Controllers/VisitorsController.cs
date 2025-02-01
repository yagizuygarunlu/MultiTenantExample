using MediatR;
using Microsoft.AspNetCore.Mvc;
using VisitorManagement.Application.Features.Visitors.Commands.CreateVisitor;
using VisitorManagement.Application.Features.Visitors.Queries.GetVisitorsList;

namespace VisitorManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitorsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly Guid _tenantId;

    public VisitorsController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _tenantId = httpContextAccessor.HttpContext?.Items["TenantId"] as Guid? ?? Guid.Empty;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateVisitor(CreateVisitorCommand command)
    {
        var visitorId = await _mediator.Send(command);
        return Ok(visitorId);
    }

    [HttpGet]
    public async Task<ActionResult<List<Domain.Entities.Visitor>>> GetVisitors(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] string? searchTerm,
        [FromQuery] string? status)
    {
        var query = new GetVisitorsListQuery
        {
            FromDate = fromDate,
            ToDate = toDate,
            SearchTerm = searchTerm,
            Status = status
        };

        var visitors = await _mediator.Send(query);
        return Ok(visitors);
    }
} 