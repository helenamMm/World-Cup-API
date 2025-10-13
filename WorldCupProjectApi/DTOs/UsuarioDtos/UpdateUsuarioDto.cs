namespace WorldCupProjectApi.DTOs;

public class UpdateUsuarioDto
{
    public string Correo { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public bool Activo { get; set; }
    
    public FavoritosDto Favoritos { get; set; }
}