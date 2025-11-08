using MongoDB.Driver;
using WorldCupProjectApi.Models;

namespace WorldCupProjectApi.Services;

public class PartidoService :BaseService<Partido>
{
 private readonly EquipoService _equipoService;

        public PartidoService(MongoDbService dbService, EquipoService equipoService) 
            : base(dbService, "partidos")
        {
            _equipoService = equipoService;
        }

    
        public async Task<List<Partido>> GetByEstadoAsync(string estado)
        {
            return await _collection.Find(p => p.Estado == estado).ToListAsync();
        }

        public async Task<List<Partido>> GetByFaseAsync(string fase)
        {
            return await _collection.Find(p => p.Fase == fase).ToListAsync();
        }

        public async Task<List<Partido>> GetByGrupoAsync(string grupo)
        {
            return await _collection.Find(p => p.Grupo == grupo).ToListAsync();
        }

        public async Task<List<Partido>> GetByEquipoAsync(string equipoId)
        {
            return await _collection.Find(p => p.EquipoAId == equipoId || p.EquipoBId == equipoId).ToListAsync();
        }

        public async Task<List<Partido>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _collection.Find(p => p.Fecha >= startDate && p.Fecha <= endDate).ToListAsync();
        }

        public async Task<List<Partido>> GetProximosPartidosAsync(int days = 7)
        {
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(days);
            return await _collection.Find(p => p.Fecha >= startDate && p.Fecha <= endDate && p.Estado == "PROGRAMADO")
                .SortBy(p => p.Fecha)
                .ToListAsync();
        }

        public async Task<List<Partido>> GetPartidosEnVivoAsync()
        {
            return await _collection.Find(p => p.Estado == "EN_JUEGO")
                .SortBy(p => p.Fecha)
                .ToListAsync();
        }

    
        public async Task<bool> ActualizarMarcadorAsync(string partidoId, int golesA, int golesB)
        {
            var partido = await GetByIdAsync(partidoId);
            if (partido == null) return false;

            partido.GolesEquipoA = partido.GolesEquipoA + golesA;
            partido.GolesEquipoB = partido.GolesEquipoB + golesB;
            partido.FechaActualizacion = DateTime.UtcNow;

            await UpdateAsync(partidoId, partido);
            return true;
        }

    
        public async Task<bool> CambiarEstadoAsync(string partidoId, string nuevoEstado)
        {
            var partido = await GetByIdAsync(partidoId);
            if (partido == null) return false;

            partido.Estado = nuevoEstado;
            partido.FechaActualizacion = DateTime.UtcNow;

            await UpdateAsync(partidoId, partido);
            return true;
        }
        
        public async Task<bool> IniciarPartidoAsync(string partidoId)
        {
            return await CambiarEstadoAsync(partidoId, "EN_JUEGO");
        }

    
        public async Task<bool> FinalizarPartidoAsync(string partidoId)
        {
            return await CambiarEstadoAsync(partidoId, "FINALIZADO");
        }

    
        public async Task<Partido> GetWithTeamsAsync(string partidoId)
        {
            var partido = await GetByIdAsync(partidoId);
            if (partido != null)
            {
                partido.EquipoA = await _equipoService.GetByIdAsync(partido.EquipoAId);
                partido.EquipoB = await _equipoService.GetByIdAsync(partido.EquipoBId);
            }
            return partido;
        }

        
        public async Task<List<Partido>> GetAllWithTeamsAsync()
        {
            var partidos = await GetAllAsync();
            foreach (var partido in partidos)
            {
                partido.EquipoA = await _equipoService.GetByIdAsync(partido.EquipoAId);
                partido.EquipoB = await _equipoService.GetByIdAsync(partido.EquipoBId);
            }
            return partidos;
        }   
}