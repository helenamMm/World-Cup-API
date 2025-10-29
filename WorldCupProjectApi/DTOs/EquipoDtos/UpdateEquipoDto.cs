using System.ComponentModel.DataAnnotations;
namespace WorldCupProjectApi.DTOs;

public class UpdateEquipoDto
{
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string? Nombre { get; set; }
    public string? NombreCompletoPais { get; set; }
    
    [Url(ErrorMessage = "La bandera debe ser una URL válida")]
    public string? Bandera { get; set; }
    public string? Informacion { get; set; }
    
    public string? Grupo { get; set; }
    
    [Range(1, 211, ErrorMessage = "El ranking FIFA debe estar entre 1 y 211")]
    public int? RankingFifa { get; set; }
    public List<UpdateJugadorDto>? Jugadores { get; set; }
}