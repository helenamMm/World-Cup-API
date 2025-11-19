namespace WorldCupProjectApi.DTOs.PartidoDtos;

public class UpdateMarcadorDto
{
    public int GolesEquipoA { get; set; } = 0;
    public int GolesEquipoB { get; set; } = 0;
    
    public string Jugador { get; set; }
}