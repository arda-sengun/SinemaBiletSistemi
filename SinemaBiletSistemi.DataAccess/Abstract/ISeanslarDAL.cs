using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Abstract;

public interface ISeanslarDAL
{
    Task<List<Seans>> SeansListeleAsync(CancellationToken cancellationToken = default);
    Task SeansEkleAsync(Seans seans, CancellationToken cancellationToken = default);
    Task SeansGuncelleAsync(Seans seans, CancellationToken cancellationToken = default);
    Task SeansSilAsync(int seansID, CancellationToken cancellationToken = default);
}
