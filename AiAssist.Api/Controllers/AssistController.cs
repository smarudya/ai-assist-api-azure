using AiAssist.Api.Models;
using AiAssist.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AiAssist.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AssistController(AssistService assistService) : ControllerBase
{
    [HttpPost]
    [EnableRateLimiting("assist")]
    [ProducesResponseType(typeof(AssistResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<AssistResponse>> PostAsync([FromBody] AssistRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return BadRequest("Prompt must not be empty.");
        }

        var result = await assistService.GetAssistAsync(request.Prompt, cancellationToken);
        return Ok(new AssistResponse
        {
            Response = result.Response,
            Provider = result.Provider
        });
    }
}
