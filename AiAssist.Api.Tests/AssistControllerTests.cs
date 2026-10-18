using AiAssist.Api.Controllers;
using AiAssist.Api.Models;
using AiAssist.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiAssist.Api.Tests;

public sealed class AssistControllerTests
{
    [Fact]
    public async Task PostAsync_WithValidPrompt_ReturnsOk()
    {
        var controller = new AssistController(new AssistService(new FakeAiClient()));
        var request = new AssistRequest { Prompt = "help me design retries" };

        var result = await controller.PostAsync(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<AssistResponse>(ok.Value);
        Assert.Equal("mocked-answer", payload.Response);
        Assert.Equal("fake", payload.Provider);
    }

    [Fact]
    public async Task PostAsync_WithEmptyPrompt_ReturnsBadRequest()
    {
        var controller = new AssistController(new AssistService(new FakeAiClient()));
        var request = new AssistRequest { Prompt = "   " };

        var result = await controller.PostAsync(request, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    private sealed class FakeAiClient : IAiClient
    {
        public string ProviderName => "fake";

        public Task<string> CompletePromptAsync(string prompt, CancellationToken cancellationToken) =>
            Task.FromResult("mocked-answer");
    }
}
