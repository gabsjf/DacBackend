using DAC_CSharp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAC_Csharp.Presentation.Controllers;

[ApiController]
[Route("api/escolas")]
public sealed class EscolasController : ControllerBase
{
    private readonly IEscolaQueryService _service;

    public EscolasController(IEscolaQueryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ListarAsync([FromQuery] int? municipioId)
    {
        var resultados = await _service.ListarAsync(municipioId);
        return Ok(resultados);
    }
}
