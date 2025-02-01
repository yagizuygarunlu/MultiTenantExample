using MediatR;

namespace VisitorManagement.Application.Features.Visitors.Commands.CreateVisitor;

public class CreateVisitorCommand : IRequest<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Company { get; set; }
    public string Purpose { get; set; }
    public DateTime VisitDate { get; set; }
    public string Notes { get; set; }
} 