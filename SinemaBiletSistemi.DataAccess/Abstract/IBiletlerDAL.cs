using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Abstract;

public interface IBiletlerDAL
{
    Task<List<Bilet>> BiletListeleAsync(CancellationToken cancellationToken = default);
    Task BiletEkleAsync(Bilet bilet, CancellationToken cancellationToken = default);
    Task BiletGuncelleAsync(Bilet bilet, CancellationToken cancellationToken = default);
    Task BiletSilAsync(int biletID, CancellationToken cancellationToken = default);
}
