using VisitorManagement.Application.Common.Interfaces;
using VisitorManagement.Infrastructure.Services;

namespace VisitorManagement.Api.Services;

public interface ITenantServiceFactory
{
    ITenantService Create(Guid tenantId);
}

public class TenantServiceFactory : ITenantServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public TenantServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ITenantService Create(Guid tenantId)
    {
        var scope = _serviceProvider.CreateScope();
        var tenantService = ActivatorUtilities.CreateInstance<TenantService>(scope.ServiceProvider);
        return tenantService;
    }
} 