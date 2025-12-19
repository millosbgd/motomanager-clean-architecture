using MotoManager.Application.Korisnici;
using System.Security.Claims;

namespace MotoManager.Api.Middleware;

public class KorisnikAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public KorisnikAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, KorisnikService korisnikService)
    {
        // Proveri da li je endpoint zaštićen autentifikacijom
        var endpoint = context.GetEndpoint();
        var requiresAuth = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() != null;

        if (requiresAuth && context.User.Identity?.IsAuthenticated == true)
        {
            // Izvuci Auth0 User ID iz tokena
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? context.User.FindFirst("sub")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // Proveri da li korisnik postoji u bazi
                var korisnikExists = await korisnikService.KorisnikExistsAsync(userId);
                
                if (!korisnikExists)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new 
                    { 
                        message = "Pristup odbijen. Korisnik nije registrovan u sistemu. Kontaktirajte administratora." 
                    });
                    return;
                }
            }
        }

        await _next(context);
    }
}
