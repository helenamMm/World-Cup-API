namespace WorldCupProjectApi.DTOs;

public class EquipoFavoritoDto
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    
    public string SiglasEquipo { get; set; }
    public string? Bandera { get; set; }
    public string Grupo { get; set; }
    public bool EsFavorito { get; set; } = true; // Siempre es true porque se agrego a favorito
}