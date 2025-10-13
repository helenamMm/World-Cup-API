namespace WorldCupProjectApi.DTOs.PartidoDtos;

public class UpdatePartidoDto
{
    public int GolesEquipoA { get; set; }
    public int GolesEquipoB { get; set; }
    public string Estado { get; set; }
    public string ArbitroPrincipal { get; set; }
}