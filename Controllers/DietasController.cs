using Microsoft.AspNetCore.Mvc;
using NutriCheck.Models;
using NutriCheck.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NutriCheck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DietasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DietasController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asigna una nueva dieta a un paciente, incluyendo el tipo de comida y plato asignado.
        /// </summary>
        /// <param name="dieta">Objeto Dieta que contiene la información de la dieta a asignar.</param>
        /// <returns>El objeto Dieta creado con su Id asignado.</returns>
        [HttpPost]
        public async Task<ActionResult<Dieta>> AsignarDieta([FromBody] Dieta dieta)
        {
            if (dieta.PacienteId <= 0 || dieta.PlatoComidaId <= 0)
                return BadRequest("Paciente y plato deben estar seleccionados.");

            if (string.IsNullOrWhiteSpace(dieta.Tipo))
                return BadRequest("El tipo de comida (desayuno, almuerzo, etc.) es obligatorio.");

            _context.Dietas.Add(dieta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AsignarDieta), new { id = dieta.Id }, dieta);
        }

        /// <summary>
        /// Obtiene las dietas asignadas a un paciente en una fecha específica.
        /// </summary>
        /// <param name="pacienteId">Id del paciente para el que se consultarán las dietas.</param>
        /// <param name="fecha">Fecha específica para obtener las dietas asignadas.</param>
        /// <returns>Una lista de dietas asignadas al paciente en la fecha indicada.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<object>> ObtenerDietasPorPacienteYFecha(
            [FromQuery] int pacienteId,
            [FromQuery] DateTime fecha)
        {
            var dietas = _context.Dietas
                .Where(d => d.PacienteId == pacienteId && d.Fecha.Date == fecha.Date)
                .Select(d => new
                {
                    d.Id,
                    d.Tipo,
                    d.Fecha,
                    d.PacienteId,
                    d.PlatoComidaId,
                    NombrePlato = d.Plato != null ? d.Plato.Nombre : "No disponible",
                    CaloriasAprox = d.Plato != null ? d.Plato.CaloriasAprox : 0,
                    ProteinasAprox = d.Plato != null ? d.Plato.ProteinasAprox : 0
                })
                .ToList();

            return Ok(dietas);
        }

        /// <summary>
        /// Exporta las dietas asignadas a un paciente en una fecha específica a un archivo PDF.
        /// </summary>
        /// <param name="pacienteId">Id del paciente para el que se generará el PDF.</param>
        /// <param name="fecha">Fecha para la que se generará el PDF.</param>
        /// <returns>El archivo PDF generado con las dietas asignadas.</returns>
        [HttpGet("{pacienteId}/pdf")]
        public IActionResult ExportarDietaPDF(int pacienteId, [FromQuery] DateTime fecha)
        {
            var dietas = _context.Dietas
                .Where(d => d.PacienteId == pacienteId && d.Fecha.Date == fecha.Date)
                .Include(d => d.Plato)
                .ToList();

            if (!dietas.Any())
                return NotFound("No hay dieta asignada para este paciente en la fecha seleccionada.");

            var pdf = GenerarPDF(dietas);
            return File(pdf, "application/pdf", $"dieta_{pacienteId}_{fecha:yyyyMMdd}.pdf");
        }

        private byte[] GenerarPDF(List<Dieta> dietas)
        {
            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Plan de dieta diaria").FontSize(20).Bold().AlignCenter();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Tipo").Bold();
                            header.Cell().Text("Comida");
                            header.Cell().Text("Calorías / Proteínas");
                        });

                        foreach (var d in dietas)
                        {
                            table.Cell().Text(d.Tipo);
                            table.Cell().Text(d.Plato?.Nombre ?? "-");
                            table.Cell().Text($"{d.Plato?.CaloriasAprox} cal / {d.Plato?.ProteinasAprox}g prot");
                        }
                    });
                });
            });

            return documento.GeneratePdf();
        }
    }
}
