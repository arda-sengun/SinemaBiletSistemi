using Microsoft.AspNetCore.Mvc;
using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BiletlerController : ControllerBase
{
    private readonly IBiletlerBL _biletlerBL;

    public BiletlerController(IBiletlerBL biletlerBL)
    {
        _biletlerBL = biletlerBL;
    }

    [HttpGet]
    public async Task<ActionResult<List<Bilet>>> Get(CancellationToken cancellationToken)
    {
        var biletler = await _biletlerBL.BiletListeleAsync(cancellationToken);
        return Ok(biletler);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Bilet bilet, CancellationToken cancellationToken)
    {
        try
        {
            await _biletlerBL.BiletEkleAsync(bilet, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, new { mesaj = "Bilet basariyla eklendi." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Bilet bilet, CancellationToken cancellationToken)
    {
        try
        {
            if (bilet is null)
            {
                return BadRequest(new { mesaj = "Bilet bilgileri bos olamaz." });
            }

            bilet.BiletID = id;
            await _biletlerBL.BiletGuncelleAsync(bilet, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _biletlerBL.BiletSilAsync(id, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }
}
