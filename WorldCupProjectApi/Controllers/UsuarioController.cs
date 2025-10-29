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
        private async Task<ActionResult> ValidateUsuarioAsync(string usuarioId)
        {
            if (string.IsNullOrEmpty(usuarioId))
                return BadRequest(new { message = "Usuario es requerido" });
            
            bool usuarioExiste = await _usuarioService.ExistsAsync(usuarioId);
            if (!usuarioExiste)
                return NotFound(new { message = "Usuario no encontrado" });
            
            return null; // no hubo errores
        }
        
        [HttpGet] 
        public async Task<ActionResult<List<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            var dtos = usuarios.Select(u => MapToDto(u)).ToList();
            return Ok(dtos);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _usuarioService.ValidateCredentialsAsync(loginDto.Email, loginDto.Password);
    
            if (usuario == null)
                return Unauthorized(new { message = "Credenciales inválidas" });
    
            return Ok(MapToDto(usuario));
        }
        
        [HttpGet("{id}")] 
        public async Task<ActionResult<UsuarioDto>> GetUsuario(string id)
        {
            var validation = await ValidateUsuarioAsync(id);
            if (validation != null) return validation;
            
            var usuario = await _usuarioService.GetByIdAsync(id);
            return Ok(MapToDto(usuario));
        }
        
        [HttpPost] 
        public async Task<ActionResult<UsuarioDto>> CreateUsuario([FromBody] CreateUsuarioDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            bool emailExists = await _usuarioService.EmailExistsAsync(createDto.Correo);
            if (emailExists)
                return Conflict(new { message = "El correo ya está registrado" });
            
            var usuario = new Usuario
            {
                Nombre = createDto.Nombre,
                Apellido = createDto.Apellido,
                FechaNacimiento = createDto.FechaNacimiento,
                Correo = createDto.Correo,
                Contra = createDto.Contra,
                Rol = createDto.Rol
            };

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
        
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(string id)
        {
            var validation = await ValidateUsuarioAsync(id);
            if (validation != null) return validation;
            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }
        
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