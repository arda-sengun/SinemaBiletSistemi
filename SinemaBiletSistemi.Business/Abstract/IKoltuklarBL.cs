using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Abstract;

public interface IKoltuklarBL
{
    Task<List<Koltuk>> KoltukListeleAsync(CancellationToken cancellationToken = default);
    Task KoltukEkleAsync(Koltuk koltuk, CancellationToken cancellationToken = default);
}
