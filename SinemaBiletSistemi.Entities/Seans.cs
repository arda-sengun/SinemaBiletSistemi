namespace SinemaBiletSistemi.Entities;

public class Seans
{
    public int SeansID { get; set; }
    public int FilmID { get; set; }
    public int SalonID { get; set; }
    public DateTime TarihSaat { get; set; }
    public decimal BiletFiyati { get; set; }
    public int BosKoltukSayisi { get; set; }
}
