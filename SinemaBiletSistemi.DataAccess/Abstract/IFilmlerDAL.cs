using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Abstract;

public interface IFilmlerDAL
{
    Task<List<Film>> FilmListeleAsync(CancellationToken cancellationToken = default);
    Task FilmEkleAsync(Film film, CancellationToken cancellationToken = default);
}
