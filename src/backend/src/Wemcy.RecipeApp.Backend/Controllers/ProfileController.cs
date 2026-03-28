using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Extensions;
using Wemcy.RecipeApp.Backend.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wemcy.RecipeApp.Backend.Controllers;

[InvalidCredentialsHandler]
[UserNotFoundHandler]
public class ProfileController(IAuthService authService, ProfileService profileService, IMapper mapper) : ProfileApiController
{
    public override Task<IActionResult> DeleteProfileById([FromRoute(Name = "id"), Required] Guid id)
    {
        throw new NotImplementedException();
    }

    public override async Task<IActionResult> GetOwnProfile()
    {
        if (User.Identity.TryGetUserId(out var userId))
        {
            return await this.GetProfileById(userId);
        }
        throw new UserNotFoundException();

    }

    public override Task<IActionResult> GetOwnProfileImage()
    {
        throw new NotImplementedException();
    }

    public async override Task<IActionResult> GetProfileById([FromRoute(Name = "id"), Required] Guid id)
    {
        var profile = await profileService.GetProfileById(id);
        return Ok(mapper.Map<ProfileResponse>(profile));
    }

    public override Task<IActionResult> GetProfileImageById([FromRoute(Name = "id"), Required] Guid id)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> UpdateOwnProfile([FromForm(Name = "displayName")] string displayName, [FromForm(Name = "password"), MinLength(6)] string password, IFormFile profileImage)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> UpdateProfileById([FromRoute(Name = "id"), Required] Guid id, [FromForm(Name = "displayName")] string displayName, [FromForm(Name = "password"), MinLength(6)] string password, IFormFile profileImage)
    {
        throw new NotImplementedException();
    }
}
