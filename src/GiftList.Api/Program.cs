using GiftList.Infrastructure.Data;
using GiftList.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqlServerDbContext<ApplicationDbContext>("giftlistdb");

builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

const string VitePolicy = "VitePolicy";
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        // Aspire assigns the Vite dev server a different port on every run, so the
        // allowed origin can't be hardcoded; this matches any localhost/127.0.0.1 port.
        // In production the SPA is served from this same host/origin, so no CORS
        // policy is needed there at all.
        options.AddPolicy(VitePolicy, policy =>
            policy.SetIsOriginAllowed(origin =>
                      Uri.TryCreate(origin, UriKind.Absolute, out var uri) &&
                      uri.Host is "localhost" or "127.0.0.1")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials());
    });
}

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseCors(VitePolicy);
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/ping", () => new PingResponse("Hello from the API", DateTimeOffset.UtcNow))
    .WithName("Ping")
    .WithTags("Diagnostics");

app.MapFallbackToFile("index.html");

app.Run();

internal sealed record PingResponse(string Message, DateTimeOffset ServerTimeUtc);
