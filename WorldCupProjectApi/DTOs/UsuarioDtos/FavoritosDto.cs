namespace WorldCupProjectApi.DTOs;

public class FavoritosDto
{
    public List<string> Partidos { get; set; } = new List<string>();
    public List<string> Equipos { get; set; } = new List<string>();
}