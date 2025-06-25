using System.ComponentModel.DataAnnotations;

namespace TextAnalyzerAPI.Domain.Models;

public class NumberRequest
{
    [Required(ErrorMessage = "El número es requerido")]
    [RegularExpression(@"^-?\d+$", 
        ErrorMessage = "Solo se permiten números enteros, no letras")]
    public required string Number { get; set; }
}

public class NumberResponse
{
    public int Number { get; set; }
    public bool IsEven { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}