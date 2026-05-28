using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Concrete;

public sealed class KoltuklarBL : IKoltuklarBL
{
    private readonly IKoltuklarDAL _koltuklarDAL;

    public KoltuklarBL(IKoltuklarDAL koltuklarDAL)
    {
        _koltuklarDAL = koltuklarDAL;
    }

    public Task<List<Koltuk>> KoltukListeleAsync(CancellationToken cancellationToken = default)
    {
        return _koltuklarDAL.KoltukListeleAsync(cancellationToken);
    }

    public Task KoltukEkleAsync(Koltuk koltuk, CancellationToken cancellationToken = default)
    {
        KoltukDogrula(koltuk);

        var kayit = new Koltuk
        {
            SalonID = koltuk.SalonID,
            KoltukNo = koltuk.KoltukNo.Trim()
        };

        return _koltuklarDAL.KoltukEkleAsync(kayit, cancellationToken);
    }

    private static void KoltukDogrula(Koltuk koltuk)
    {
        if (koltuk is null)
        {
            throw new ArgumentNullException(nameof(koltuk), "Koltuk bilgileri bos olamaz.");
        }

        if (koltuk.SalonID <= 0)
        {
            throw new ArgumentException("SalonID 0'dan buyuk olmalidir.", nameof(koltuk));
        }

        if (string.IsNullOrWhiteSpace(koltuk.KoltukNo))
        {
            throw new ArgumentException("Koltuk numarasi bos olamaz.", nameof(koltuk));
        }
    }
}
