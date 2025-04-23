using Microsoft.AspNetCore.Mvc;
using NutriCheck.Data;
using NutriCheck.Models;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreferenciasController : ControllerBase
    {
        private readonly MongoDbService _db;
        public PreferenciasController(MongoDbService db) => _db = db;

        [HttpGet("{pacienteId:length(24)}")]
        public ActionResult<List<Preferencia>> Get(string pacienteId)
        {
            var list = _db.Preferencias
                          .Find(p => p.PacienteId == pacienteId)
                          .ToList();
            return Ok(list);
        }

        [HttpPost]
        public ActionResult<Preferencia> Create(Preferencia pref)
        {
            if (string.IsNullOrWhiteSpace(pref.Tipo))
                return BadRequest("Tipo requerido");
            // validar paciente existe
            var existe = _db.Pacientes.Find(p => p.Id == pref.PacienteId).Any();
            if (!existe) return BadRequest("Paciente no existe");

            _db.Preferencias.InsertOne(pref);
            return CreatedAtAction(nameof(Get), new { pacienteId = pref.PacienteId }, pref);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var exists = _db.Preferencias.Find(p => p.Id == id).Any();
            if (!exists) return NotFound();
            _db.Preferencias.DeleteOne(p => p.Id == id);
            return NoContent();
        }
    }
}
