using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Concrete;

public sealed class MusterilerBL : IMusterilerBL
{
    private readonly IMusterilerDAL _musterilerDAL;

    public MusterilerBL(IMusterilerDAL musterilerDAL)
    {
        _musterilerDAL = musterilerDAL;
    }

    public Task<List<Musteri>> MusteriListeleAsync(CancellationToken cancellationToken = default)
    {
        return _musterilerDAL.MusteriListeleAsync(cancellationToken);
    }

    public Task MusteriEkleAsync(Musteri musteri, CancellationToken cancellationToken = default)
    {
        MusteriDogrula(musteri);
        return _musterilerDAL.MusteriEkleAsync(TemizKayitOlustur(musteri), cancellationToken);
    }

    public Task MusteriGuncelleAsync(Musteri musteri, CancellationToken cancellationToken = default)
    {
        MusteriDogrula(musteri);
        IdDogrula(musteri.MusteriID);
        return _musterilerDAL.MusteriGuncelleAsync(TemizKayitOlustur(musteri), cancellationToken);
    }

    public Task MusteriSilAsync(int musteriID, CancellationToken cancellationToken = default)
    {
        IdDogrula(musteriID);
        return _musterilerDAL.MusteriSilAsync(musteriID, cancellationToken);
    }

    private static Musteri TemizKayitOlustur(Musteri musteri)
    {
        return new Musteri
        {
            MusteriID = musteri.MusteriID,
            AdSoyad = musteri.AdSoyad.Trim(),
            Telefon = musteri.Telefon.Trim(),
            Email = musteri.Email.Trim()
        };
    }

    private static void MusteriDogrula(Musteri musteri)
    {
        if (musteri is null)
        {
            throw new ArgumentNullException(nameof(musteri), "Musteri bilgileri bos olamaz.");
        }

        if (string.IsNullOrWhiteSpace(musteri.AdSoyad))
        {
            throw new ArgumentException("Ad soyad bos olamaz.", nameof(musteri));
        }

        if (string.IsNullOrWhiteSpace(musteri.Telefon))
        {
            throw new ArgumentException("Telefon bos olamaz.", nameof(musteri));
        }

        if (string.IsNullOrWhiteSpace(musteri.Email))
        {
            throw new ArgumentException("Email bos olamaz.", nameof(musteri));
        }
    }

    private static void IdDogrula(int musteriID)
    {
        if (musteriID <= 0)
        {
            throw new ArgumentException("MusteriID 0'dan buyuk olmalidir.", nameof(musteriID));
        }
    }
}
