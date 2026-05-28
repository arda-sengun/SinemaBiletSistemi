using Microsoft.AspNetCore.Mvc;
using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class KoltuklarController : ControllerBase
{
    private readonly IKoltuklarBL _koltuklarBL;

    public KoltuklarController(IKoltuklarBL koltuklarBL)
    {
        _koltuklarBL = koltuklarBL;
    }

    [HttpGet]
    public async Task<ActionResult<List<Koltuk>>> Get(CancellationToken cancellationToken)
    {
        var koltuklar = await _koltuklarBL.KoltukListeleAsync(cancellationToken);
        return Ok(koltuklar);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Koltuk koltuk, CancellationToken cancellationToken)
    {
        try
        {
            await _koltuklarBL.KoltukEkleAsync(koltuk, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, new { mesaj = "Koltuk basariyla eklendi." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }
}
