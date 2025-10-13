using WorldCupProjectApi.Models;
namespace WorldCupProjectApi.DTOs;

public class UsuarioDto
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Correo { get; set; }
    public string Rol { get; set; } = "user";
    public DateTime FechaRegistro { get; set; }
    public bool Activo { get; set; } = true;
    public FavoritosDto Favoritos { get; set; } = new FavoritosDto();
}