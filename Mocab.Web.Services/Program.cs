using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mocab.Web.Services.Data;
using Mocab.Web.Services.Data.Entities;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Configuration;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowMyOrigin",
        builder => builder
            .WithOrigins("http://localhost:8100", "https://gplanethn.ddns.net")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddAuthorization();
//builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        // Allow the cookie to be sent in cross-site requests
        options.Cookie.SameSite = SameSiteMode.None;

        //// Disable redirect for API calls: instead of redirecting to login, return 401.
        //options.Events.OnRedirectToLogin = context =>
        //{
        //    // For API endpoints, you may want to just return a 401
        //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //    return Task.CompletedTask;
        //};

        //// For logout, instead of redirecting to the login page, return 200.
        //options.Events.OnRedirectToLogout = context =>
        //{
        //    context.Response.StatusCode = StatusCodes.Status200OK;
        //    return Task.CompletedTask;
        //};
        //options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<DataContext>()
    .AddApiEndpoints();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMyOrigin");
app.UseHttpsRedirection();

app.MapIdentityApi<User>();

//app.UseAuthentication();
app.UseAuthorization();

app.Map("/", () => "Hello World ASP.NET Core 8 Web API, Code Bless You!");

app.MapPost("/logout", async (SignInManager<User> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();

}).RequireAuthorization();

app.MapControllers();

app.Run();
