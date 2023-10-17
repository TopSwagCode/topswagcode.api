using FluentValidation;
using OpenTelemetry.Trace;

namespace TopSwagCode.Api.Features.File;

public class UploadEndpoint : Endpoint<UploadEndpointRequest>
{
    private readonly Tracer _tracer;

    public UploadEndpoint(Tracer tracer)
    {
        _tracer = tracer;
    }
    public override void Configure()
    {
        Post("/upload/image");
        AllowAnonymous();
        AllowFileUploads();
    }

    public override async Task HandleAsync(UploadEndpointRequest req, CancellationToken ct)
    {
        using var span = _tracer.StartActiveSpan("Upload-Span");
        if (Files.Count > 0)
        {
            var file = Files[0];
            var length = file.Length;

            span.AddEvent($"Received file: {file.FileName}");
            span.SetAttribute("filename", file.FileName);
            span.SetAttribute("contenttype", file.ContentType);
            
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

public class UploadEndpointValidator : Validator<UploadEndpointRequest>
{
    public UploadEndpointValidator()
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