using TextAnalyzerAPI.Domain.Models;
using System.Text;
using System.Globalization;

namespace TextAnalyzerAPI.Services.Features.Palindromes;

public class PalindromeService
{
    public PalindromeResponse CheckPalindrome(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return new PalindromeResponse
            {
                OriginalText = text ?? "",
                CleanedText = "",
                IsPalindrome = false,
                Message = "El texto no puede estar vacío"
            };
        }

        // Limpiar el texto: quitar espacios, acentos y convertir a minúsculas
        var cleanedText = CleanText(text);
        
        // Verificar si es palíndromo
        var isPalindrome = IsPalindromeCheck(cleanedText);

        return new PalindromeResponse
        {
            OriginalText = text,
            CleanedText = cleanedText,
            IsPalindrome = isPalindrome,
            Message = isPalindrome 
                ? $"¡'{text}' es un palíndromo!" 
                : $"'{text}' no es un palíndromo."
        };
    }

    private string CleanText(string text)
    {
        var sb = new StringBuilder();
        
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                // Quitar acentos y convertir a minúscula
                var normalized = c.ToString().Normalize(NormalizationForm.FormD);
                foreach (char ch in normalized)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(char.ToLower(ch));
                    }
                }
            }
        }
        
        return sb.ToString();
    }

    private bool IsPalindromeCheck(string text)
    {
        if (string.IsNullOrEmpty(text)) return false;
        
        int left = 0;
        int right = text.Length - 1;
        
        while (left < right)
        {
            if (text[left] != text[right])
                return false;
            
            left++;
            right--;
        }
        
        return true;
    }
}