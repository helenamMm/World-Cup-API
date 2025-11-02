using System.Net.Http.Headers;
using System.Security.Claims;
using WorldCupProjectApi.Services;
namespace WorldCupProjectApi.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AuthService _authService;

    public AuthMiddleware(RequestDelegate next, AuthService authService)
    {
        _next = next;
        _authService = authService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.ToString();
        var method = context.Request.Method;
        
        Console.WriteLine($"=== AUTH MIDDLEWARE ===");
        Console.WriteLine($"Path: {path}");
        Console.WriteLine($"Method: {method}");
        Console.WriteLine($"Lower Path: {path.ToLower()}");
    
        bool isPublic = IsPublicRoute(context);
        Console.WriteLine($"Is Public Route: {isPublic}");
        Console.WriteLine($"=== END DEBUG ===");
        
        if (isPublic)
        {
            Console.WriteLine("SKIPPING AUTH - Public route");
            await _next(context);
            return;
        }
        Console.WriteLine("REQUIRING AUTH - Protected route");
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Token de autorización requerido" });
            return;
        }
        
        var token = authHeader.ToString().Replace("Bearer ", "").Trim();
        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Token vacío" });
            return;
        }
        
        var principal = _authService.ValidateToken(token);
        if (principal == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Token inválido o expirado" });
            return;
        }
        
        context.User = principal;
            
        await _next(context);

    }

    private bool IsPublicRoute(HttpContext context)
    {
        var path = context.Request.Path.ToString().ToLower();
        var method = context.Request.Method.ToUpper();
        Console.WriteLine($"Checking if public: {path} [{method}]");

        if (path.StartsWith("/api/usuarios") || path.StartsWith("/api/favoritos"))
        {
            return true;
        }

        if ((path.StartsWith("/api/equipo") || path.Contains("/api/partido")) && method == "GET")
        {
            return true;
        }
        
        return false;
    }
    
}