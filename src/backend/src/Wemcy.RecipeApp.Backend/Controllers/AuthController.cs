using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Wemcy.RecipeApp.Backend.Api.Controllers;

namespace Wemcy.RecipeApp.Backend.Controllers;

public class AuthController(
    UserManager<IdentityUser<Guid>> userManager,
    IConfiguration configuration) : AuthApiController
{
    private readonly UserManager<IdentityUser<Guid>> userManager = userManager;
    private readonly IConfiguration configuration = configuration;

    public async override Task<IActionResult> Register([FromBody] Api.Models.RegisterRequest registerRequest)
    {
        var user = new IdentityUser<Guid>
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
        };

        var result = await userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User registered successfully" });
    }
    public async override Task<IActionResult> Login([FromBody] Api.Models.LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, loginRequest.Password))
            return Unauthorized(new { message = "Invalid email or password" });

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    private string GenerateJwtToken(IdentityUser<Guid> user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
