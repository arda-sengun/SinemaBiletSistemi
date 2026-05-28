using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Abstract;

public interface IMusterilerBL
{
    Task<List<Musteri>> MusteriListeleAsync(CancellationToken cancellationToken = default);
    Task MusteriEkleAsync(Musteri musteri, CancellationToken cancellationToken = default);
    Task MusteriGuncelleAsync(Musteri musteri, CancellationToken cancellationToken = default);
    Task MusteriSilAsync(int musteriID, CancellationToken cancellationToken = default);
}
