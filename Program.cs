using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;
using TopSwagCode.Api;
using TopSwagCode.Api.Features.Weather;

const string myAllowSpecificOrigins = nameof(myAllowSpecificOrigins);

var bld = WebApplication.CreateBuilder();
bld.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});
bld.Services.AddFastEndpoints()
    .AddCookieAuth(validFor: TimeSpan.FromMinutes(10))
    .AddJWTBearerAuth("sHG3x5uO2gkx6AkLT5AVSA==\nB071B7C79B8EDA0931E8090B4F901548")
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


bld.Services.AddSignalR();
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
app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c =>
    {
        c.Endpoints.RoutePrefix = "api";
        c.Versioning.Prefix = "v";
        //c.Versioning.DefaultVersion = 1;
    })
    .UseSwaggerGen();
app.MapHub<WeatherHub>("/weatherHub");
app.Run();