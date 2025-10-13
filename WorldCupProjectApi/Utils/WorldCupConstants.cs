namespace WorldCupProjectApi.Utils;

public class WorldCupConstants
{
    public static readonly string[] GruposPermitidos = { "A", "B", "C", "D", "E", "F", "G", "H" };
    
    public static readonly string[] FasesPermitidas = {
        "FASE_GRUPOS", "OCTAVOS", "CUARTOS", "SEMIFINAL", "FINAL"
    };
    
    public static readonly string[] EstadosPartidoPermitidos = {
        "PROGRAMADO", "EN_JUEGO", "FINALIZADO", "SUSPENDIDO"
    };
}