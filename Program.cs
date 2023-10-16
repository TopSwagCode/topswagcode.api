using FastEndpoints.Security;
using FastEndpoints.Swagger;
using TopSwagCode.Api;

const string myAllowSpecificOrigins = nameof(myAllowSpecificOrigins);

var bld = WebApplication.CreateBuilder();
bld.Services.AddFastEndpoints()
    .AddCookieAuth(validFor: TimeSpan.FromMinutes(10))
    .AddAuthorization()
    .SwaggerDocument(o =>
    {
        o.MaxEndpointVersion = 1;
        o.DocumentSettings = s =>
        {
            s.DocumentName = "Release 1.0";
            s.Title = "TopSwagCode.API";
            s.Version = "v1.0";
        };
    })
    .SwaggerDocument(o =>
    {
        o.MaxEndpointVersion = 2;
        o.DocumentSettings = s =>
        {
            s.DocumentName = "Release 2.0";
            s.Title = "TopSwagCode.API";
            s.Version = "v2.0";
        };
    });



bld.Services.RegisterServicesFromTopSwagCodeApi();

bld.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});

var app = bld.Build();
app.UseFileServer();
app.UseCors(myAllowSpecificOrigins);
app.UseFastEndpoints(c =>
    {
        c.Endpoints.RoutePrefix = "api";
        c.Versioning.Prefix = "v";
        //c.Versioning.DefaultVersion = 1;
    })
    .UseAuthorization()
    .UseSwaggerGen();
app.Run();