using FluentValidation;

namespace TopSwagCode.Api.Features.File;

public class UploadEndpoint : Endpoint<UploadEndpointRequest>
{
    public override void Configure()
    {
        Post("/upload/image");
        AllowAnonymous();
        AllowFileUploads();
    }

    public override async Task HandleAsync(UploadEndpointRequest req, CancellationToken ct)
    {
        if (Files.Count > 0)
        {
            var file = Files[0];
            var length = file.Length;
            
            await SendStreamAsync(
                stream: file.OpenReadStream(),
                fileName: file.FileName,
                fileLengthBytes: file.Length,
                contentType: "image/png");

            return;
        }
        await SendNoContentAsync();
    }
}

public class UploadEndpointRequest
{
    public IFormFile? File { get; set; }
}

public class Validator : Validator<UploadEndpointRequest>
{
    public Validator()
    {
        RuleFor(x => x.File)
            .NotNull().DependentRules(() =>
            {
                RuleFor(x => x.File!.ContentType)
                    .Must(x => x.Equals("image/png", StringComparison.InvariantCultureIgnoreCase))
                    .WithMessage("Image must be of content type PNG");
                RuleFor(x => x.File!.Length)
                    .LessThan(1024 * 512)
                    .WithMessage("Image must be less than 512kb");
            });
    }
}