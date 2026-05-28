using SinemaBiletSistemi.Business.Abstract;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Concrete;

public sealed class SalonlarBL : ISalonlarBL
{
    private readonly ISalonlarDAL _salonlarDAL;

    public SalonlarBL(ISalonlarDAL salonlarDAL)
    {
        _salonlarDAL = salonlarDAL;
    }

    public Task<List<Salon>> SalonListeleAsync(CancellationToken cancellationToken = default)
    {
        return _salonlarDAL.SalonListeleAsync(cancellationToken);
    }

    public Task SalonEkleAsync(Salon salon, CancellationToken cancellationToken = default)
    {
        SalonDogrula(salon);

        var kayit = new Salon
        {
            SalonAdi = salon.SalonAdi.Trim(),
            Kapasite = salon.Kapasite
        };

        return _salonlarDAL.SalonEkleAsync(kayit, cancellationToken);
    }

    private static void SalonDogrula(Salon salon)
    {
        if (salon is null)
        {
            throw new ArgumentNullException(nameof(salon), "Salon bilgileri bos olamaz.");
        }

        if (string.IsNullOrWhiteSpace(salon.SalonAdi))
        {
            throw new ArgumentException("Salon adi bos olamaz.", nameof(salon));
        }

        if (salon.Kapasite <= 0)
        {
            throw new ArgumentException("Salon kapasitesi 0'dan buyuk olmalidir.", nameof(salon));
        }
    }
}
