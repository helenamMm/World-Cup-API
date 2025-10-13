namespace WorldCupProjectApi.DTOs;

public class UpdateEquipoDto
{
    public string? Nombre { get; set; }
    public string? NombreCompletoPais { get; set; }
    public string? Bandera { get; set; }
    public string? Informacion { get; set; }
    
    public string? Grupo { get; set; }
    public int? RankingFifa { get; set; }
    public List<UpdateJugadorDto>? Jugadores { get; set; }
}