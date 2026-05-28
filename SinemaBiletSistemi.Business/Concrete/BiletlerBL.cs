using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Concrete;

public sealed class BiletlerBL : IBiletlerBL
{
    private readonly IBiletlerDAL _biletlerDAL;

    public BiletlerBL(IBiletlerDAL biletlerDAL)
    {
        _biletlerDAL = biletlerDAL;
    }

    public Task<List<Bilet>> BiletListeleAsync(CancellationToken cancellationToken = default)
    {
        return _biletlerDAL.BiletListeleAsync(cancellationToken);
    }

    public Task BiletEkleAsync(Bilet bilet, CancellationToken cancellationToken = default)
    {
        BiletEkleDogrula(bilet);
        return _biletlerDAL.BiletEkleAsync(TemizKayitOlustur(bilet), cancellationToken);
    }

    public Task BiletGuncelleAsync(Bilet bilet, CancellationToken cancellationToken = default)
    {
        BiletDogrula(bilet);
        IdDogrula(bilet.BiletID);
        return _biletlerDAL.BiletGuncelleAsync(TemizKayitOlustur(bilet), cancellationToken);
    }

    public Task BiletSilAsync(int biletID, CancellationToken cancellationToken = default)
    {
        IdDogrula(biletID);
        return _biletlerDAL.BiletSilAsync(biletID, cancellationToken);
    }

    private static Bilet TemizKayitOlustur(Bilet bilet)
    {
        return new Bilet
        {
            BiletID = bilet.BiletID,
            SeansID = bilet.SeansID,
            MusteriID = bilet.MusteriID,
            KoltukID = bilet.KoltukID,
            SatisTarihi = bilet.SatisTarihi,
            OdemeTutari = bilet.OdemeTutari
        };
    }

    private static void BiletEkleDogrula(Bilet bilet)
    {
        if (bilet is null)
        {
            throw new ArgumentNullException(nameof(bilet), "Bilet bilgileri bos olamaz.");
        }

        if (bilet.SeansID <= 0)
        {
            throw new ArgumentException("SeansID 0'dan buyuk olmalidir.", nameof(bilet));
        }

        if (bilet.MusteriID <= 0)
        {
            throw new ArgumentException("MusteriID 0'dan buyuk olmalidir.", nameof(bilet));
        }

        if (bilet.KoltukID <= 0)
        {
            throw new ArgumentException("KoltukID 0'dan buyuk olmalidir.", nameof(bilet));
        }
    }

    private static void BiletDogrula(Bilet bilet)
    {
        BiletEkleDogrula(bilet);

        if (bilet.SatisTarihi == default)
        {
            throw new ArgumentException("Satis tarihi bos olamaz.", nameof(bilet));
        }

        if (bilet.OdemeTutari <= 0)
        {
            throw new ArgumentException("Odeme tutari 0'dan buyuk olmalidir.", nameof(bilet));
        }
    }

    private static void IdDogrula(int biletID)
    {
        if (biletID <= 0)
        {
            throw new ArgumentException("BiletID 0'dan buyuk olmalidir.", nameof(biletID));
        }
    }
}
