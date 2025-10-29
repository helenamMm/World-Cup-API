using System.ComponentModel.DataAnnotations;
namespace WorldCupProjectApi.DTOs;

public class JugadorDto
{
    public string? Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Nombre { get; set; }
    
    [Required(ErrorMessage = "El apellido es requerido")]
    public string Apellido { get; set; }
    
    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateTime FechaNacimiento { get; set; }
    
    [Range(1, 99, ErrorMessage = "El número de camiseta debe estar entre 1 y 99")]
    public int NumeroCamiseta { get; set; }
    
    [Required(ErrorMessage = "La posición es requerida")]
    public string Posicion { get; set; }
}