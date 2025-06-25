using TextAnalyzerAPI.Domain.Models;
using System.Text;

namespace TextAnalyzerAPI.Services.Features.Numbers;

public class NumberService
{
    public NumberResponse CheckEvenOdd(string numberString)
    {
        if (!int.TryParse(numberString, out int number))
        {
            return new NumberResponse
            {
                Number = 0,
                IsEven = false,
                Type = "Error",
                Message = "El valor ingresado no es un número válido"
            };
        }

        var isEven = IsEven(number);
        
        return new NumberResponse
        {
            Number = number,
            IsEven = isEven,
            Type = isEven ? "Par" : "Impar",
            Message = $"El número {number} es {(isEven ? "par" : "impar")}"
        };
    }

    private bool IsEven(int number)
    {
        return number % 2 == 0;
    }

    public NumberResponse GetNumberInfo(string numberString)
    {
        var result = CheckEvenOdd(numberString);
        
        if (result.Type != "Error")
        {
            var additionalInfo = new StringBuilder();
            additionalInfo.Append(result.Message);
            
            if (result.Number == 0)
                additionalInfo.Append(" (El cero se considera par)");
            else if (result.Number < 0)
                additionalInfo.Append(" (Número negativo)");
            
            result.Message = additionalInfo.ToString();
        }
        
        return result;
    }
}