using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Security;

namespace Wemcy.RecipeApp.Backend.Services;

public class ProfileService(UserManager<AppUser> userManager, ImageService imageService, UserService userService)
{
    // TODO Make these atomic transactions to prevent partial updates
    public async Task<AppUser> GetProfileById(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        await userService.EnsureAuthorizedAsync(user, Operations.Read);
        return user;

    }

    public async Task<Stream> GetProfileImageById(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        var image = user.Image ?? throw new ImageNotFoundException("User does not have profile picture");
        return imageService.GetImageById(image.Id);
    }
    public async Task UpdateProfileByIdAsync(Guid id, UserProfileUpdateRequest request)
    {
        var user = await GetProfileById(id);
        await userService.EnsureAuthorizedAsync(user, Operations.Update);
        if (request.HasImageUpdate)
        {
            var image = await imageService.CreateImage(request.ImageStream, request.ImageName);
            user.Image = image;
        }
        if (request.HasDisplayNameUpdate)
        {
            user.DisplayName = request.DisplayName;
        }
        if (request.HasPasswordUpdate)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, token, request.Password);
        }
        // TODO email
        await userManager.UpdateAsync(user);

    }

    public async Task DeleteProfileByIdAsync(Guid id)
    {
        var user = await this.GetProfileById(id);
        await userService.EnsureAuthorizedAsync(user, Operations.Delete);
        await userManager.UpdateSecurityStampAsync(user);
        await userManager.DeleteAsync(user);
    }

    internal async Task AddRoleToUserAsync(Guid id, AddRoleToProfileByIdRequest addRoleToProfileByIdRequest)
    {
        var user = await this.GetProfileById(id);
        await userService.EnsureAuthorizedAsync(user, Operations.Update);
        if (addRoleToProfileByIdRequest.Role != RolesEnum.AdminEnum)
            throw new Exception(addRoleToProfileByIdRequest.Role.ToString()); // TODO better exception
        if (!await userManager.IsInRoleAsync(user, Security.Roles.Admin))
            await userManager.AddToRoleAsync(user, Security.Roles.Admin);
    }
}
