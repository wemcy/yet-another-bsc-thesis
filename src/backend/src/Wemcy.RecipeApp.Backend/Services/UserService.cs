using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Security;

namespace Wemcy.RecipeApp.Backend.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IImageService imageService, IAuthorizationService authorizationService, RoleManager<IdentityRole<Guid>> roleManager) : IUserService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IAuthorizationService _authorizationService = authorizationService;
    private readonly IImageService _imageService = imageService;


    public async Task<User> GetCurrentUserAsync()
    {
        var user = await _userManager.FindByIdAsync(GetCurrentUserId().ToString());
        return user ?? throw new UnauthorizedAccessException("Authenticated user no longer exists.");
    }

    public async Task EnsureCurrentUserCanAsync<T>(OperationAuthorizationRequirement operation, T resource)
    {
        var result = await _authorizationService.AuthorizeAsync(GetCurrentUserPrincipal(), resource, operation);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("You are not allowed to modify this resource.");
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString()) ?? throw new UserNotFoundException(id);
    }

    public async Task CreateAdminUserAsync()
    {
        await EnsureRoleCreated(Roles.Admin);

        const string defaultAdminEmail = "admin@recipe-app.local";
        const string defaultAdminPassword = "Admin123!";
        const string defaultAdminDisplayName = "Administrator";

        var defaultAdmin = await CreateUserIfNotExist(defaultAdminEmail, defaultAdminPassword, defaultAdminDisplayName);

        await AddRoleToUser(defaultAdmin, Roles.Admin);
    }

    private async Task AddRoleToUser(User appUser, string role)
    {
        if (!await _userManager.IsInRoleAsync(appUser, role))
        {
            var roleResult = await _userManager.AddToRoleAsync(appUser, role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not assign {role} role to user {appUser.UserName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }

    private async Task<User> CreateUserIfNotExist(string email, string password, string displayName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is not null)
            return user;

        var newUser = new User
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            DisplayName = displayName,
            Image = null,
        };

        var createResult = await _userManager.CreateAsync(newUser, password);
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException(
                $"Could not create user {email}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
        }
        return newUser;
    }

    public async Task CreateUserAsync(string email, string password, string? displayName)
    {
        var user = new User
        {
            UserName = email,
            Email = email,
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? email : displayName.Trim(),
            Image = null,
        };

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            throw new RegistrationExeption([.. createResult.Errors]);
        }
    }

    private async Task EnsureRoleCreated(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }
    public async Task<Stream> GetProfileImageByIdAsync(Guid id, ImageSize imageSize)
    {
        var user = await GetUserByIdAsync(id);
        var image = user.Image ?? throw new ImageNotFoundException("User does not have profile picture");
        return await _imageService.GetImageById(image.Id, imageSize);
    }



    public async Task UpdateProfileByIdAsync(Guid id, UserProfileUpdateRequest request)
    {
        var user = await GetUserByIdAsync(id);
        await EnsureCurrentUserCanAsync(Operations.Update, user);
        if (request.HasImageUpdate)
        {
            var image = await _imageService.CreateImage(request.ImageStream, request.ImageName);
            user.Image = image;
        }
        if (request.HasDisplayNameUpdate)
        {
            user.DisplayName = request.DisplayName;
        }
        if (request.HasEmailUpdate)
        {
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
            await _userManager.ChangeEmailAsync(user, request.Email, token);
            await _userManager.SetUserNameAsync(user, request.Email);
        }
        if (request.HasPasswordUpdate)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, request.Password);
        }
        await _userManager.UpdateAsync(user);
    }

    public async Task DeleteUserByIdAsync(Guid id)
    {
        var user = await GetUserByIdAsync(id);
        await EnsureCurrentUserCanAsync(Operations.Delete, user);
        await _userManager.UpdateSecurityStampAsync(user);
        await _userManager.DeleteAsync(user);
    }

    public async Task AddRoleToUserAsync(Guid id, AddUserRoleByIdRequest addUserRoleByIdRequest)
    {
        var user = await GetUserByIdAsync(id);
        await EnsureCurrentUserCanAsync(Operations.Update, user);
        if (addUserRoleByIdRequest.Role != UserRole.AdminEnum)
            throw new UnknowRoleExeption(addUserRoleByIdRequest.Role.ToString()); // TODO better exception
        if (!await _userManager.IsInRoleAsync(user, Security.Roles.Admin))
            await _userManager.AddToRoleAsync(user, Security.Roles.Admin);
    }

    private ClaimsPrincipal GetCurrentUserPrincipal()
    {
        return _httpContextAccessor?.HttpContext?.User ?? throw new UnauthorizedAccessException();
    }

    private Guid GetCurrentUserId()
    {
        var userIdValue = GetCurrentUserPrincipal().FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdValue is null || !Guid.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException("Authenticated user id is missing or invalid.");

        return userId;
    }
}
