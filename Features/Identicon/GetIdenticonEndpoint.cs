using FluentValidation;
using Jdenticon;

namespace TopSwagCode.Api.Features.Identicon;

public class GetIdenticonEndpoint : Endpoint<GetIdenticonEndpointRequest>
{
    
    public override void Configure()
    {
        Get("/identicon");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetIdenticonEndpointRequest req, CancellationToken ct)
    {
        var icon = Jdenticon.Identicon.FromValue(req.Name, req.Size);
        using MemoryStream stream = new();
        await icon.SaveAsPngAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);
        
        await SendStreamAsync(stream, contentType: "image/png", cancellation: ct);
    }
}

public class Validator : Validator<GetIdenticonEndpointRequest>
{
    public Validator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required!")
            .MinimumLength(3)
            .WithMessage("Name is too short!");

        RuleFor(x => x.Size)
            .NotEmpty() // There is nice default descriptions aswell.
            .GreaterThan(50);
    }
}

public record GetIdenticonEndpointRequest(string Name, int Size);