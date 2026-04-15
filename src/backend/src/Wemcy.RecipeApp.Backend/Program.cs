using Microsoft.AspNetCore.HttpOverrides;
using System.Reflection;
using Wemcy.RecipeApp.Backend.Configuration;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Extensions;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Security;
using Wemcy.RecipeApp.Backend.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AdminSettings>(builder.Configuration.GetSection(AdminSettings.SectionName));
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection(CorsSettings.SectionName));
builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection(PaginationSettings.SectionName));
builder.Services.Configure<ImageSizeSettings>(builder.Configuration.GetSection(ImageSizeSettings.SectionName));

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.AddPaginationFilter();
    options.Filters.Add<InvalidCredentialsHandler>();
    options.Filters.Add<EntityNotFoundHandler>();
    options.Filters.Add<RegistrationErrorHandler>();
    options.Filters.Add<UnknownImageFormatHandler>();
    options.Filters.Add<UnauthorizedHandler>();

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

builder.Services.AddScoped<IRecipeService, RecipeService>()
                .AddScoped<IRecipeRepository, RecipeRepository>()
                .AddScoped<IImageRepository, ImageRepository>()
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IImageStorageService, ImageStorageService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IShowcaseRecipeService, ShowcaseRecipeService>()
                .AddScoped<IRecipeShowcaseRepository, RecipeShowcaseRepository>()
                .AddSingleton<IAuthorizationHandler, RecipeAuthorizationHandler>()
                .AddSingleton<IAuthorizationHandler, CommentAuthorizationCrudHandler>()
                .AddSingleton<IAuthorizationHandler, UserAuthorizationHandler>();

builder.Services.AddHostedService<ShowcaseRefreshService>();
builder.Services.ConfigurePagination();

var corsSettings = builder.Configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>() ?? new CorsSettings();
builder.Services.AddHttpContextAccessor().AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsSettings.AllowedOrigins)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

await app.MigrateDatabase();
await app.EnsureDefaultAdminAsync();
await app.EnsureDefaultShowcasesCreated();

// Configure the HTTP request pipeline.

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
    KnownNetworks = { },
    KnownProxies = { }
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

    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
