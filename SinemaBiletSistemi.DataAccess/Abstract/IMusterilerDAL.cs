using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Abstract;

public interface IMusterilerDAL
{
    Task<List<Musteri>> MusteriListeleAsync(CancellationToken cancellationToken = default);
    Task MusteriEkleAsync(Musteri musteri, CancellationToken cancellationToken = default);
    Task MusteriGuncelleAsync(Musteri musteri, CancellationToken cancellationToken = default);
    Task MusteriSilAsync(int musteriID, CancellationToken cancellationToken = default);
}
