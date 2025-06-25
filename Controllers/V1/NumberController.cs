using Microsoft.AspNetCore.Mvc;
using TextAnalyzerAPI.Domain.Models;
using TextAnalyzerAPI.Services.Features.Numbers;
using Swashbuckle.AspNetCore.Annotations;

namespace TextAnalyzerAPI.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class NumberController : ControllerBase
{
    private readonly NumberService _numberService;

    public NumberController(NumberService numberService)
    {
        _numberService = numberService;
    }

    /// <summary>
    /// Verifica si un número es par o impar
    /// </summary>
    /// <param name="request">Número a verificar (solo dígitos)</param>
    /// <returns>Información sobre el número</returns>
    [HttpPost("check")]
    [SwaggerOperation(Summary = "Verificar par/impar", 
                     Description = "Verifica si el número es par o impar. Solo acepta números enteros.")]
    [SwaggerResponse(200, "Verificación exitosa", typeof(NumberResponse))]
    [SwaggerResponse(400, "Datos inválidos")]
    public IActionResult CheckNumber([FromBody] NumberRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _numberService.GetNumberInfo(request.Number);
        
        if (result.Type == "Error")
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Verifica número mediante parámetro en la URL
    /// </summary>
    /// <param name="number">Número a verificar</param>
    /// <returns>Información sobre el número</returns>
    [HttpGet("check/{number}")]
    [SwaggerOperation(Summary = "Verificar par/impar (GET)", 
                     Description = "Verifica si el número en la URL es par o impar.")]
    public IActionResult CheckNumberGet([FromRoute] string number)
    {
        // Validar que solo contenga números
        if (!System.Text.RegularExpressions.Regex.IsMatch(number, @"^-?\d+$"))
        {
            return BadRequest(new { Message = "Solo se permiten números enteros, no letras" });
        }

        var result = _numberService.GetNumberInfo(number);
        return Ok(result);
    }
}