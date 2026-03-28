using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Services
{
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
        public async Task UpdateProfileByIdAsync(Guid id, Stream? imageStream, string? name, string? password, string? displayName)
        {
            var user = await GetProfileById(id);
            if (imageStream is not null && name is not null)
            {
                var image = await imageService.CreateImage(imageStream, name);
                user.Image = image;
            }
            if (displayName is not null)
            {
                user.DisplayName = displayName;
            }
            if (password is not null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, token, password);
            }
            // TODO email
            await userManager.UpdateAsync(user);
           
        }
    }
}
