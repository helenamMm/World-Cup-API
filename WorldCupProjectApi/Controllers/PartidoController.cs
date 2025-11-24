using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WorldCupProjectApi.DTOs;
using WorldCupProjectApi.DTOs.PartidoDtos;
using WorldCupProjectApi.Services;
using WorldCupProjectApi.Models;
using WorldCupProjectApi.Utils;
namespace WorldCupProjectApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PartidoController : ControllerBase
{
        private readonly PartidoService _partidoService;
        private readonly EquipoService _equipoService;
        private readonly NotificationService _notificationService;

        public PartidoController(PartidoService partidoService, EquipoService equipoService,  NotificationService notificationService)
        {
            _partidoService = partidoService;
            _equipoService = equipoService;
            _notificationService = notificationService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<PartidoDto>>> GetPartidos()
        {
            var partidos = await _partidoService.GetAllWithTeamsAsync();
            var dtos = partidos.Select(p => MapToDto(p)).ToList();
            return Ok(dtos);
        }

    
        [HttpGet("{id}")]
        public async Task<ActionResult<PartidoDto>> GetPartido(string id)
        {
            var partido = await _partidoService.GetWithTeamsAsync(id);
            if (partido == null) return NotFound();
            return Ok(MapToDto(partido));
        }

    
        [HttpGet("estado/{estado}")]
        public async Task<ActionResult<List<PartidoDto>>> GetPartidosByEstado(string estado)
        {
            if (!WorldCupConstants.EstadosPartidoPermitidos.Contains(estado))
                return BadRequest($"Estado inválido. Estados permitidos: {string.Join(", ", WorldCupConstants.EstadosPartidoPermitidos)}");

            var partidos = await _partidoService.GetByEstadoAsync(estado);
            var partidosConEquipos = await PopulateTeams(partidos);
            var dtos = partidosConEquipos.Select(p => MapToDto(p)).ToList();
            return Ok(dtos);
        }
        
        [HttpGet("fase/{fase}")]
        public async Task<ActionResult<List<PartidoDto>>> GetPartidosByFase(string fase)
        {
            if (!WorldCupConstants.FasesPermitidas.Contains(fase))
                return BadRequest($"Fase inválida. Fases permitidas: {string.Join(", ", WorldCupConstants.FasesPermitidas)}");

            var partidos = await _partidoService.GetByFaseAsync(fase);
            var partidosConEquipos = await PopulateTeams(partidos);
            var dtos = partidosConEquipos.Select(p => MapToDto(p)).ToList();
            return Ok(dtos);
        }
        
        [HttpGet("grupo/{grupo}")]
        public async Task<ActionResult<List<PartidoDto>>> GetPartidosByGrupo(string grupo)
        {
            if (!WorldCupConstants.GruposPermitidos.Contains(grupo.ToUpper()))
                return BadRequest($"Grupo inválido. Grupos permitidos: {string.Join(", ", WorldCupConstants.GruposPermitidos)}");

            var partidos = await _partidoService.GetByGrupoAsync(grupo.ToUpper());
            var partidosConEquipos = await PopulateTeams(partidos);
            var dtos = partidosConEquipos.Select(p => MapToDto(p)).ToList();
            return Ok(dtos);
        }

    
        [HttpGet("equipo/{equipoId}")]
        public async Task<ActionResult<List<PartidoDto>>> GetPartidosByEquipo(string equipoId)
        {
            var partidos = await _partidoService.GetByEquipoAsync(equipoId);
            var partidosConEquipos = await PopulateTeams(partidos);
            var dtos = partidosConEquipos.Select(p => MapToDto(p)).ToList();
            return Ok(dtos);
        }

    
        [HttpGet("proximos")]
        public async Task<ActionResult<List<PartidoDto>>> GetProximosPartidos([FromQuery] int dias = 7)
        {
            var partidos = await _partidoService.GetProximosPartidosAsync(dias);
            var partidosConEquipos = await PopulateTeams(partidos);
            var dtos = partidosConEquipos.Select(p => MapToDto(p)).ToList();
            return Ok(dtos);
        }
        
        [HttpGet("en-vivo")]
        public async Task<ActionResult<List<PartidoDto>>> GetPartidosEnVivo()
        {
            var partidos = await _partidoService.GetPartidosEnVivoAsync();
            var partidosConEquipos = await PopulateTeams(partidos);
            var dtos = partidosConEquipos.Select(p => MapToDto(p)).ToList();
            return Ok(dtos);
        }

    
        [HttpPost]
        public async Task<ActionResult<PartidoDto>> CreatePartido([FromBody] CreatePartidoDto createDto)
        {
            if (!WorldCupConstants.EstadosPartidoPermitidos.Contains(createDto.Estado))
                return BadRequest($"Estado inválido. Estados permitidos: {string.Join(", ", WorldCupConstants.EstadosPartidoPermitidos)}");
            
            if (!WorldCupConstants.FasesPermitidas.Contains(createDto.Fase))
                return BadRequest($"Fase inválida. Fases permitidas: {string.Join(", ", WorldCupConstants.FasesPermitidas)}");
            
            if (createDto.Fase == "FASE_GRUPOS")
            {
                if (string.IsNullOrEmpty(createDto.Grupo) || !WorldCupConstants.GruposPermitidos.Contains(createDto.Grupo.ToUpper()))
                    return BadRequest($"Para fase de grupos, debe especificar un grupo válido: {string.Join(", ", WorldCupConstants.GruposPermitidos)}");
            }
            
            var equipoA = await _equipoService.GetByIdAsync(createDto.EquipoAId);
            var equipoB = await _equipoService.GetByIdAsync(createDto.EquipoBId);
            if (equipoA == null || equipoB == null)
                return BadRequest("Uno o ambos equipos no existen");
            
            if (createDto.Fase == "FASE_GRUPOS" && equipoA.Grupo != equipoB.Grupo)
            {
                return BadRequest("Los grupos de los equipos deben coincidir");
            }
            var partido = new Partido
            {
                EquipoAId = createDto.EquipoAId,
                EquipoBId = createDto.EquipoBId,
                Fecha = createDto.Fecha,
                Estadio = createDto.Estadio,
                Ciudad = createDto.Ciudad,
                Estado = createDto.Estado,
                Fase = createDto.Fase,
                Grupo = createDto.Grupo?.ToUpper(),
                ArbitroPrincipal = createDto.ArbitroPrincipal
            };

            await _partidoService.CreateAsync(partido);
            
            partido.EquipoA = equipoA;
            partido.EquipoB = equipoB;

            return CreatedAtAction(nameof(GetPartido), new { id = partido.Id }, MapToDto(partido));
        }

    
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePartido(string id, [FromBody] UpdatePartidoDto updateDto)
        {
            if (updateDto.Estado != null && !WorldCupConstants.EstadosPartidoPermitidos.Contains(updateDto.Estado))
                return BadRequest($"Estado inválido. Estados permitidos: {string.Join(", ", WorldCupConstants.EstadosPartidoPermitidos)}");

            var partido = await _partidoService.GetByIdAsync(id);
            if (partido == null) return NotFound();

            partido.GolesEquipoA = updateDto.GolesEquipoA;
            partido.GolesEquipoB = updateDto.GolesEquipoB;
            partido.Estado = updateDto.Estado ?? partido.Estado;
            partido.ArbitroPrincipal = updateDto.ArbitroPrincipal ?? partido.ArbitroPrincipal;
            partido.FechaActualizacion = DateTime.UtcNow;

            await _partidoService.UpdateAsync(id, partido);
            return CreatedAtAction(nameof(GetPartido), new { id = partido.Id }, MapToDto(partido));
        }

    
        [HttpPatch("{id}/marcador")]
        public async Task<ActionResult> UpdateMarcador(string id, [FromBody] UpdateMarcadorDto updateDto)
        {
            var success = await _partidoService.ActualizarMarcadorAsync(id, updateDto.GolesEquipoA, updateDto.GolesEquipoB);
            var partido = await  _partidoService.GetWithTeamsAsync(id);
            if (!success) return NotFound();
            var equipoAnotador = updateDto.GolesEquipoA > updateDto.GolesEquipoB? partido.EquipoA.Nombre : partido.EquipoB.Nombre;
            
            await _notificationService.SendGoalNotificationAsync( partido, equipoAnotador, updateDto.Jugador);
            return CreatedAtAction(nameof(GetPartido), new { id = partido.Id }, MapToDto(partido));
        }

      
        [HttpPatch("{id}/iniciar")]
        public async Task<ActionResult> IniciarPartido(string id)
        {
            var success = await _partidoService.IniciarPartidoAsync(id);
            var partido = await  _partidoService.GetWithTeamsAsync(id);
            if (!success) return NotFound();

            await _notificationService.SendMatchUpdateNotificationAsync(partido, partido.Estado);
            return CreatedAtAction(nameof(GetPartido), new { id = partido.Id }, MapToDto(partido));
        }

    
        [HttpPatch("{id}/finalizar")]
        public async Task<ActionResult> FinalizarPartido(string id)
        {
            var success = await _partidoService.FinalizarPartidoAsync(id);
            var partido = await  _partidoService.GetByIdAsync(id);
            if (!success) return NotFound();
            return CreatedAtAction(nameof(GetPartido), new { id = partido.Id }, MapToDto(partido));
        }

    
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePartido(string id)
        {
            await _partidoService.DeleteAsync(id);
            return Ok(new { message = "Partido eliminado" });
        }

        
        private PartidoDto MapToDto(Partido partido)
        {
            return new PartidoDto
            {
                Id = partido.Id,
                EquipoA = partido.EquipoA != null ? MapEquipoToDto(partido.EquipoA) : null,
                EquipoB = partido.EquipoB != null ? MapEquipoToDto(partido.EquipoB) : null,
                GolesEquipoA = partido.GolesEquipoA,
                GolesEquipoB = partido.GolesEquipoB,
                Fecha = partido.Fecha,
                Estadio = partido.Estadio,
                Ciudad = partido.Ciudad,
                Estado = partido.Estado,
                Fase = partido.Fase,
                Grupo = partido.Grupo,
                ArbitroPrincipal = partido.ArbitroPrincipal,
                FechaCreacion = partido.FechaCreacion,
                FechaActualizacion = partido.FechaActualizacion
            };
        }

        private EquipoDto MapEquipoToDto(Equipo equipo)
        {
            return new EquipoDto
            {
                Id = equipo.Id,
                Nombre = equipo.Nombre,
                NombreCompletoPais = equipo.NombreCompletoPais,
                Bandera = equipo.Bandera,
                SiglasEquipo = equipo.SiglasEquipo,
                Grupo = equipo.Grupo,
                RankingFifa = equipo.RankingFifa
            };
        }

        private async Task<List<Partido>> PopulateTeams(List<Partido> partidos)
        {
            foreach (var partido in partidos)
            {
                partido.EquipoA = await _equipoService.GetByIdAsync(partido.EquipoAId);
                partido.EquipoB = await _equipoService.GetByIdAsync(partido.EquipoBId);
            }
            return partidos;
        }
}