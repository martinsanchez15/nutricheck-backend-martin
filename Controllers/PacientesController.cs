using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutriCheck.Data;
using NutriCheck.Models;
using MongoDB.Bson;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IMongoCollection<Paciente> _pacientes;

        public PacientesController(MongoDbService mongoDbService)
        {
            _pacientes = mongoDbService.Pacientes;
        }

        // Crear paciente
        [HttpPost]
        public async Task<ActionResult<Paciente>> CrearPaciente([FromBody] Paciente paciente)
        {
            if (string.IsNullOrWhiteSpace(paciente.Nombre) || paciente.Edad <= 0)
                return BadRequest("El nombre y la edad del paciente son obligatorios y válidos.");

            await _pacientes.InsertOneAsync(paciente);
            return CreatedAtAction(nameof(CrearPaciente), new { id = paciente.Id }, paciente);
        }

        // Obtener todos los pacientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> ObtenerPacientes()
        {
            var pacientes = await _pacientes.Find(_ => true).ToListAsync();
            return Ok(pacientes);
        }

        // Editar paciente
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarPaciente(string id, [FromBody] Paciente pacienteActualizado)
        {
            var filter = Builders<Paciente>.Filter.Eq(p => p.Id, id);
            var result = await _pacientes.ReplaceOneAsync(filter, pacienteActualizado);

            if (result.MatchedCount == 0)
                return NotFound();

            return Ok(pacienteActualizado);
        }

        // Eliminar paciente
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPaciente(string id)
        {
            var result = await _pacientes.DeleteOneAsync(p => p.Id == id);

            if (result.DeletedCount == 0)
                return NotFound();

            return NoContent();
        }
    }
}
