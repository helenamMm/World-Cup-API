namespace WorldCupProjectApi.DTOs.PartidoDtos;

public class CreatePartidoDto
{
    public string EquipoAId { get; set; }
    public string EquipoBId { get; set; }
    public DateTime Fecha { get; set; }
    public string Estadio { get; set; }
    public string Ciudad { get; set; }
    public string Estado { get; set; } = "PROGRAMADO";
    public string Fase { get; set; }
    public string Grupo { get; set; }
    public string ArbitroPrincipal { get; set; }
}