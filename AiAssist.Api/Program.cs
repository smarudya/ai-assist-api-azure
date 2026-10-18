using System.Threading.RateLimiting;
using AiAssist.Api.Configuration;
using AiAssist.Api.Services;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AzureOpenAiOptions>(builder.Configuration.GetSection(AzureOpenAiOptions.SectionName));
builder.Services.AddHttpClient<AzureOpenAiClient>();
builder.Services.AddScoped<AssistService>();
builder.Services.AddScoped<IAiClient>(sp =>
{
    var options = sp.GetRequiredService<IConfiguration>().GetSection(AzureOpenAiOptions.SectionName).Get<AzureOpenAiOptions>();
    var hasAzureConfig =
        !string.IsNullOrWhiteSpace(options?.Endpoint) &&
        !string.IsNullOrWhiteSpace(options?.ApiKey) &&
        !string.IsNullOrWhiteSpace(options?.DeploymentName);

    return hasAzureConfig ? sp.GetRequiredService<AzureOpenAiClient>() : new MockAiClient();
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("assist", limiterOptions =>
    {
        limiterOptions.PermitLimit = 20;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 5;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddHealthChecks();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRateLimiter();
app.MapControllers();
app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

app.Run();
