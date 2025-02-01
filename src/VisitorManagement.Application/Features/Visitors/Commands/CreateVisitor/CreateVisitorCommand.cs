using System;
using MediatR;

namespace VisitorManagement.Application.Features.Visitors.Commands.CreateVisitor;

public record CreateVisitorCommand : IRequest<CreateVisitorResponse>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string Purpose { get; init; } = string.Empty;
    public DateTime VisitDate { get; init; }
    public string Notes { get; init; } = string.Empty;
} 