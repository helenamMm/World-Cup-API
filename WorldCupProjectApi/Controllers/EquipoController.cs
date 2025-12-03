using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WorldCupProjectApi.DTOs;
using WorldCupProjectApi.Services;
using WorldCupProjectApi.Models;
namespace WorldCupProjectApi.Controllers;
[ApiController]
[Route("api/[controller]")]

public class EquipoController: ControllerBase
{
   private readonly EquipoService _equipoService;

   public EquipoController(EquipoService equipoService)
   {
      _equipoService = equipoService;
   }
   
    [HttpGet]
    public async Task<ActionResult<List<EquipoDto>>> GetEquipos()
    {
        var equipos = await _equipoService.GetAllAsync();
        var dtos = equipos.Select(e => MapToDto(e)).ToList();
        return Ok(dtos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<EquipoDto>> GetEquipo(string id)
    { 
        var equipo = await _equipoService.GetByIdAsync(id);
        if (equipo == null) return NotFound();
        return Ok(MapToDto(equipo));
    }

       
    [HttpGet("siglas/{siglas}")]
    public async Task<ActionResult<EquipoDto>> GetEquipoBySiglas(string siglas)
    {
        var equipo = await _equipoService.GetBySiglasAsync(siglas);
        if (equipo == null) return NotFound();
        return Ok(MapToDto(equipo));
    }

       
    [HttpGet("grupo/{grupo}")]
    public async Task<ActionResult<List<EquipoDto>>> GetEquiposByGrupo(string grupo)
    {
        var equipos = await _equipoService.GetByGroupAsync(grupo);
        var dtos = equipos.Select(e => MapToDto(e)).ToList();
        return Ok(dtos);
    }
    
    [HttpGet("ranking")]
    public async Task<ActionResult<List<EquipoDto>>> GetEquiposByRanking([FromQuery] int min = 1, [FromQuery] int max = 50)
    {
        var equipos = await _equipoService.GetByFifaRankingAsync(min, max);
        var dtos = equipos.Select(e => MapToDto(e)).ToList();
        return Ok(dtos);
    }
    
    [HttpGet("top/{limitN}")]
    public async Task<ActionResult<List<EquipoDto>>> GetTopEquipos(int limitN)
    {
        var equipos = await _equipoService.GetTopTeamsAsync(limitN);
        var dtos = equipos.Select(e => MapToDto(e)).ToList();
        return Ok(dtos);
    }
    
    [HttpPost]
    public async Task<ActionResult<EquipoDto>> CreateEquipo([FromBody] CreateEquipoDto createDto)
    {
        var existingEquipo = await _equipoService.GetBySiglasAsync(createDto.SiglasEquipo);
        if (existingEquipo != null) 
            return BadRequest($"Ya existe un equipo con las siglas {createDto.SiglasEquipo}");
            
        var jugadores = createDto.Jugadores.Select(j => new Jugador
        {
            Nombre = j.Nombre,
            Apellido = j.Apellido,
            FechaNacimiento = j.FechaNacimiento,
            NumeroCamiseta = j.NumeroCamiseta,
            Posicion = j.Posicion
        }).ToList();

            var equipo = new Equipo
            {
                Nombre = createDto.Nombre,
                NombreCompletoPais = createDto.NombreCompletoPais,
                Bandera = createDto.Bandera,
                Informacion = createDto.Informacion,
                SiglasEquipo = createDto.SiglasEquipo,
                Grupo = createDto.Grupo,
                RankingFifa = createDto.RankingFifa,
                Jugadores = jugadores
            };

            await _equipoService.CreateAsync(equipo);
            return CreatedAtAction(nameof(GetEquipo), new { id = equipo.Id }, MapToDto(equipo));
        }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEquipo(string id, [FromBody] UpdateEquipoDto updateDto)
    {
        var equipo = await _equipoService.GetByIdAsync(id);
        if (equipo == null) return NotFound();
        
        equipo.Nombre = !string.IsNullOrEmpty(updateDto.Nombre) ? updateDto.Nombre : equipo.Nombre;
        equipo.NombreCompletoPais = !string.IsNullOrEmpty(updateDto.NombreCompletoPais) ? updateDto.NombreCompletoPais : equipo.NombreCompletoPais;
        equipo.Bandera = !string.IsNullOrEmpty(updateDto.Bandera) ? updateDto.Bandera : equipo.Bandera;
        equipo.Informacion = !string.IsNullOrEmpty(updateDto.Informacion) ? updateDto.Informacion : equipo.Informacion;
        equipo.Grupo = !string.IsNullOrEmpty(updateDto.Grupo) ? updateDto.Grupo : equipo.Grupo;
        
        equipo.RankingFifa = updateDto.RankingFifa ?? equipo.RankingFifa;

        await _equipoService.UpdateAsync(id, equipo);
    
        return Ok(MapToDto(equipo));
    }

    
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEquipo(string id)
        {
            await _equipoService.DeleteAsync(id);
            return Ok(new{message = "El equipo se ha eliminado correctamente"});
        }

        // JUGADORES ENDPOINTS

    
        [HttpPost("jugador/{siglasEquipo}")]
        public async Task<ActionResult> AddJugador(string siglasEquipo, [FromBody] JugadorDto jugadorDto)
        {
            var jugador = new Jugador
            {
                Nombre = jugadorDto.Nombre,
                Apellido = jugadorDto.Apellido,
                FechaNacimiento = jugadorDto.FechaNacimiento,
                NumeroCamiseta = jugadorDto.NumeroCamiseta,
                Posicion = jugadorDto.Posicion
            };

            await _equipoService.AddJugadorAsync(siglasEquipo, jugador);
            return Ok(new {mesaage  = "El jugador se actualizo correctamente"});
        }

    
        [HttpDelete("{siglasEquipo}/jugadores/{jugadorId}")]
        public async Task<ActionResult> RemoveJugador(string siglasEquipo, string jugadorId)
        {
            await _equipoService.RemoveJugadorAsync(siglasEquipo, jugadorId);
            return Ok(new {message = "El jugador se elimino correctamente"});
        }

        
        [HttpGet("{id}/jugadores")]
        public async Task<ActionResult<List<JugadorDto>>> GetJugadores(string id)
        {
            var equipo = await _equipoService.GetByIdAsync(id);
            if (equipo == null) return NotFound();

            var jugadoresDto = equipo.Jugadores.Select(j => new JugadorDto
            {
                Id = j.Id,
                Nombre = j.Nombre,
                Apellido = j.Apellido,
                FechaNacimiento = j.FechaNacimiento,
                NumeroCamiseta = j.NumeroCamiseta,
                Posicion = j.Posicion
            }).ToList();

            return Ok(jugadoresDto);
        }

    
        [HttpGet("{equipoId}/jugadores/numero/{numeroCamiseta}")]
        public async Task<ActionResult<JugadorDto>> GetJugadorByNumber(string equipoId, int numeroCamiseta)
        {
            var jugador = await _equipoService.GetJugadorByNumberAsync(equipoId, numeroCamiseta);
            if (jugador == null) return NotFound();

            var jugadorDto = new JugadorDto
            {
                Id = jugador.Id,
                Nombre = jugador.Nombre,
                Apellido = jugador.Apellido,
                FechaNacimiento = jugador.FechaNacimiento,
                NumeroCamiseta = jugador.NumeroCamiseta,
                Posicion = jugador.Posicion
            };

            return Ok(jugadorDto);
        }

        
        private EquipoDto MapToDto(Equipo equipo)
        {
            return new EquipoDto
            {
                Id = equipo.Id,
                Nombre = equipo.Nombre,
                NombreCompletoPais = equipo.NombreCompletoPais,
                Bandera = equipo.Bandera,
                Informacion = equipo.Informacion,
                SiglasEquipo = equipo.SiglasEquipo,
                Grupo = equipo.Grupo,
                RankingFifa = equipo.RankingFifa,
                FechaCreacion = equipo.FechaCreacion,
                Jugadores = equipo.Jugadores.Select(j => new JugadorDto
                {
                    Id = j.Id,
                    Nombre = j.Nombre,
                    Apellido = j.Apellido,
                    FechaNacimiento = j.FechaNacimiento,
                    NumeroCamiseta = j.NumeroCamiseta,
                    Posicion = j.Posicion
                }).ToList()
            };
        }
}
