using AiAssist.Api.Services;

namespace AiAssist.Api.Tests;

public sealed class AssistServiceTests
{
    [Fact]
    public async Task GetAssistAsync_ReturnsClientResponseAndProvider()
    {
        var client = new FakeAiClient("fake-response", "fake");
        var service = new AssistService(client);

        var result = await service.GetAssistAsync(" test prompt ", CancellationToken.None);

        Assert.Equal("fake-response", result.Response);
        Assert.Equal("fake", result.Provider);
        Assert.Equal("test prompt", client.ReceivedPrompt);
    }

    private sealed class FakeAiClient(string response, string provider) : IAiClient
    {
        public string ProviderName => provider;

        public string ReceivedPrompt { get; private set; } = string.Empty;

        public Task<string> CompletePromptAsync(string prompt, CancellationToken cancellationToken)
        {
            ReceivedPrompt = prompt;
            return Task.FromResult(response);
        }
    }
}
