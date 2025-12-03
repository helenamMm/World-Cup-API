using System.ComponentModel.DataAnnotations;
namespace WorldCupProjectApi.DTOs;

public class CreateEquipoDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; }
    
    [Required(ErrorMessage = "El nombre completo del país es requerido")]
    public string NombreCompletoPais { get; set; }
    
    [Required(ErrorMessage = "La bandera es requerida")]
    
    public string Bandera { get; set; }
    public string Informacion { get; set; }
    
    [Required(ErrorMessage = "Las siglas del equipo son requeridas")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Las siglas deben tener exactamente 3 caracteres")]
    public string SiglasEquipo { get; set; }
    public string Grupo { get; set; }
    
    [Range(1, 211, ErrorMessage = "El ranking FIFA debe estar entre 1 y 211")]
    public int RankingFifa { get; set; }
    public List<JugadorDto> Jugadores { get; set; } = new List<JugadorDto>();
}