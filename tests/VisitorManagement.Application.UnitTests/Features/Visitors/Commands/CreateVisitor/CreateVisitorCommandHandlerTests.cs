using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shouldly;
using VisitorManagement.Application.Features.Visitors.Commands.CreateVisitor;
using VisitorManagement.Application.UnitTests.Common;
using VisitorManagement.Domain.Entities;
using Xunit;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace VisitorManagement.Application.UnitTests.Features.Visitors.Commands.CreateVisitor;

public class CreateVisitorCommandHandlerTests : TestBase
{
    private readonly CreateVisitorCommandHandler _handler;

    public CreateVisitorCommandHandlerTests()
    {
        _handler = new CreateVisitorCommandHandler(TenantService, TenantDatabaseService);
    }

    [Fact]
    public async Task Handle_ShouldCreateVisitor_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateVisitorCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Company = "Test Company",
            Purpose = "Meeting",
            VisitDate = DateTime.UtcNow.AddDays(1),
            Notes = "Test notes"
        };

        var dbSetMock = Substitute.For<DbSet<Visitor>>();
        dbSetMock.AddAsync(Arg.Any<Visitor>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                var visitor = callInfo.Arg<Visitor>();
                var entityEntry = Substitute.For<EntityEntry<Visitor>>();
                entityEntry.Entity.Returns(visitor);
                return new ValueTask<EntityEntry<Visitor>>(entityEntry);
            });

        TenantContext.Set<Visitor>().Returns(dbSetMock);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.FirstName.ShouldBe(command.FirstName);
        result.LastName.ShouldBe(command.LastName);
        result.Email.ShouldBe(command.Email);
        result.PhoneNumber.ShouldBe(command.PhoneNumber);
        result.Company.ShouldBe(command.Company);
        result.Purpose.ShouldBe(command.Purpose);
        result.VisitDate.ShouldBe(command.VisitDate);
        result.Notes.ShouldBe(command.Notes);
        result.Id.ShouldNotBe(Guid.Empty);

        await TenantContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
} 