using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Model.Entities;

namespace Wemcy.RecipeApp.Backend.Services;

public interface IUserService
{
    Task AddRoleToUserAsync(Guid id, AddUserRoleByIdRequest addUserRoleByIdRequest);
    Task CreateAdminUserAsync();
    Task CreateUserAsync(string email, string password, string? displayName);
    Task DeleteUserByIdAsync(Guid id);
    Task EnsureCurrentUserCanAsync<T>(OperationAuthorizationRequirement operation, T resource);
    Task<User?> FindUserByEmailAsync(string email);
    Task<User> GetCurrentUserAsync();
    Task<Stream> GetProfileImageByIdAsync(Guid id, ImageSize size);
    Task<User> GetUserByIdAsync(Guid id);
    Task UpdateProfileByIdAsync(Guid id, UserProfileUpdateRequest request);
}
