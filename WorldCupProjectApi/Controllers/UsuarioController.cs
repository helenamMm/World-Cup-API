using Microsoft.AspNetCore.Mvc;
using WorldCupProjectApi.DTOs;
using WorldCupProjectApi.Services;
using WorldCupProjectApi.Models;

namespace WorldCupProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            var dtos = usuarios.Select(u => MapToDto(u)).ToList();
            return Ok(dtos);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(string id)
        {
            // Using BaseService method!
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(MapToDto(usuario));
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> CreateUsuario([FromBody] CreateUsuarioDto createDto)
        {
            var usuario = new Usuario
            {
                Nombre = createDto.Nombre,
                Apellido = createDto.Apellido,
                FechaNacimiento = createDto.FechaNacimiento,
                Correo = createDto.Correo,
                Contra = createDto.Contra,
                Rol = createDto.Rol
            };

            // Using BaseService method!
            await _usuarioService.CreateAsync(usuario);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, MapToDto(usuario));
        }
        
        [HttpPut()]
        public async Task<ActionResult> UpdateUsuario([FromBody] UpdateUsuarioDto updateDto)
        {
            var email = updateDto.Correo;
            var userUpdateInfo = await _usuarioService.UpdateByEmailAsync(email, updateDto);
            if (userUpdateInfo == null)
                return NotFound($"Usuario con email {email} no encontrado");
            
            return Ok(MapToDto(userUpdateInfo));
        }
        

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(string id)
        {
            // Using BaseService method!
            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }

        // Helper method to convert to DTO
        private UsuarioDto MapToDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro,
                Activo = usuario.Activo,
                Favoritos = new FavoritosDto
                {
                    Partidos = usuario.Favoritos.Partidos,
                    Equipos = usuario.Favoritos.Equipos
                }
            };
        }
    }
}