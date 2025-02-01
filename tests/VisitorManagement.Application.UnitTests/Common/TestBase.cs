using System;
using NSubstitute;
using VisitorManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace VisitorManagement.Application.UnitTests.Common;

public abstract class TestBase
{
    protected readonly IMasterDbContext MasterContext;
    protected readonly ITenantService TenantService;
    protected readonly ITenantDatabaseService TenantDatabaseService;
    protected readonly DbContext TenantContext;
    protected readonly Guid CurrentTenantId;

    protected TestBase()
    {
        MasterContext = Substitute.For<IMasterDbContext>();
        TenantService = Substitute.For<ITenantService>();
        TenantDatabaseService = Substitute.For<ITenantDatabaseService>();
        TenantContext = Substitute.For<DbContext>();
        CurrentTenantId = Guid.NewGuid();

        // Setup default tenant service behavior
        TenantService.GetCurrentTenant().Returns(CurrentTenantId);
        TenantService.GetTenantDbContextAsync(Arg.Any<Guid>()).Returns(TenantContext);
    }
} 