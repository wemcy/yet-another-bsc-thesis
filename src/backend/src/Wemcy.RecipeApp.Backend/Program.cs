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

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedPrefix;
});
builder.Services.AddScoped<RecipeService, RecipeService>().
                 AddScoped<RecipeRepository, RecipeRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}
// app.UsePathBase("/api");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
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
