namespace WorldCupProjectApi.DTOs;

public class PartidoFavoritoDto
{
    public string Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Estadio { get; set; }
    public string Fase { get; set; }
    public string Grupo { get; set; }
    
    public EquipoFavoritoDto EquipoA { get; set; }
    
    public EquipoFavoritoDto EquipoB { get; set; }
    public bool EsFavorito { get; set; } = true; 
    public string Estado { get; set; } // "Programado", "EnJuego", "Finalizado"
}