using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Services
{
    public class ProfileService(UserManager<AppUser> userManager)
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

        public async Task UpdatePasswordAsync(Guid id, string currentPassword, string newPassword)
        {
            var user = await GetProfileById(id);
            await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
    }
}
