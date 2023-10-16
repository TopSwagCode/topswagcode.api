using FastEndpoints.Security;
using FluentValidation;

namespace TopSwagCode.Api.Features.Auth;

public class LoginEndpoint : Endpoint<LoginEndpointRequest>
{
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginEndpointRequest req, CancellationToken ct)
    {
        await CookieAuth.SignInAsync(u =>
        {
            u.Roles.Add("Admin");
            u.Permissions.AddRange(new[] { "Create_Item", "Delete_Item" });
            u.Claims.Add(new("Address", "123 Street"));

            //indexer based claim setting
            u["Username"] = req.Username;
            u["Department"] = "Administration";
        });
    }
}

public class Validator : Validator<LoginEndpointRequest>
{
    public Validator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Name is required!");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required!");
    }
}

public record LoginEndpointRequest(string Username, string Password);