using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Services;

public class ProfileService(UserManager<AppUser> userManager, ImageService imageService)
{
    // TODO Make these atomic transactions to prevent partial updates
    public async Task<AppUser> GetProfileById(Guid id)
    {
        return await userManager.FindByIdAsync(id.ToString())
            ?? throw new UserNotFoundException("User not found.");
    }

    public async Task UpdateDisplayNameAsync(Guid id, string displayName)
    {
        var user = await GetProfileById(id);
        user.DisplayName = displayName;
        await userManager.UpdateAsync(user);
    }

    public async Task UpdateEmailAsync(Guid id, string newEmail)
    {
        var user = await GetProfileById(id);
        await userManager.SetEmailAsync(user, newEmail);
    }


    public async Task<Stream> GetProfileImageById(Guid id)
    {
        var user = await this.GetProfileById(id);
        var image = user.Image ?? throw new ImageNotFoundException("User does not have profile picture");
        return imageService.GetImageById(image.Id);
    }
    public async Task UpdateProfileByIdAsync(Guid id,UserProfileUpdateRequest request)
    {
        var user = await GetProfileById(id);
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
}


public class UserProfileUpdateRequest
{
    public required Stream? ImageStream { get; set; }
    public required string? ImageName { get; set; }
    public required string? DisplayName { get; set; }
    public required string? Password { get; set; }
    //public required string? Email { get; set; }

    [MemberNotNullWhen(true, nameof(ImageStream))]
    [MemberNotNullWhen(true, nameof(ImageName))]
    public bool HasImageUpdate => ImageStream is not null && ImageName is not null;
    [MemberNotNullWhen(true, nameof(DisplayName))]
    public bool HasDisplayNameUpdate => !String.IsNullOrWhiteSpace(DisplayName);
    [MemberNotNullWhen(true, nameof(Password))]
    public bool HasPasswordUpdate => !String.IsNullOrWhiteSpace(Password);

}