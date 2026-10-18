namespace AiAssist.Api.Configuration;

public sealed class AzureOpenAiOptions
{
    public const string SectionName = "AzureOpenAI";

    public string Endpoint { get; init; } = string.Empty;

    public string ApiKey { get; init; } = string.Empty;

    public string DeploymentName { get; init; } = string.Empty;

    public string ApiVersion { get; init; } = "2024-02-15-preview";
}
