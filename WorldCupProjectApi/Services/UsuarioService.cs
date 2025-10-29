using System.Runtime.InteropServices.JavaScript;
using MongoDB.Driver;
using WorldCupProjectApi.DTOs;
using WorldCupProjectApi.Models;

namespace WorldCupProjectApi.Services
{
    public class UsuarioService : BaseService<Usuario>
    {
        public UsuarioService(MongoDbService dbService):base(dbService, "usuarios" )
        {
        }
        
        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _collection.Find(u => u.Correo == email).FirstOrDefaultAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _collection.Find(u => u.Correo == email).AnyAsync();
        }

        public async Task<Usuario> UpdateByEmailAsync(string email, UpdateUsuarioDto updateInfo)
        {
            var existingUser = await _collection.Find(u => u.Correo == email).FirstOrDefaultAsync();
            if (existingUser == null) return null;
            
            if (!string.IsNullOrEmpty(updateInfo.Nombre))
                existingUser.Nombre = updateInfo.Nombre;

            if (!string.IsNullOrEmpty(updateInfo.Apellido))
                existingUser.Apellido = updateInfo.Apellido;

            if (updateInfo.FechaNacimiento.HasValue)
                existingUser.FechaNacimiento = updateInfo.FechaNacimiento.Value;

            existingUser.Activo = updateInfo.Activo;
            
            await _collection.ReplaceOneAsync(
                Builders<Usuario>.Filter.Eq(u => u.Correo, email), // Find by email
                existingUser 
            );
            
            return existingUser;

        }
        
        public async Task<bool> AddEquipoFavoritoAsync(string usuarioId, string equipoId)
        {
            var usuario = await GetByIdAsync(usuarioId);
            if (usuario == null) return false;

            if (!usuario.Favoritos.Equipos.Contains(equipoId))
            {
                usuario.Favoritos.Equipos.Add(equipoId);
                await UpdateAsync(usuarioId, usuario);
            }
            return true;
        }   

        public async Task<Usuario> ValidateCredentialsAsync(string email, string password)
        {
            var usuario = await GetByEmailAsync(email);
            
            if (usuario == null || usuario.Contra != password)
                return null;
            
            return usuario;
        }
        public async Task<bool> RemoveEquipoFavoritoAsync(string usuarioId, string equipoId)
        {
            var usuario = await GetByIdAsync(usuarioId);
            if (usuario == null) return false;

            usuario.Favoritos.Equipos.Remove(equipoId);
            await UpdateAsync(usuarioId, usuario);
            return true;
        }

        public async Task<bool> AddPartidoFavoritoAsync(string usuarioId, string partidoId)
        {
            var usuario = await GetByIdAsync(usuarioId);
            if (usuario == null) return false;

            if (!usuario.Favoritos.Partidos.Contains(partidoId))
            {   
                usuario.Favoritos.Partidos.Add(partidoId);
                await UpdateAsync(usuarioId, usuario);
            }
            return true;
        }

        public async Task<bool> RemovePartidoFavoritoAsync(string usuarioId, string partidoId)
        {
            var usuario = await GetByIdAsync(usuarioId);
            if (usuario == null) return false;

            usuario.Favoritos.Partidos.Remove(partidoId);
            await UpdateAsync(usuarioId, usuario);
            return true;
        }

        public async Task<List<string>> GetEquiposFavoritosAsync(string usuarioId) //Mandarlo a la verga
        {
            var usuario = await GetByIdAsync(usuarioId);
            return usuario?.Favoritos.Equipos ?? new List<string>();
        }

        public async Task<List<string>> GetPartidosFavoritosAsync(string usuarioId)
        {
            var usuario = await GetByIdAsync(usuarioId);
            return usuario?.Favoritos.Partidos ?? new List<string>();
        }

        public async Task<bool> IsEquipoFavoritoAsync(string usuarioId, string equipoId)
        {
            var usuario = await GetByIdAsync(usuarioId);
            return usuario?.Favoritos.Equipos.Contains(equipoId) ?? false;
        }

        public async Task<bool> IsPartidoFavoritoAsync(string usuarioId, string partidoId)
        {
            var usuario = await GetByIdAsync(usuarioId);
            return usuario?.Favoritos.Partidos.Contains(partidoId) ?? false;
        }
    }
}