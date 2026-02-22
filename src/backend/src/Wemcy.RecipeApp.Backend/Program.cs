using System;
using System.Reflection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper( config => { }, Assembly.GetExecutingAssembly() );

var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseNpgsql(cs));
builder.Services.AddScoped<RecipeService, RecipeService>().
                 AddScoped<RecipeRepository, RecipeRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(
        c => {
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = new List<Microsoft.OpenApi.Models.OpenApiServer> {
                new Microsoft.OpenApi.Models.OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host}:{httpReq.Headers["X-Forwarded-Port"]}/api/{httpReq.PathBase.Value}" }
            };
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

app.UseAuthorization();

app.MapControllers();

app.Run();
