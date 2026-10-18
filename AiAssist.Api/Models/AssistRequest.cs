using System.ComponentModel.DataAnnotations;

namespace AiAssist.Api.Models;

public sealed class AssistRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(2000)]
    public string Prompt { get; init; } = string.Empty;
}
