using DAC_CSharp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DAC_Csharp.Presentation.Controllers;

[ApiController]
[Route("api/municipios")]
public sealed class MunicipiosController : ControllerBase
{
    private readonly IMunicipioQueryService _service;

    public MunicipiosController(IMunicipioQueryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ListarAsync()
    {
        var resultados = await _service.ListarAsync();
        return Ok(resultados);
    }
}
