namespace SinemaBiletSistemi.Entities;

public class Bilet
{
    public int BiletID { get; set; }
    public int SeansID { get; set; }
    public int MusteriID { get; set; }
    public int KoltukID { get; set; }
    public DateTime SatisTarihi { get; set; }
    public decimal OdemeTutari { get; set; }
}
