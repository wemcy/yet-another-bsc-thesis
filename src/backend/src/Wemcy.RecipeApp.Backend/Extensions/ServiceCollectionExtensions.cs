using Microsoft.AspNetCore.Identity;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection ConfigureCookies(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.Path = "/";
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;
            // Return 401/403 JSON instead of redirecting (API behaviour)
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });

        return services;
    }

    public static IServiceCollection AddCookieAuthorizationPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("cookieAuth", policy =>
            {
                policy.AuthenticationSchemes.Add(IdentityConstants.ApplicationScheme);
                policy.RequireAuthenticatedUser();
            });
        });

        return services;
    }
}

