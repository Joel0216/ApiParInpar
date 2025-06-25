using System.ComponentModel.DataAnnotations;

namespace TextAnalyzerAPI.Domain.Models;

public class PalindromeRequest
{
    [Required(ErrorMessage = "El texto es requerido")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", 
        ErrorMessage = "Solo se permiten letras y espacios, no números")]
    [MinLength(1, ErrorMessage = "El texto debe tener al menos 1 carácter")]
    public required string Text { get; set; }
}

public class PalindromeResponse
{
    public string OriginalText { get; set; } = string.Empty;
    public string CleanedText { get; set; } = string.Empty;
    public bool IsPalindrome { get; set; }
    public string Message { get; set; } = string.Empty;
}