namespace WorldCupProjectApi.DTOs.PartidoDtos;

public class PartidoDto
{
    public string Id { get; set; }
    public EquipoDto EquipoA { get; set; }
    public EquipoDto EquipoB { get; set; }
    public int GolesEquipoA { get; set; }
    public int GolesEquipoB { get; set; }
    public DateTime Fecha { get; set; }
    public string Estadio { get; set; }
    public string Ciudad { get; set; }
    public string Estado { get; set; }
    public string Fase { get; set; }
    public string Grupo { get; set; }
    public string ArbitroPrincipal { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
}