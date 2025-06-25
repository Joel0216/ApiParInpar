using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TextAnalyzerAPI.Domain.Models;
using TextAnalyzerAPI.Services.Features.Palindromes;
using Swashbuckle.AspNetCore.Annotations;

namespace TextAnalyzerAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // ✅ Protegido con JWT
    public class PalindromeController : ControllerBase
    {
        private readonly PalindromeService _palindromeService;

        public PalindromeController(PalindromeService palindromeService)
        {
            _palindromeService = palindromeService;
        }

        /// <summary>
        /// Verifica si un texto es palíndromo (POST)
        /// </summary>
        /// <param name="request">Texto a verificar</param>
        /// <returns>Resultado de la verificación</returns>
        [HttpPost("check")]
        [SwaggerOperation(Summary = "Verificar si un texto es palíndromo (POST)",
                          Description = "Envía un texto en el cuerpo para verificar si es palíndromo.")]
        [SwaggerResponse(200, "Verificación exitosa", typeof(PalindromeResponse))]
        [SwaggerResponse(400, "Texto inválido")]
        public IActionResult CheckPalindrome([FromBody] PalindromeRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest(new { Message = "El texto es obligatorio." });
            }

            var result = _palindromeService.CheckPalindrome(request.Text);
            return Ok(result);
        }

        /// <summary>
        /// Verifica si un texto es palíndromo (GET)
        /// </summary>
        /// <param name="text">Texto a verificar en la URL</param>
        /// <returns>Resultado de la verificación</returns>
        [HttpGet("check/{text}")]
        [SwaggerOperation(Summary = "Verificar si un texto es palíndromo (GET)",
                          Description = "Verifica directamente desde la URL si el texto es palíndromo.")]
        [SwaggerResponse(200, "Verificación exitosa", typeof(PalindromeResponse))]
        [SwaggerResponse(400, "Texto inválido")]
        public IActionResult CheckPalindromeGet([FromRoute] string text)
        {
            // Validación: solo letras y espacios
            if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                return BadRequest(new { Message = "Solo se permiten letras y espacios. No se aceptan números ni símbolos." });
            }

            var result = _palindromeService.CheckPalindrome(text);
            return Ok(result);
        }
    }
}