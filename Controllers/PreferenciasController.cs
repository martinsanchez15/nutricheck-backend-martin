using Microsoft.AspNetCore.Mvc;
using NutriCheck.Data;
using NutriCheck.Models;
using NutriCheck.DTOs;
using Microsoft.EntityFrameworkCore;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreferenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PreferenciasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<PreferenciaDTO>> CrearPreferencia([FromBody] Preferencia preferencia)
        {
            if (string.IsNullOrWhiteSpace(preferencia.Tipo))
                return BadRequest("El tipo de preferencia es obligatorio.");

            // ⚠️ Convertir string a int para buscar el paciente
            if (!int.TryParse(preferencia.PacienteId, out int pacienteIdInt))
                return BadRequest("PacienteId no es un entero válido.");

            var paciente = await _context.Pacientes.FindAsync(pacienteIdInt);
            if (paciente == null)
                return NotFound("Paciente no encontrado.");

            _context.Preferencias.Add(preferencia);
            await _context.SaveChangesAsync();

            var dto = new PreferenciaDTO
            {
                Id = preferencia.Id,
                Tipo = preferencia.Tipo,
                PacienteId = paciente.Id.ToString(),
                NombrePaciente = paciente.Nombre ?? ""
            };

            return CreatedAtAction(nameof(CrearPreferencia), new { id = dto.Id }, dto);
        }

        [HttpGet("{pacienteId}")]
        public async Task<ActionResult<IEnumerable<PreferenciaDTO>>> ObtenerPreferencias(string pacienteId)
        {
            if (!int.TryParse(pacienteId, out int pacienteIdInt))
                return BadRequest("PacienteId no es un entero válido.");

            var paciente = await _context.Pacientes.FindAsync(pacienteIdInt);
            if (paciente == null)
                return NotFound("Paciente no encontrado.");

            var preferencias = await _context.Preferencias
                .Where(p => p.PacienteId == pacienteId)
                .ToListAsync();

            var dtos = preferencias.Select(p => new PreferenciaDTO
            {
                Id = p.Id,
                Tipo = p.Tipo,
                PacienteId = p.PacienteId,
                NombrePaciente = paciente.Nombre ?? ""
            });

            return Ok(dtos);
        }
    }
}
