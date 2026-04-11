using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Extensions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Security;
using Wemcy.RecipeApp.Backend.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.AddPaginationFilter();
}).AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(config => { }, Assembly.GetExecutingAssembly());

var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseContext>(opt =>
{
    opt.UseLazyLoadingProxies();
    opt.UseNpgsql(cs);
});

builder.Services
    .AddAppIdentity()
    .ConfigureCookies()
    .AddCookieAuthorizationPolicy();

builder.Services.AddScoped<RecipeService, RecipeService>().
                 AddScoped<RecipeRepository, RecipeRepository>().
                 AddScoped<ImageRepository, ImageRepository>().
                 AddScoped<ImageService, ImageService>().
                 AddScoped<ImageStorageService, ImageStorageService>().
                 AddScoped<UserService, UserService>().
                 AddScoped<AuthService, AuthService>().
                 AddScoped<ProfileService, ProfileService>().
                 AddScoped<ShowcaseRecipeService, ShowcaseRecipeService>().
                 AddScoped<RecipeShowcaseRepository, RecipeShowcaseRepository>().
                 AddSingleton<IAuthorizationHandler, RecipeAuthorizationCrudHandler>().
                 AddSingleton<IAuthorizationHandler, CommentAuthorizationCrudHandler>().
                 AddSingleton<IAuthorizationHandler, AppUserAuthorizationCrudHandler>();
builder.Services.AddHostedService<ShowcaseRefreshService>();
builder.Services.ConfigurePagination();

builder.Services.AddHttpContextAccessor().AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:9393", "http://localhost:5173", "http://127.0.0.1:9393", "http://127.0.0.1:5173")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

await app.EnsureDefaultAdminAsync();

// Configure the HTTP request pipeline.

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(
        c =>
        {
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                swaggerDoc.Servers = [
                    new() { Url = $"{httpReq.Headers["X-Forwarded-Proto"]}://{httpReq.Headers["X-Forwarded-Host"]}:{httpReq.Headers["X-Forwarded-Port"]}/{httpReq.Headers["X-Forwarded-Prefix"]}/{httpReq.PathBase.Value}", Description = "This is the path comes from the proxy" }
                ];
            });

        });
    app.UseForwardedHeaders();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseForwardedHeaders();
}

//app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
