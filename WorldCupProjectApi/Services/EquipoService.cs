using MongoDB.Driver;
using WorldCupProjectApi.Models;
namespace WorldCupProjectApi.Services;

public class EquipoService: BaseService<Equipo>
{
    public EquipoService(MongoDbService dbService) : base(dbService, "equipos")
    {
    }
    
    public async Task<Equipo> GetBySiglasAsync(string siglas)
    {
        return await _collection.Find(e => e.SiglasEquipo == siglas).FirstOrDefaultAsync();
    }

    public async Task<List<Equipo>> GetByGroupAsync(string grupo)
    {
        return await _collection.Find(e => e.Grupo == grupo).ToListAsync();
    }

    public async Task<List<Equipo>> GetByFifaRankingAsync(int minRanking, int maxRanking)
    {
        return await _collection.Find(e => e.RankingFifa >= minRanking && e.RankingFifa <= maxRanking).ToListAsync();
    }

    public async Task<List<Equipo>> GetTopTeamsAsync(int topN)
    {
        return await _collection.Find(_ => true)
            .SortBy(e => e.RankingFifa)
            .Limit(topN)
            .ToListAsync();
    }

    public async Task AddJugadorAsync(string siglasEquipo, Jugador jugador)
    {
        var update = Builders<Equipo>.Update.AddToSet(e => e.Jugadores, jugador);
        await _collection.UpdateOneAsync(e => e.SiglasEquipo == siglasEquipo, update);
    }

    public async Task RemoveJugadorAsync(string siglasEquipo, string jugadorId)
    {
        var update = Builders<Equipo>.Update.PullFilter(e => e.Jugadores, 
            j => j.Id == jugadorId);
        await _collection.UpdateOneAsync(e => e.SiglasEquipo == siglasEquipo, update);
    }

    public async Task<Jugador> GetJugadorByNumberAsync(string equipoId, int numeroCamiseta)
    {
        var equipo = await GetByIdAsync(equipoId);
        return equipo?.Jugadores.FirstOrDefault(j => j.NumeroCamiseta == numeroCamiseta);
    }
}
