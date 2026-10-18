namespace AiAssist.Api.Services;

public interface IAiClient
{
    string ProviderName { get; }

    Task<string> CompletePromptAsync(string prompt, CancellationToken cancellationToken);
}
