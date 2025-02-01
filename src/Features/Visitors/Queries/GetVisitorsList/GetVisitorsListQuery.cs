using MediatR;
using VisitorManagement.Domain.Entities;

namespace VisitorManagement.Application.Features.Visitors.Queries.GetVisitorsList;

public class GetVisitorsListQuery : IRequest<List<Visitor>>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
} 