using System.ComponentModel.DataAnnotations;
namespace WorldCupProjectApi.DTOs;
public class UpdateUsuarioDto
{
    public string Correo { get; set; }
    
    [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")] 
    public string Nombre { get; set; }
    
    [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
    public string Apellido { get; set; }
    
    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateTime? FechaNacimiento { get; set; }
    public bool Activo { get; set; }
    
    public FavoritosDto Favoritos { get; set; }
}