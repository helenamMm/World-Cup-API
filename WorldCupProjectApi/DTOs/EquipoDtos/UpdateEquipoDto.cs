using System.ComponentModel.DataAnnotations;
namespace WorldCupProjectApi.DTOs;

public class UpdateEquipoDto
{
    private string _nombre;
    private string _nombreCompletoPais;
    private string _bandera;
    private string _informacion;
    private string _grupo;
    
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string? Nombre { get; set; }
    
    public string? NombreCompletoPais { get; set; }
    
    
    public string? Bandera { get; set; }
    public string? Informacion { get; set; }
    
    public string? Grupo { get; set; }
    
    [Range(1, 211, ErrorMessage = "El ranking FIFA debe estar entre 1 y 211")]
    public int? RankingFifa { get; set; }
    //public List<UpdateJugadorDto>? Jugadores { get; set; }
}