using System;
using System.Collections.Generic;
using MediatR;

namespace VisitorManagement.Application.Features.Visitors.Queries.GetVisitorsList;

public record GetVisitorsListQuery : IRequest<List<GetVisitorsListResponse>>
{
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public string? SearchTerm { get; init; }
    public string? Status { get; init; }
}

public record GetVisitorsListResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string Purpose { get; init; } = string.Empty;
    public DateTime VisitDate { get; init; }
    public string Notes { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
} 