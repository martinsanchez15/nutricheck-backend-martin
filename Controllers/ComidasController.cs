using Microsoft.AspNetCore.Mvc;
using NutriCheck.Data;
using NutriCheck.Models;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComidasController : ControllerBase
    {
        private readonly MongoDbService _db;
        public ComidasController(MongoDbService db) => _db = db;

        [HttpGet]
        public ActionResult<List<Comida>> GetAll() => 
            Ok(_db.Comidas.Find(_ => true).ToList());

        [HttpGet("{id:length(24)}")]
        public ActionResult<Comida> Get(string id)
        {
            var c = _db.Comidas.Find(x => x.Id == id).FirstOrDefault();
            if (c == null) return NotFound();
            return Ok(c);
        }

        [HttpPost]
        public ActionResult<Comida> Create(Comida c)
        {
            if (string.IsNullOrWhiteSpace(c.Nombre)) 
                return BadRequest("Nombre requerido");
            _db.Comidas.InsertOne(c);
            return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Comida c)
        {
            var exists = _db.Comidas.Find(x => x.Id == id).Any();
            if (!exists) return NotFound();
            c.Id = id;
            _db.Comidas.ReplaceOne(x => x.Id == id, c);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var exists = _db.Comidas.Find(x => x.Id == id).Any();
            if (!exists) return NotFound();
            _db.Comidas.DeleteOne(x => x.Id == id);
            return NoContent();
        }
    }
}
