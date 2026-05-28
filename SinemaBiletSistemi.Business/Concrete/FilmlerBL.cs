using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Concrete;

public sealed class FilmlerBL : IFilmlerBL
{
    private readonly IFilmlerDAL _filmlerDAL;

    public FilmlerBL(IFilmlerDAL filmlerDAL)
    {
        _filmlerDAL = filmlerDAL;
    }

    public Task<List<Film>> FilmListeleAsync(CancellationToken cancellationToken = default)
    {
        return _filmlerDAL.FilmListeleAsync(cancellationToken);
    }

    public Task FilmEkleAsync(Film film, CancellationToken cancellationToken = default)
    {
        FilmDogrula(film);

        var kayit = new Film
        {
            FilmAdi = film.FilmAdi.Trim(),
            Yonetmen = film.Yonetmen.Trim(),
            Sure = film.Sure,
            Tur = film.Tur.Trim(),
            AfisURL = film.AfisURL?.Trim() ?? string.Empty
        };

        return _filmlerDAL.FilmEkleAsync(kayit, cancellationToken);
    }

    private static void FilmDogrula(Film film)
    {
        if (film is null)
        {
            throw new ArgumentNullException(nameof(film), "Film bilgileri bos olamaz.");
        }

        if (string.IsNullOrWhiteSpace(film.FilmAdi))
        {
            throw new ArgumentException("Film adi bos olamaz.", nameof(film));
        }

        if (string.IsNullOrWhiteSpace(film.Yonetmen))
        {
            throw new ArgumentException("Yonetmen bos olamaz.", nameof(film));
        }

        if (film.Sure <= 0)
        {
            throw new ArgumentException("Film suresi 0'dan buyuk olmalidir.", nameof(film));
        }

        if (string.IsNullOrWhiteSpace(film.Tur))
        {
            throw new ArgumentException("Film turu bos olamaz.", nameof(film));
        }
    }
}
