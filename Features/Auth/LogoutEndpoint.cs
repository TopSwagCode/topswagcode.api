using FastEndpoints.Security;

namespace TopSwagCode.Api.Features.Auth;

public class LogoutEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/auth/logout");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var user = HttpContext.User;
        await CookieAuth.SignOutAsync();
    }
}