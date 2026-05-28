using Microsoft.AspNetCore.Mvc;
using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class FilmlerController : ControllerBase
{
    private readonly IFilmlerBL _filmlerBL;

    public FilmlerController(IFilmlerBL filmlerBL)
    {
        _filmlerBL = filmlerBL;
    }

    [HttpGet]
    public async Task<ActionResult<List<Film>>> Get(CancellationToken cancellationToken)
    {
        var filmler = await _filmlerBL.FilmListeleAsync(cancellationToken);
        return Ok(filmler);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Film film, CancellationToken cancellationToken)
    {
        try
        {
            await _filmlerBL.FilmEkleAsync(film, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, new { mesaj = "Film basariyla eklendi." });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { mesaj = exception.Message });
        }
    }
}
