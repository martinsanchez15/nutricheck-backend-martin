using Microsoft.AspNetCore.Mvc;
using NutriCheck.Data;
using NutriCheck.Models;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly MongoDbService _db;
        public PacientesController(MongoDbService db) => _db = db;

        [HttpGet]
        public ActionResult<List<Paciente>> GetAll()
            => Ok(_db.Pacientes.Find(_ => true).ToList());

        [HttpGet("{id:length(24)}")]
        public ActionResult<Paciente> Get(string id)
        {
            var p = _db.Pacientes.Find(x => x.Id == id).FirstOrDefault();
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public ActionResult<Paciente> Create(Paciente paciente)
        {
            if (string.IsNullOrWhiteSpace(paciente.Nombre)) 
                return BadRequest("Nombre requerido");
            _db.Pacientes.InsertOne(paciente);
            return CreatedAtAction(nameof(Get), new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Paciente pacienteIn)
        {
            var exists = _db.Pacientes.Find(x => x.Id == id).Any();
            if (!exists) return NotFound();
            pacienteIn.Id = id;
            _db.Pacientes.ReplaceOne(x => x.Id == id, pacienteIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var exists = _db.Pacientes.Find(x => x.Id == id).Any();
            if (!exists) return NotFound();
            _db.Pacientes.DeleteOne(x => x.Id == id);
            return NoContent();
        }
    }
}
