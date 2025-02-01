using System;

namespace VisitorManagement.Application.Features.Tenants.Commands.CreateTenant;

public record CreateTenantResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
} 