using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Tests.Services;

public class AuthServiceTests
{
    private readonly SignInManager<User> _signInManager;
    private readonly IUserService _userService = Substitute.For<IUserService>();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _signInManager = CreateSignInManagerSubstitute();
        _sut = new AuthService(_signInManager, _userService);
    }

    private static SignInManager<User> CreateSignInManagerSubstitute()
    {
        var userStore = Substitute.For<IUserStore<User>>();
        var userManager = Substitute.For<UserManager<User>>(
            userStore, null, null, null, null, null, null, null, null);
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        var claimsPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<User>>();
        return Substitute.For<SignInManager<User>>(
            userManager, httpContextAccessor, claimsPrincipalFactory, null, null, null, null);
    }

    private static User NewUser() => new()
    {
        Id = Guid.NewGuid(),
        Email = "user@test.com",
        DisplayName = "Test User",
        Image = null
    };

    // ── RegisterAsync ────────────────────────────────────────────────────────────

    [Fact]
    public async Task RegisterAsync_DelegatesToUserService()
    {
        await _sut.RegisterAsync("new@test.com", "password123", "New User");

        await _userService.Received(1).CreateUserAsync("new@test.com", "password123", "New User");
    }

    // ── LoginAsync ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsLoginResponse()
    {
        var user = NewUser();
        _userService.FindUserByEmailAsync(user.Email!).Returns(user);
        _signInManager
            .PasswordSignInAsync(user, Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(SignInResult.Success);

        var result = await _sut.LoginAsync(user.Email!, "correct-password");

        result.Id.Should().Be(user.Id);
        result.Email.Should().Be(user.Email);
        result.DisplayName.Should().Be(user.DisplayName);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ThrowsInvalidCredentialsException()
    {
        _userService.FindUserByEmailAsync(Arg.Any<string>()).Returns((User?)null);

        var act = async () => await _sut.LoginAsync("nobody@test.com", "pass");

        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ThrowsInvalidCredentialsException()
    {
        var user = NewUser();
        _userService.FindUserByEmailAsync(user.Email!).Returns(user);
        _signInManager
            .PasswordSignInAsync(user, Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(SignInResult.Failed);

        var act = async () => await _sut.LoginAsync(user.Email!, "wrong-password");

        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_DoesNotReturnLoginResponse()
    {
        var user = NewUser();
        _userService.FindUserByEmailAsync(user.Email!).Returns(user);
        _signInManager
            .PasswordSignInAsync(user, Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(SignInResult.Failed);

        var act = async () => await _sut.LoginAsync(user.Email!, "wrong-password");

        await act.Should().ThrowAsync<InvalidCredentialsException>();
        await _userService.DidNotReceive().CreateUserAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string?>());
    }

    // ── LogoutAsync ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task LogoutAsync_DelegatesToSignInManager()
    {
        await _sut.LogoutAsync();

        await _signInManager.Received(1).SignOutAsync();
    }
}
