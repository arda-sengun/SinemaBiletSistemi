using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Abstract;

public interface ISeanslarBL
{
    Task<List<Seans>> SeansListeleAsync(CancellationToken cancellationToken = default);
    Task SeansEkleAsync(Seans seans, CancellationToken cancellationToken = default);
    Task SeansGuncelleAsync(Seans seans, CancellationToken cancellationToken = default);
    Task SeansSilAsync(int seansID, CancellationToken cancellationToken = default);
}
