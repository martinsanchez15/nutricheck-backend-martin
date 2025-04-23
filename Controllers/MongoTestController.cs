using Microsoft.AspNetCore.Mvc;
using NutriCheck.Data;
using NutriCheck.Models;
using MongoDB.Driver;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MongoTestController : ControllerBase
    {
        private readonly MongoDbService _mongoService;

        public MongoTestController(MongoDbService mongoService)
        {
            _mongoService = mongoService;
        }

        [HttpGet("pacientes")]
        public ActionResult<List<Paciente>> ObtenerPacientesMongo()
        {
            var pacientes = _mongoService.Pacientes.Find(_ => true).ToList();
            return Ok(pacientes);
        }
    }
}
