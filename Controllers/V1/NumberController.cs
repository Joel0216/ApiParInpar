using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TextAnalyzerAPI.Domain.Models;
using TextAnalyzerAPI.Services.Features.Numbers;
using Swashbuckle.AspNetCore.Annotations;

namespace TextAnalyzerAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // ✅ Protegido con JWT
    public class NumberController : ControllerBase
    {
        private readonly NumberService _numberService;

        public NumberController(NumberService numberService)
        {
            _numberService = numberService;
        }

        /// <summary>
        /// Verifica si un número es par o impar (POST)
        /// </summary>
        /// <param name="request">Número a verificar</param>
        /// <returns>Resultado de la verificación</returns>
        [HttpPost("check")]
        [SwaggerOperation(Summary = "Verifica si el número es par o impar (POST)",
                          Description = "Envía un número en el cuerpo de la solicitud para verificar si es par o impar.")]
        [SwaggerResponse(200, "Verificación exitosa", typeof(NumberResponse))]
        [SwaggerResponse(400, "Entrada inválida")]
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
        /// Verifica si un número es par o impar (GET)
        /// </summary>
        /// <param name="number">Número como parámetro en la URL</param>
        /// <returns>Resultado de la verificación</returns>
        [HttpGet("check/{number}")]
        [SwaggerOperation(Summary = "Verifica si el número es par o impar (GET)",
                          Description = "Pasa un número en la URL para verificar si es par o impar.")]
        [SwaggerResponse(200, "Verificación exitosa", typeof(NumberResponse))]
        [SwaggerResponse(400, "Entrada inválida")]
        public IActionResult CheckNumberGet([FromRoute] string number)
        {
            // Validar que sea número entero (positivo o negativo)
            if (!System.Text.RegularExpressions.Regex.IsMatch(number, @"^-?\d+$"))
            {
                return BadRequest(new { Message = "Solo se permiten números enteros. No se aceptan letras ni decimales." });
            }

            var result = _numberService.GetNumberInfo(number);
            return Ok(result);
        }
    }
}