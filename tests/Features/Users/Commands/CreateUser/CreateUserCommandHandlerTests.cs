using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shouldly;
using VisitorManagement.Application.Common.Exceptions;
using VisitorManagement.Application.Common.Interfaces;
using VisitorManagement.Application.Features.Users.Commands.CreateUser;
using VisitorManagement.Application.UnitTests.Common;
using VisitorManagement.Domain.Entities;
using Xunit;

namespace VisitorManagement.Application.UnitTests.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandlerTests : TestBase
{
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _handler = new CreateUserCommandHandler(Context, TenantService);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateUser()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123",
            FirstName = "Test",
            LastName = "User"
        };

        var users = new List<User>();
        var dbSetMock = Substitute.For<DbSet<User>>();
        
        dbSetMock.AnyAsync(
            Arg.Any<Func<User, bool>>(),
            Arg.Any<CancellationToken>()
        ).Returns(false);

        Context.Users.Returns(dbSetMock);
        Context.Users.Add(Arg.Do<User>(user => users.Add(user)));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Username.ShouldBe(command.Username);
        result.Email.ShouldBe(command.Email);
        result.FirstName.ShouldBe(command.FirstName);
        result.LastName.ShouldBe(command.LastName);

        users.Count.ShouldBe(1);
        var createdUser = users[0];
        createdUser.TenantId.ShouldBe(CurrentTenantId);
        createdUser.IsActive.ShouldBeTrue();
        createdUser.PasswordHash.ShouldNotBe(command.Password); // Password should be hashed
    }

    [Fact]
    public async Task Handle_DuplicateUsername_ShouldThrowConflictException()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Username = "existinguser",
            Email = "test@example.com",
            Password = "password123",
            FirstName = "Test",
            LastName = "User"
        };

        var dbSetMock = Substitute.For<DbSet<User>>();
        dbSetMock.AnyAsync(
            Arg.Any<Func<User, bool>>(),
            Arg.Any<CancellationToken>()
        ).Returns(true);

        Context.Users.Returns(dbSetMock);

        // Act & Assert
        await Should.ThrowAsync<ConflictException>(
            async () => await _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_NoTenantContext_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123",
            FirstName = "Test",
            LastName = "User"
        };

        TenantService.GetCurrentTenant()
            .Returns(x => { throw new UnauthorizedException("No tenant context set"); });

        // Act & Assert
        await Should.ThrowAsync<UnauthorizedException>(
            async () => await _handler.Handle(command, CancellationToken.None)
        );
    }
} 