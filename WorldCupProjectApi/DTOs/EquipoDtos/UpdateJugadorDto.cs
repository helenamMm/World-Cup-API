namespace WorldCupProjectApi.DTOs;

public class UpdateJugadorDto
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public int? NumeroCamiseta { get; set; }
    public string? Posicion { get; set; }
}