namespace WorldCupProjectApi.DTOs.PartidoDtos;

public class UpdatePartidoDto
{
    public DateTime? Fecha { get; set; }
    public string? Estadio { get; set; }
    public string? Ciudad { get; set; }
    public string? Estado { get; set; } 
    public string? Fase { get; set; }
    public string? Grupo { get; set; }
    public string? ArbitroPrincipal { get; set; }
}