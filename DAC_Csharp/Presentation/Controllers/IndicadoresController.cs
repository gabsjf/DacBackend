using DAC_CSharp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAC_Csharp.Presentation.Controllers;

[ApiController]
[Route("api/indicadores")]
public sealed class IndicadoresController : ControllerBase
{
    private readonly IIndicadorEducacionalQueryService _service;

    public IndicadoresController(IIndicadorEducacionalQueryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ListarAsync([FromQuery] int? ano, [FromQuery] int? municipioId, [FromQuery] int? escolaId)
    {
        var resultados = await _service.ListarAsync(ano, municipioId, escolaId);
        return Ok(resultados);
    }
}
