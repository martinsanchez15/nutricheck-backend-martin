using Microsoft.AspNetCore.Mvc;
using NutriCheck.Data;
using NutriCheck.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DietasController : ControllerBase
    {
        private readonly MongoDbService _db;
        public DietasController(MongoDbService db) => _db = db;

        [HttpGet]
        public ActionResult<List<Dieta>> GetAll() =>
            Ok(_db.Dietas.Find(_ => true).ToList());

        [HttpPost]
        public ActionResult<Dieta> Create(Dieta d)
        {
            if (string.IsNullOrWhiteSpace(d.Titulo)) 
                return BadRequest("Título requerido");
            _db.Dietas.InsertOne(d);
            return CreatedAtAction(nameof(GetAll), new { id = d.Id }, d);
        }

        [HttpGet("export/{id:length(24)}")]
        public IActionResult ExportPdf(string id)
        {
            var dieta = _db.Dietas.Find(x => x.Id == id).FirstOrDefault();
            if (dieta == null) return NotFound();

            try
            {
                var bytes = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.Content().Text(dieta.Titulo);
                    });
                }).GeneratePdf();

                return File(bytes, "application/pdf", $"{dieta.Titulo}.pdf");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error generando PDF");
            }
        }
    }
}
