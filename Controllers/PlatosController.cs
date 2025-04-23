using Microsoft.AspNetCore.Mvc;
using NutriCheck.Models;
using NutriCheck.Data;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/nutricionista/comidas")]
    public class PlatosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlatosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crea un nuevo plato de comida y lo asocia a un nutricionista.
        /// </summary>
        /// <param name="plato">Datos del plato: nombre, ingredientes, receta, calorías, proteínas</param>
        /// <returns>El plato creado o un error si los datos son inválidos</returns>
        [HttpPost("crear")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlatoComida>> CrearPlato([FromBody] PlatoComida plato)
        {
            if (string.IsNullOrWhiteSpace(plato.Nombre) || string.IsNullOrWhiteSpace(plato.Ingredientes))
                return BadRequest("El nombre y los ingredientes son obligatorios.");

            if (plato.CaloriasAprox <= 0 || plato.ProteinasAprox < 0)
                return BadRequest("Las calorías deben ser mayores a 0 y las proteínas no negativas.");

            _context.PlatosComida.Add(plato);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CrearPlato), new { id = plato.Id }, plato);
        }

        /// <summary>
        /// Devuelve todos los platos creados por un nutricionista.
        /// </summary>
        /// <param name="nutricionistaId">ID del nutricionista</param>
        /// <returns>Lista de platos creados por ese nutricionista</returns>
        [HttpGet("listar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PlatoComida>> ObtenerPlatosPorNutricionista([FromQuery] int nutricionistaId)
        {
            var platos = _context.PlatosComida
                .Where(p => p.NutricionistaId == nutricionistaId)
                .ToList();

            return Ok(platos);
        }
    }
}
