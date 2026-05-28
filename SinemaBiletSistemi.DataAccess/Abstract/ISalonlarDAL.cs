using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Abstract;

public interface ISalonlarDAL
{
    Task<List<Salon>> SalonListeleAsync(CancellationToken cancellationToken = default);
    Task SalonEkleAsync(Salon salon, CancellationToken cancellationToken = default);
}
