using Microsoft.AspNetCore.Mvc;
using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MusterilerController : ControllerBase
{
    private readonly IMusterilerBL _musterilerBL;

    public MusterilerController(IMusterilerBL musterilerBL)
    {
        _musterilerBL = musterilerBL;
    }

    [HttpGet]
    public async Task<ActionResult<List<Musteri>>> Get(CancellationToken cancellationToken)
    {
        var musteriler = await _musterilerBL.MusteriListeleAsync(cancellationToken);
        return Ok(musteriler);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Musteri musteri, CancellationToken cancellationToken)
    {
        try
        {
            await _musterilerBL.MusteriEkleAsync(musteri, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, new { mesaj = "Musteri basariyla eklendi." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Musteri musteri, CancellationToken cancellationToken)
    {
        try
        {
            if (musteri is null)
            {
                return BadRequest(new { mesaj = "Musteri bilgileri bos olamaz." });
            }

            musteri.MusteriID = id;
            await _musterilerBL.MusteriGuncelleAsync(musteri, cancellationToken);
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
            await _musterilerBL.MusteriSilAsync(id, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }
}
