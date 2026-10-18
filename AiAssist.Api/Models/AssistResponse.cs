namespace AiAssist.Api.Models;

public sealed class AssistResponse
{
    public required string Response { get; init; }

    public required string Provider { get; init; }
}
