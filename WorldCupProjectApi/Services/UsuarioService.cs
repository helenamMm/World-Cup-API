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
    }
}