using Microsoft.AspNetCore.Authorization;
using MotoManager.Application.Korisnici;
using System.Security.Claims;

namespace MotoManager.Api.Authorization;

public class RegisteredUserHandler : AuthorizationHandler<RegisteredUserRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public RegisteredUserHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        RegisteredUserRequirement requirement)
    {
        // Izvuci Auth0 User ID iz tokena
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? context.User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            return;
        }

        // Resolve scoped service from service provider
        using var scope = _serviceProvider.CreateScope();
        var korisnikService = scope.ServiceProvider.GetRequiredService<KorisnikService>();

        // Proveri da li korisnik postoji u bazi
        var korisnikExists = await korisnikService.KorisnikExistsAsync(userId);

        if (korisnikExists)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
