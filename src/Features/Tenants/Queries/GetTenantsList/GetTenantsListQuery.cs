using MediatR;
using VisitorManagement.Domain.Entities;

namespace VisitorManagement.Application.Features.Tenants.Queries.GetTenantsList;

public record GetTenantsListQuery : IRequest<List<Tenant>>; 