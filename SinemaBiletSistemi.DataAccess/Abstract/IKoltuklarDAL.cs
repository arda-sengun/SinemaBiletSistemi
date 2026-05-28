using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Abstract;

public interface IKoltuklarDAL
{
    Task<List<Koltuk>> KoltukListeleAsync(CancellationToken cancellationToken = default);
    Task KoltukEkleAsync(Koltuk koltuk, CancellationToken cancellationToken = default);
}
