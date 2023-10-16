using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TopSwagCode.Api.Features.Auth;

public class ClaimsEndpoint : EndpointWithoutRequest<ClaimsEndpointResponse>
{
    public override void Configure()
    {
        Get("/auth/claims");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var claims = User.Claims.Aggregate(
            new Dictionary<string, string>(),
            (dict, claim) =>
            {
                dict[claim.Type] = claim.Value;
                return dict;
            });

        await SendOkAsync(new ClaimsEndpointResponse(claims), ct);
    }
}

public record ClaimsEndpointResponse(Dictionary<string, string> Claims);