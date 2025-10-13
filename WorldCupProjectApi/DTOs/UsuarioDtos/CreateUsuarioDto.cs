namespace WorldCupProjectApi.DTOs;

public class CreateUsuarioDto
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Correo { get; set; }
    public string Contra { get; set; }
    public string Rol { get; set; } = "user";
}