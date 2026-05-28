using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.Business.Abstract;

public interface ISalonlarBL
{
    Task<List<Salon>> SalonListeleAsync(CancellationToken cancellationToken = default);
    Task SalonEkleAsync(Salon salon, CancellationToken cancellationToken = default);
}
