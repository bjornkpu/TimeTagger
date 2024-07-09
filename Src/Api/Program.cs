using FastEndpoints;
using Api;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<DbCtx>("postgresdb");

builder.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = builder.Configuration["Auth:JwtKey"])
    .AddAuthorization()
    .AddFastEndpoints(o => o.SourceGeneratorDiscoveredTypes.AddRange(DiscoveredTypes.All))
    .SwaggerDocument();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c => c.Errors.UseProblemDetails())
    .UseSwaggerGen();

app.Run();

// For testing purposes
public partial class Program { }
