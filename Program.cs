using FastEndpoints.Security;
using FastEndpoints.Swagger;
using TopSwagCode.Api;
// TODO: Support Versioning
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var bld = WebApplication.CreateBuilder();
bld.Services.AddFastEndpoints()
    .AddCookieAuth(validFor: TimeSpan.FromMinutes(10))
    .AddAuthorization()
    .SwaggerDocument(o =>
    {
        // Generate Api Clients
        // https://fast-endpoints.com/docs/swagger-support#save-to-disk-with-app-run
        o.DocumentSettings = s =>
        {
            s.Title = "My API";
            s.Version = "v1";
        };
        o.EnableJWTBearerAuth = false;
    });
bld.Services.RegisterServicesFromTopSwagCodeApi();

bld.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});

var app = bld.Build();
app.UseFileServer();
app.UseCors(MyAllowSpecificOrigins);
app.UseFastEndpoints()
    .UseAuthorization()
    .UseSwaggerGen();
app.Run();