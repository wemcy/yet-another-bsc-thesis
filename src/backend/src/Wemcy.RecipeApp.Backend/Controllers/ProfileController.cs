using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Extensions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Security;
using Wemcy.RecipeApp.Backend.Services;
using Wemcy.RecipeApp.Backend.Utils;

namespace Wemcy.RecipeApp.Backend.Controllers;

public class ProfileController(IMapper mapper, IUserService userService) : ProfileApiController
{

    [Authorize(Roles = Roles.Admin)]

    public override async Task<IActionResult> AddUserRoleById([FromRoute(Name = "id"), Required] Guid id, [FromBody] AddUserRoleByIdRequest addUserRoleByIdRequest)
    {
        await userService.AddRoleToUserAsync(id, addUserRoleByIdRequest);
        return NoContent();
    }

    public override async Task<IActionResult> DeleteProfileById([FromRoute(Name = "id"), Required] Guid id)
    {
        await userService.DeleteUserByIdAsync(id);
        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    public override async Task<IActionResult> GetAdminViewProfileById([FromRoute(Name = "id"), Required] Guid id)
    {
        var profile = await userService.GetUserByIdAsync(id);
        return Ok(mapper.Map<Api.Models.Profile>(profile));
    }

    public override async Task<IActionResult> GetOwnProfile()
    {
        if (User.Identity.TryGetUserId(out var userId))
        {
            return await this.GetAdminViewProfileById(userId);
        }
        throw new UserNotFoundException();
    }


    public override async Task<IActionResult> GetOwnProfileImage([FromQuery(Name = "imageSize")] ImageSize? imageSize)
    {
        if (User.Identity.TryGetUserId(out var userId))
        {
            return await this.GetProfileImageById(userId, imageSize);
        }
        throw new UserNotFoundException();
    }

    public async override Task<IActionResult> GetProfileById([FromRoute(Name = "id"), Required] Guid id)
    {
        var profile = await userService.GetUserByIdAsync(id);
        return Ok(mapper.Map<Api.Models.ProfileSummary>(profile));
    }

    public override async Task<IActionResult> GetProfileImageById([FromRoute(Name = "id"), Required] Guid id, [FromQuery(Name = "size")] ImageSize? size)
    {
        try
        {
            return new FileStreamResult(await userService.GetProfileImageByIdAsync(id, size ?? ImageSize.LargeEnum), "image/jpeg");
        }
        catch (ImageNotFoundException)
        {
            return File(DefaultImages.DefaultProfileSvg, "image/svg+xml");
        }

    }

    public override async Task<IActionResult> UpdateOwnProfile([FromForm(Name = "displayName")] string? displayName, [FromForm(Name = "password"), MinLength(6)] string? password, IFormFile? profileImage, [FromForm(Name = "email")] string? email)
    {
        if (User.Identity.TryGetUserId(out var userId))
        {
            return await this.UpdateProfileById(userId, displayName, password, profileImage, email);
        }
        throw new UserNotFoundException();
    }

    public override async Task<IActionResult> UpdateProfileById([FromRoute(Name = "id"), Required] Guid id, [FromForm(Name = "displayName")] string? displayName, [FromForm(Name = "password"), MinLength(6)] string? password, IFormFile? profileImage, [FromForm(Name = "email")] string? email)
    {
        await userService.UpdateProfileByIdAsync(id, new UserProfileUpdateRequest()
        {
            DisplayName = displayName,
            Password = password,
            ImageStream = profileImage?.OpenReadStream(),
            ImageName = profileImage?.FileName,
            Email = email
        });
        return NoContent();
    }
}
