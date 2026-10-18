using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AiAssist.Api.Configuration;
using Microsoft.Extensions.Options;

namespace AiAssist.Api.Services;

public sealed class AzureOpenAiClient(HttpClient httpClient, IOptions<AzureOpenAiOptions> options) : IAiClient
{
    public string ProviderName => "azure-openai";

    public async Task<string> CompletePromptAsync(string prompt, CancellationToken cancellationToken)
    {
        var cfg = options.Value;
        if (string.IsNullOrWhiteSpace(cfg.Endpoint) || string.IsNullOrWhiteSpace(cfg.ApiKey) || string.IsNullOrWhiteSpace(cfg.DeploymentName))
        {
            throw new InvalidOperationException("Azure OpenAI configuration is incomplete.");
        }

        var uri = $"{cfg.Endpoint.TrimEnd('/')}/openai/deployments/{cfg.DeploymentName}/chat/completions?api-version={cfg.ApiVersion}";
        using var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.Add("api-key", cfg.ApiKey);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var payload = new
        {
            messages = new[]
            {
                new { role = "system", content = "You are a concise software engineering assistant." },
                new { role = "user", content = prompt }
            },
            temperature = 0.2,
            max_tokens = 250
        };

        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        using var doc = JsonDocument.Parse(json);
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return string.IsNullOrWhiteSpace(content) ? "No response from Azure OpenAI." : content.Trim();
    }
}
