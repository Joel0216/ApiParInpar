using Microsoft.AspNetCore.Mvc;
using TextAnalyzerAPI.Domain.Models;
using TextAnalyzerAPI.Services.Features.Palindromes;
using Swashbuckle.AspNetCore.Annotations;

namespace TextAnalyzerAPI.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class PalindromeController : ControllerBase
{
    private readonly PalindromeService _palindromeService;

    public PalindromeController(PalindromeService palindromeService)
    {
        _palindromeService = palindromeService;
    }

    /// <summary>
    /// Verifica si un texto es palíndromo
    /// </summary>
    /// <param name="request">Texto a verificar (solo letras)</param>
    /// <returns>Resultado de la verificación</returns>
    [HttpPost("check")]
    [SwaggerOperation(Summary = "Verificar palíndromo", 
                     Description = "Verifica si el texto ingresado es un palíndromo. Solo acepta letras y espacios.")]
    [SwaggerResponse(200, "Verificación exitosa", typeof(PalindromeResponse))]
    [SwaggerResponse(400, "Datos inválidos")]
    public IActionResult CheckPalindrome([FromBody] PalindromeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _palindromeService.CheckPalindrome(request.Text);
        return Ok(result);
    }

    /// <summary>
    /// Verifica palíndromo mediante parámetro en la URL
    /// </summary>
    /// <param name="text">Texto a verificar</param>
    /// <returns>Resultado de la verificación</returns>
    [HttpGet("check/{text}")]
    [SwaggerOperation(Summary = "Verificar palíndromo (GET)", 
                     Description = "Verifica si el texto en la URL es un palíndromo.")]
    public IActionResult CheckPalindromeGet([FromRoute] string text)
    {
        // Validar que solo contenga letras
        if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
        {
            return BadRequest(new { Message = "Solo se permiten letras y espacios, no números" });
        }

        var result = _palindromeService.CheckPalindrome(text);
        return Ok(result);
    }
}