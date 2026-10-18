namespace AiAssist.Api.Services;

public sealed class MockAiClient : IAiClient
{
    public string ProviderName => "mock";

    public Task<string> CompletePromptAsync(string prompt, CancellationToken cancellationToken)
    {
        var sanitizedPrompt = prompt.Trim();
        var preview = sanitizedPrompt.Length > 160 ? sanitizedPrompt[..160] + "..." : sanitizedPrompt;
        var result = $"Mock assist suggestion: break '{preview}' into small tasks, validate assumptions, and add telemetry.";
        return Task.FromResult(result);
    }
}
