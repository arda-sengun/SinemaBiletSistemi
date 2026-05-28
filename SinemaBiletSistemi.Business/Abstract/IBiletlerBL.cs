using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Abstract;

public interface IBiletlerBL
{
    Task<List<Bilet>> BiletListeleAsync(CancellationToken cancellationToken = default);
    Task BiletEkleAsync(Bilet bilet, CancellationToken cancellationToken = default);
    Task BiletGuncelleAsync(Bilet bilet, CancellationToken cancellationToken = default);
    Task BiletSilAsync(int biletID, CancellationToken cancellationToken = default);
}
