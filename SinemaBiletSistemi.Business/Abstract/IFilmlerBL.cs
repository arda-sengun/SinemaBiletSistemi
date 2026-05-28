using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Abstract;

public interface IFilmlerBL
{
    Task<List<Film>> FilmListeleAsync(CancellationToken cancellationToken = default);
    Task FilmEkleAsync(Film film, CancellationToken cancellationToken = default);
}
