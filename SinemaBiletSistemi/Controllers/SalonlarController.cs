using Microsoft.AspNetCore.Mvc;
using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SalonlarController : ControllerBase
{
    private readonly ISalonlarBL _salonlarBL;

    public SalonlarController(ISalonlarBL salonlarBL)
    {
        _salonlarBL = salonlarBL;
    }

    [HttpGet]
    public async Task<ActionResult<List<Salon>>> Get(CancellationToken cancellationToken)
    {
        var salonlar = await _salonlarBL.SalonListeleAsync(cancellationToken);
        return Ok(salonlar);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Salon salon, CancellationToken cancellationToken)
    {
        try
        {
            await _salonlarBL.SalonEkleAsync(salon, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, new { mesaj = "Salon basariyla eklendi." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }
}
