using IAM.Domain.Models;
using IAM.Domain.Repositories;
using IAM.Domain.Services;
using IAM.Infrastructure.Authentication;
using IAM.Infrastructure.Persistence;
using IAM.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Base de Datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Inyección de Dependencias
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// 3. Endpoints (Minimal API)
var authGroup = app.MapGroup("/api/auth");

authGroup.MapPost("/register", async (AuthRequest request, AuthService authService) =>
{
    try
    {
        var user = await authService.RegisterAsync(request);
        return Results.Created($"/api/users/{user.Id}", new { user.Id, user.Email });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

authGroup.MapPost("/login", async (AuthRequest request, AuthService authService) =>
{
    try
    {
        var response = await authService.LoginAsync(request);
        return Results.Ok(response);
    }
    catch (UnauthorizedAccessException)
    {
        return Results.Unauthorized();
    }
});

app.Run();