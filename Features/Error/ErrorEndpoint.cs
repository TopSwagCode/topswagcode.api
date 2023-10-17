using OpenTelemetry.Trace;

namespace TopSwagCode.Api.Features.Error;

public class ErrorEndpoint : EndpointWithoutRequest
{
    private readonly Tracer _tracer;

    public ErrorEndpoint(Tracer tracer)
    {
        _tracer = tracer;
    }
    
    public override void Configure()
    {
        Post("/error");
        AllowAnonymous();
    }
    
    public override Task HandleAsync(CancellationToken ct)
    {
        using var span = _tracer.StartActiveSpan("Error-Span");

        try
        {
            // ... Some logic went wrong.
            throw new NotImplementedException("you screwed up");
        }
        catch (NotImplementedException e)
        {
            span.RecordException(e);
            throw;
        }
    }
}