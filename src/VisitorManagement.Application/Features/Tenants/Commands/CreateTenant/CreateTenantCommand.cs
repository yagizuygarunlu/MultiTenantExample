using MediatR;

namespace VisitorManagement.Application.Features.Tenants.Commands.CreateTenant;

public record CreateTenantCommand : IRequest<CreateTenantResponse>
{
    public string Name { get; init; } = string.Empty;
    public string ConnectionString { get; init; } = string.Empty;
} 