using MatchApi.Api.Endpoints;
using MatchApi.Api.ExceptionHandling;
using MatchApi.Api.Hubs;
using MatchApi.Api.Realtime;
using MatchApi.Application;
using MatchApi.Application.Common.Interfaces;
using MatchApi.Infrastructure;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog replaces the default logging provider. Console is always on; the Application Insights
// sink is only added when ApplicationInsights:Enabled=true in config, which is how QA/Prod turn it
// on (appsettings.json) while Development stays console-only (appsettings.Development.json).
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();

    var appInsightsEnabled = context.Configuration.GetValue<bool>("ApplicationInsights:Enabled");
    var appInsightsConnectionString = context.Configuration["ApplicationInsights:ConnectionString"];

    if (appInsightsEnabled && !string.IsNullOrWhiteSpace(appInsightsConnectionString))
    {
        var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        telemetryConfiguration.ConnectionString = appInsightsConnectionString;

        loggerConfiguration.WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
    }
});

// Layer registrations (Clean Architecture composition root)
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

    // SignalR powers the real-time commentary feed; the broadcaster adapts the
    // application layer's ICommentaryBroadcaster port onto a SignalR hub context.
    builder.Services.AddSignalR();
builder.Services.AddScoped<ICommentaryBroadcaster, SignalRCommentaryBroadcaster>();
builder.Services.AddScoped<IScoreBroadcaster, SignalRScoreBroadcaster>();

const string ReactClientCorsPolicy = "ReactClient";
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
        ?? new[] { "http://localhost:5173" };

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(ReactClientCorsPolicy, policy =>
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
    });

    builder.Services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],

                ValidAudience = builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
    });

    builder.Services.AddEndpointsApiExplorer();

    // Configure Swagger/OpenAPI

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MatchApi",
            Version = "v1",
            Description = "Clean Architecture minimal API for managing fixtures."
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter a JWT token obtained from /api/admin/login"
        });

        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            { new OpenApiSecuritySchemeReference("Bearer", document, null), new List<string>() }
        });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MatchApi v1");
        options.RoutePrefix = "swagger"; // UI served at /swaggerAdmin
    });
}


app.UseHttpsRedirection();

app.UseCors(ReactClientCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapAdminEndpoints();
app.MapFixtureEndpoints();
app.MapTeamsEndpoints();
app.MapPlayerEndpoints();
app.MapCommentaryEndpoints();
app.MapSportsEndpoints();
app.MapCurrentMatchesEndpoints();
app.MapLiveCommentaryEndpoints();

//app.MapGet("/test-cricbuzz", async () =>
//{
//    using var client = new HttpClient();

//    client.DefaultRequestHeaders.UserAgent.ParseAdd(
//        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/150.0.0.0 Safari/537.36");

//    var url =
//        "https://www.cricbuzz.com/live-cricket-full-commentary/129519/engw-vs-indw-one-off-test-india-women-tour-of-england-2026";

//    var response = await client.GetAsync(url);

//    var html = await response.Content.ReadAsStringAsync();

//    var matches = Regex.Matches(
//        html,
//        @"\\\""commText\\\"":\\\""(.*?)\\\""",
//        RegexOptions.Singleline);

//    var commentary = matches
//        .Select(x => x.Groups[1].Value)
//        .Take(20)
//        .ToList();

//    return Results.Ok(new
//    {
//        StatusCode = (int)response.StatusCode,
//        HtmlLength = html.Length,
//        CommentaryCount = matches.Count,
//        Commentary = commentary
//    });
//});
app.MapHub<CommentaryHub>("/hubs/commentary");

app.MapGet("/", () => Results.Ok(new { service = "MatchApi", status = "running" }))
    .ExcludeFromDescription();

app.MapGet("/_test-exception", IResult () => throw new NullReferenceException("Temporary test of the global exception handler"))
    .ExcludeFromDescription();

app.Run();

// Exposed for WebApplicationFactory-based integration tests
public partial class Program { }

