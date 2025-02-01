using System;
using NSubstitute;
using VisitorManagement.Application.Common.Interfaces;

namespace VisitorManagement.Application.UnitTests.Common;

public abstract class TestBase
{
    protected readonly IMasterDbContext Context;
    protected readonly ITenantService TenantService;
    protected readonly Guid CurrentTenantId;

    protected TestBase()
    {
        Context = Substitute.For<IMasterDbContext>();
        TenantService = Substitute.For<ITenantService>();
        CurrentTenantId = Guid.NewGuid();

        // Setup default tenant service behavior
        TenantService.GetCurrentTenant().Returns(CurrentTenantId);
    }
} 