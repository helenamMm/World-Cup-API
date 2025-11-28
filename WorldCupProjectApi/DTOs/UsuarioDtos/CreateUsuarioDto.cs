using System.ComponentModel.DataAnnotations;
namespace WorldCupProjectApi.DTOs;
public class CreateUsuarioDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")] 
    public string Nombre { get; set; }
    
    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
    public string Apellido { get; set; }
    
    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateTime FechaNacimiento { get; set; }
    
    [Required(ErrorMessage = "El correo es requerido")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido")] 
    public string Correo { get; set; }
    
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Contra { get; set; }
    private string _rol = "user";

    public string Rol
    {
        get => _rol;
        set => _rol = string.IsNullOrWhiteSpace(value) ? "user" : value;
    } 
}