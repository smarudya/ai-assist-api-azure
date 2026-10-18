namespace AiAssist.Api.Services;

public sealed class AssistService(IAiClient aiClient)
{
    public async Task<(string Response, string Provider)> GetAssistAsync(string prompt, CancellationToken cancellationToken)
    {
        var cleaned = prompt.Trim();
        var response = await aiClient.CompletePromptAsync(cleaned, cancellationToken);
        return (response, aiClient.ProviderName);
    }
}
