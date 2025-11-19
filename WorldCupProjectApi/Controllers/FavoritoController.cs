using Microsoft.AspNetCore.Mvc;
using WorldCupProjectApi.Services;
using WorldCupProjectApi.Models;
using WorldCupProjectApi.DTOs;

namespace WorldCupProjectApi.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class FavoritosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly EquipoService _equipoService;
        private readonly PartidoService _partidoService;
        
        public FavoritosController(
            UsuarioService usuarioService,
            EquipoService equipoService,
            PartidoService partidoService)
        {
            _usuarioService = usuarioService;
            _equipoService = equipoService;
            _partidoService = partidoService;
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
        
        private async Task<ActionResult> ValidateEquipoAsync(string equipoId)
        {
            if (string.IsNullOrEmpty(equipoId))
                return BadRequest(new { message = "Equipo es requerido" });
            
            var equipo = await _equipoService.GetByIdAsync(equipoId);
            if (equipo == null)
                return NotFound(new { message = "Equipo no encontrado" });
            
            return null; 
        }
    
        private async Task<ActionResult> ValidatePartidoAsync(string partidoId)
        {
            if (string.IsNullOrEmpty(partidoId))
                return BadRequest(new { message = "Partido es requerido" });
            
            var partido = await _partidoService.GetByIdAsync(partidoId);
            if (partido == null)
                return NotFound(new { message = "Partido no encontrado" });
            
            return null; 
        }
    
        private async Task<ActionResult> ValidateUsuarioAndEquipoAsync(string usuarioId, string equipoId)
        {
            var usuarioValidation = await ValidateUsuarioAsync(usuarioId);
            if (usuarioValidation != null) return usuarioValidation;
        
            var equipoValidation = await ValidateEquipoAsync(equipoId);
            if (equipoValidation != null) return equipoValidation;
        
            return null; 
        }
    
        private async Task<ActionResult> ValidateUsuarioAndPartidoAsync(string usuarioId, string partidoId)
        {
            var usuarioValidation = await ValidateUsuarioAsync(usuarioId);
            if (usuarioValidation != null) return usuarioValidation;
        
            var partidoValidation = await ValidatePartidoAsync(partidoId);
            if (partidoValidation != null) return partidoValidation;
        
            return null; 
        }
        
        [HttpGet("equipos/{usuarioId}")]
        public async Task<ActionResult<List<Equipo>>> GetEquiposFavoritos(string usuarioId)
        {
            var validation = await ValidateUsuarioAsync(usuarioId);
            if (validation != null) return validation;
            try
            {
                var equipoIds = await _usuarioService.GetEquiposFavoritosAsync(usuarioId);

                if (equipoIds == null || !equipoIds.Any())
                    return Ok(new {message = "El usuario aun no tiene equipos favoritos"});

                var equipos = await _equipoService.GetAllAsync();
                var equiposFavoritos = equipos.Where(e => equipoIds.Contains(e.Id));
                
                var equiposDto = equiposFavoritos.Select(MapToEquipoFavoritoDto).ToList();
                return Ok(equiposDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"error interno del servidor: {ex.Message}");
            }
        }
        
        [HttpGet("partidos/{usuarioId}")]
        public async Task<ActionResult<List<Partido>>> GetPartidosFavoritos(string usuarioId)
        {
            var validation = await ValidateUsuarioAsync(usuarioId);
            if (validation != null) return validation;
            try
            {
                var partidoIds = await _usuarioService.GetPartidosFavoritosAsync(usuarioId);
                
                if (partidoIds == null || !partidoIds.Any())
                    return Ok(new {message = "El usuario aun no tiene partidos favoritos"});
                
                var partidos = await _partidoService.GetAllWithTeamsAsync();
                var partidosFavoritos = partidos.Where(p => partidoIds.Contains(p.Id)).ToList();
                var partidosDto = partidosFavoritos.Select(MapToPartidoFavoritoDto).ToList();
                return Ok(partidosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"error interno del servidor: {ex.Message}");
            }
        }
        
        [HttpPost("equipos/{equipoId}/{usuarioId}")]
        public async Task<ActionResult> AddEquipoFavorito(string equipoId, string usuarioId)
        {
            var validation = await ValidateUsuarioAndEquipoAsync(usuarioId, equipoId);
            if (validation != null) return validation;
            
            var alreadyFavorite = await _usuarioService.IsEquipoFavoritoAsync(usuarioId, equipoId);
            if (alreadyFavorite)
                return Ok(new { message = "El equipo ya está en favoritos" });

            var success = await _usuarioService.AddEquipoFavoritoAsync(usuarioId, equipoId);
            if (!success)
                return BadRequest("No se pudo agregar el equipo a favoritos");

            return Ok(new { message = "Equipo agregado a favoritos" });    
            
        }
        
        [HttpDelete("equipos/{equipoId}/{usuarioId}")]
        public async Task<ActionResult> RemoveEquipoFavorito(string equipoId, string usuarioId)
        {
            var validation = await ValidateUsuarioAndEquipoAsync(usuarioId, equipoId);
            if (validation != null) return validation;
            
            var isFavorite = await _usuarioService.IsEquipoFavoritoAsync(usuarioId, equipoId);
            if (!isFavorite)
                return Ok(new { message = "El equipo no esta en favoritos" });
            
            var success = await _usuarioService.RemoveEquipoFavoritoAsync(usuarioId, equipoId);
            if (!success)
                return BadRequest(new {message = "No se pudo remover el equipo de favoritos"});

            return Ok(new { message = "Equipo removido de favoritos" });
        }
        
        [HttpPost("partidos/{partidoId}/{usuarioId}")]
        public async Task<ActionResult> AddPartidoFavorito(string partidoId, string usuarioId)
        {
           var validationn = await ValidateUsuarioAndPartidoAsync(usuarioId, partidoId);
           if(validationn != null) return validationn;
           
            var isFavorite = await _usuarioService.IsPartidoFavoritoAsync(usuarioId, partidoId);
            if (isFavorite)
                return Ok(new { message = "El partido ya está en favoritos" });

            var success = await _usuarioService.AddPartidoFavoritoAsync(usuarioId, partidoId);
            if (!success)
                return BadRequest("No se pudo agregar el partido a favoritos");

            return Ok(new { message = "Partido agregado a favoritos" });
        }
        
        [HttpDelete("partidos/{partidoId}/{usuarioId}")]
        public async Task<ActionResult> RemovePartidoFavorito(string partidoId, string usuarioId)
        {
            var validationn = await ValidateUsuarioAndPartidoAsync(usuarioId, partidoId);
            if(validationn != null) return validationn;

            var isFavorite = await _usuarioService.IsPartidoFavoritoAsync(usuarioId, partidoId);
            if (!isFavorite)
                return Ok(new { message = "El partido no está en favoritos" });
            var success = await _usuarioService.RemovePartidoFavoritoAsync(usuarioId, partidoId);
            if (!success)
                return BadRequest("No se pudo remover el partido de favoritos");

            return Ok(new { message = "Partido removido de favoritos" });
        }
        
        [HttpGet("equipos/{equipoId}/is-favorite/{usuarioId}")]
        public async Task<ActionResult<bool>> IsEquipoFavorito(string equipoId, string usuarioId)
        {
            var validationn = await ValidateUsuarioAndEquipoAsync(usuarioId, equipoId);
            if(validationn != null) return validationn;
            
            var isFavorite = await _usuarioService.IsEquipoFavoritoAsync(usuarioId, equipoId);
            return Ok(isFavorite);
        }
        
        [HttpGet("partidos/{partidoId}/is-favorite/{usuarioId}")]
        public async Task<ActionResult<bool>> IsPartidoFavorito(string partidoId, string usuarioId)
        {
            var validationn = await ValidateUsuarioAndPartidoAsync(usuarioId, partidoId);
            if(validationn != null) return validationn;
            
            var isFavorite = await _usuarioService.IsPartidoFavoritoAsync(usuarioId, partidoId);
            return Ok(isFavorite);
        }
        
        private EquipoFavoritoDto MapToEquipoFavoritoDto(Equipo equipo)
        {
            return new EquipoFavoritoDto
            {
                Id = equipo.Id,
                Nombre = equipo.Nombre,
                SiglasEquipo = equipo.SiglasEquipo,
                Bandera = equipo.Bandera,
                Grupo = equipo.Grupo,
                EsFavorito = true
            };
        }

        private PartidoFavoritoDto MapToPartidoFavoritoDto(Partido partido)
        {
            return new PartidoFavoritoDto
            {
                Id = partido.Id,
                Fecha = partido.Fecha,
                Estadio = partido.Estadio,
                Fase = partido.Fase,
                Grupo = partido.Grupo,
                EsFavorito = true,
                Estado = partido.Estado,
                EquipoA = partido.EquipoA != null ? MapToEquipoFavoritoDto(partido.EquipoA) : null,
                EquipoB = partido.EquipoB != null ? MapToEquipoFavoritoDto(partido.EquipoB) : null
            };
        }
    }
    