using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Services
{
    public interface IUserService
    {
        Task AddRoleToUserAsync(Guid id, AddUserRoleByIdRequest addUserRoleByIdRequest);
        Task CreateAdminUser();
        Task CreateUserAsync(string email, string password, string? displayName);
        Task DeleteProfileByIdAsync(Guid id);
        Task EnsureAuthorizedAsync<T>(T resource, OperationAuthorizationRequirement operation);
        Task<User?> FindByEmailAsync(string email);
        ClaimsPrincipal GetCurrentUser();
        Task<User> GetCurrentUserEntityAsync();
        Guid GetCurrentUserId();
        Task<Stream> GetProfileImageById(Guid id);
        Task<User> GetUserByIdAsync(Guid id);
        Task UpdateProfileByIdAsync(Guid id, UserProfileUpdateRequest request);
    }
}
