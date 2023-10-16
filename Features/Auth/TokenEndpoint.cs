using FastEndpoints.Security;
using FluentValidation;

namespace TopSwagCode.Api.Features.Auth;

public class TokenEndpoint : Endpoint<TokenEndpointRequest, TokenEndpointResponse>
{
    public override void Configure()
    {
        Post("/auth/token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TokenEndpointRequest req, CancellationToken ct)
    {
        // Check API key in database or whatever logic you got.
        var jwtToken = JWTBearer.CreateToken(
            signingKey: "sHG3x5uO2gkx6AkLT5AVSA==\nB071B7C79B8EDA0931E8090B4F901548",
            expireAt: DateTime.UtcNow.AddDays(1),
            privileges: u =>
            {
                u.Roles.Add("Manager");
                u.Permissions.AddRange(new[] { "ManageUsers", "ManageInventory" });
                u.Claims.Add(new("UserName", req.ApiKey));
                u["UserID"] = "001";
            });

        await SendAsync(new TokenEndpointResponse(jwtToken), cancellation: ct);
    }
}
public record TokenEndpointRequest(string ApiKey);
public record TokenEndpointResponse(string Token);


public class TokenEndpointValidator : Validator<TokenEndpointRequest>
{
    public TokenEndpointValidator()
    {
        RuleFor(x => x.ApiKey)
            .NotEmpty();
    }
}
