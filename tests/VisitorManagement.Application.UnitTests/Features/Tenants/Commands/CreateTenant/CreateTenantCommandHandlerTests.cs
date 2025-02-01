using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NSubstitute;
using Shouldly;
using VisitorManagement.Application.Common.Exceptions;
using VisitorManagement.Application.Features.Tenants.Commands.CreateTenant;
using VisitorManagement.Application.UnitTests.Common;
using VisitorManagement.Domain.Entities;
using Xunit;

namespace VisitorManagement.Application.UnitTests.Features.Tenants.Commands.CreateTenant;

public class CreateTenantCommandHandlerTests : TestBase
{
    private readonly CreateTenantCommandHandler _handler;

    public CreateTenantCommandHandlerTests()
    {
        _handler = new CreateTenantCommandHandler(MasterContext, TenantDatabaseService);
    }

    [Fact]
    public async Task Handle_ShouldCreateTenant_WhenNameIsUnique()
    {
        // Arrange
        var command = new CreateTenantCommand
        {
            Name = "Test Tenant",
            ConnectionString = "Server=localhost;Database=test_db"
        };

        var tenants = new List<Tenant>();
        var dbSetMock = Substitute.For<DbSet<Tenant>>();
        
        dbSetMock.AddAsync(Arg.Any<Tenant>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                var tenant = callInfo.Arg<Tenant>();
                tenants.Add(tenant);
                var entityEntry = Substitute.For<EntityEntry<Tenant>>();
                entityEntry.Entity.Returns(tenant);
                return new ValueTask<EntityEntry<Tenant>>(entityEntry);
            });

        MasterContext.Tenants.Returns(dbSetMock);
        MasterContext.Tenants.AnyAsync(
            Arg.Any<System.Linq.Expressions.Expression<Func<Tenant, bool>>>(), 
            Arg.Any<CancellationToken>()
        ).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe(command.Name);
        result.IsActive.ShouldBeTrue();
        result.Id.ShouldNotBe(Guid.Empty);

        await TenantDatabaseService.Received(1)
            .CreateDatabaseAsync(result.Id, command.ConnectionString);
    }

    [Fact]
    public async Task Handle_ShouldThrowConflictException_WhenNameExists()
    {
        // Arrange
        var command = new CreateTenantCommand
        {
            Name = "Existing Tenant",
            ConnectionString = "Server=localhost;Database=test_db"
        };

        MasterContext.Tenants.AnyAsync(
            Arg.Any<System.Linq.Expressions.Expression<Func<Tenant, bool>>>(), 
            Arg.Any<CancellationToken>()
        ).Returns(true);

        // Act & Assert
        await Should.ThrowAsync<ConflictException>(
            async () => await _handler.Handle(command, CancellationToken.None)
        );
    }
} 