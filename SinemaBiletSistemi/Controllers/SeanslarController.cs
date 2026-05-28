using Microsoft.AspNetCore.Mvc;
using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SeanslarController : ControllerBase
{
    private readonly ISeanslarBL _seanslarBL;

    public SeanslarController(ISeanslarBL seanslarBL)
    {
        _seanslarBL = seanslarBL;
    }

    [HttpGet]
    public async Task<ActionResult<List<Seans>>> Get(CancellationToken cancellationToken)
    {
        var seanslar = await _seanslarBL.SeansListeleAsync(cancellationToken);
        return Ok(seanslar);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Seans seans, CancellationToken cancellationToken)
    {
        try
        {
            await _seanslarBL.SeansEkleAsync(seans, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, new { mesaj = "Seans basariyla eklendi." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Seans seans, CancellationToken cancellationToken)
    {
        try
        {
            if (seans is null)
            {
                return BadRequest(new { mesaj = "Seans bilgileri bos olamaz." });
            }

            seans.SeansID = id;
            await _seanslarBL.SeansGuncelleAsync(seans, cancellationToken);
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
            await _seanslarBL.SeansSilAsync(id, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }
}
