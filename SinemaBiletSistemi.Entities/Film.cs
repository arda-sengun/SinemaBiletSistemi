namespace SinemaBiletSistemi.Entities;

public class Film
{
    public int FilmID { get; set; }
    public string FilmAdi { get; set; } = string.Empty;
    public string Yonetmen { get; set; } = string.Empty;
    public int Sure { get; set; }
    public string Tur { get; set; } = string.Empty;
    public string AfisURL { get; set; } = string.Empty;
}
