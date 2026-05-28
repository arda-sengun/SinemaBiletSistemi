using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Concrete;

public sealed class SeanslarBL : ISeanslarBL
{
    private readonly ISeanslarDAL _seanslarDAL;

    public SeanslarBL(ISeanslarDAL seanslarDAL)
    {
        _seanslarDAL = seanslarDAL;
    }

    public Task<List<Seans>> SeansListeleAsync(CancellationToken cancellationToken = default)
    {
        return _seanslarDAL.SeansListeleAsync(cancellationToken);
    }

    public Task SeansEkleAsync(Seans seans, CancellationToken cancellationToken = default)
    {
        SeansEkleDogrula(seans);
        return _seanslarDAL.SeansEkleAsync(TemizKayitOlustur(seans), cancellationToken);
    }

    public Task SeansGuncelleAsync(Seans seans, CancellationToken cancellationToken = default)
    {
        SeansDogrula(seans);
        IdDogrula(seans.SeansID);
        return _seanslarDAL.SeansGuncelleAsync(TemizKayitOlustur(seans), cancellationToken);
    }

    public Task SeansSilAsync(int seansID, CancellationToken cancellationToken = default)
    {
        IdDogrula(seansID);
        return _seanslarDAL.SeansSilAsync(seansID, cancellationToken);
    }

    private static Seans TemizKayitOlustur(Seans seans)
    {
        return new Seans
        {
            SeansID = seans.SeansID,
            FilmID = seans.FilmID,
            SalonID = seans.SalonID,
            TarihSaat = seans.TarihSaat,
            BiletFiyati = seans.BiletFiyati,
            BosKoltukSayisi = seans.BosKoltukSayisi
        };
    }

    private static void SeansEkleDogrula(Seans seans)
    {
        if (seans is null)
        {
            throw new ArgumentNullException(nameof(seans), "Seans bilgileri bos olamaz.");
        }

        if (seans.FilmID <= 0)
        {
            throw new ArgumentException("FilmID 0'dan buyuk olmalidir.", nameof(seans));
        }

        if (seans.SalonID <= 0)
        {
            throw new ArgumentException("SalonID 0'dan buyuk olmalidir.", nameof(seans));
        }

        if (seans.TarihSaat == default)
        {
            throw new ArgumentException("Seans tarih saati bos olamaz.", nameof(seans));
        }

        if (seans.BiletFiyati <= 0)
        {
            throw new ArgumentException("Bilet fiyati 0'dan buyuk olmalidir.", nameof(seans));
        }
    }

    private static void SeansDogrula(Seans seans)
    {
        SeansEkleDogrula(seans);

        if (seans.BosKoltukSayisi < 0)
        {
            throw new ArgumentException("Bos koltuk sayisi negatif olamaz.", nameof(seans));
        }
    }

    private static void IdDogrula(int seansID)
    {
        if (seansID <= 0)
        {
            throw new ArgumentException("SeansID 0'dan buyuk olmalidir.", nameof(seansID));
        }
    }
}
