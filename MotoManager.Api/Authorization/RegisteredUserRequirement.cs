using Microsoft.AspNetCore.Authorization;

namespace MotoManager.Api.Authorization;

public class RegisteredUserRequirement : IAuthorizationRequirement
{
    // Marker requirement - ne sadrži dodatne parametre
}
