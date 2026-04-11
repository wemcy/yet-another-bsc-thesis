using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Security;

namespace Wemcy.RecipeApp.Backend.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ImageService imageService, IAuthorizationService authorizationService, RoleManager<IdentityRole<Guid>> roleManager) : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IAuthorizationService _authorizationService = authorizationService;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
    private readonly ImageService _imageService = imageService;

    public ClaimsPrincipal GetCurrentUser()
    {
        return _httpContextAccessor?.HttpContext?.User ?? throw new UnauthorizedAccessException();
    }

    public Guid GetCurrentUserId()
    {
        var userIdValue = GetCurrentUser().FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdValue is null || !Guid.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException("Authenticated user id is missing or invalid.");

        return userId;
    }

    public async Task<User> GetCurrentUserEntityAsync()
    {
        var user = await _userManager.FindByIdAsync(GetCurrentUserId().ToString());
        return user ?? throw new UnauthorizedAccessException("Authenticated user no longer exists.");
    }

    public async Task EnsureAuthorizedAsync<T>(T resource, OperationAuthorizationRequirement operation)
    {
        var result = await _authorizationService.AuthorizeAsync(GetCurrentUser(), resource, operation);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("You are not allowed to modify this resource.");
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString()) ?? throw new UserNotFoundException(id);
    }

    public async Task CreateAdminUser()
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

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }
    public async Task<Stream> GetProfileImageById(Guid id)
    {
        var user = await GetUserByIdAsync(id);
        var image = user.Image ?? throw new ImageNotFoundException("User does not have profile picture");
        return _imageService.GetImageById(image.Id);
    }
    public async Task UpdateProfileByIdAsync(Guid id, UserProfileUpdateRequest request)
    {
        var user = await GetUserByIdAsync(id);
        await EnsureAuthorizedAsync(user, Operations.Update);
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

    public async Task DeleteProfileByIdAsync(Guid id)
    {
        var user = await GetUserByIdAsync(id);
        await EnsureAuthorizedAsync(user, Operations.Delete);
        await _userManager.UpdateSecurityStampAsync(user);
        await _userManager.DeleteAsync(user);
    }

    public async Task AddRoleToUserAsync(Guid id, AddUserRoleByIdRequest addUserRoleByIdRequest)
    {
        var user = await GetUserByIdAsync(id);
        await EnsureAuthorizedAsync(user, Operations.Update);
        if (addUserRoleByIdRequest.Role != UserRole.AdminEnum)
            throw new UnknowRoleExeption(addUserRoleByIdRequest.Role.ToString()); // TODO better exception
        if (!await _userManager.IsInRoleAsync(user, Security.Roles.Admin))
            await _userManager.AddToRoleAsync(user, Security.Roles.Admin);
    }
}
