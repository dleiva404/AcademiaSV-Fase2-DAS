using AcademiaSv.Microservices.Data;
using AcademiaSv.Microservices.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSv.Microservices.Controllers
{
    [Route("api/reportes")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly MicroservicesContext _context;

        public ReportesController(MicroservicesContext context)
        {
            _context = context;
        }

        // GET: api/reportes/resumen
        [HttpGet("resumen")]
        public async Task<ActionResult<ResumenGeneralDto>> GetResumen()
        {
            var resumen = new ResumenGeneralDto
            {
                TotalAlumnos        = await _context.Alumnos.CountAsync(),
                TotalMaestros       = await _context.Maestros.CountAsync(),
                TotalMaterias       = await _context.Materias.CountAsync(),
                TotalSecciones      = await _context.Secciones.CountAsync(),
                TotalInscripciones  = await _context.Inscripciones.CountAsync(),
                AlumnosActivos      = await _context.Alumnos.CountAsync(a => a.estado == "Activo"),
                InscripcionesActivas= await _context.Inscripciones.CountAsync(i => i.estado == "Activa")
            };
            return Ok(resumen);
        }

        // GET: api/reportes/notas
        [HttpGet("notas")]
        public async Task<ActionResult<ReporteNotasDto>> GetReporteNotas()
        {
            var notas = await _context.Notas
                .Include(n => n.Inscripcion)
                    .ThenInclude(i => i.Seccion)
                        .ThenInclude(s => s.Materia)
                .ToListAsync();

            if (!notas.Any())
                return Ok(new ReporteNotasDto());

            var aprobados = notas.Count(n => n.estado == "Aprobado");
            var reprobados = notas.Count(n => n.estado == "Reprobado");
            var promedio = notas.Where(n => n.nota_global.HasValue)
                                .Average(n => n.nota_global!.Value);

            var porMateria = notas
                .GroupBy(n => n.Inscripcion?.Seccion?.Materia?.nombre ?? "Sin materia")
                .Select(g => new {
                    Materia = g.Key,
                    Aprobados = g.Count(n => n.estado == "Aprobado"),
                    Reprobados = g.Count(n => n.estado == "Reprobado")
                }).ToList();

            var reporte = new ReporteNotasDto
            {
                TotalAprobados  = aprobados,
                TotalReprobados = reprobados,
                PromedioGlobal  = Math.Round(promedio, 2),
                MateriaConMasAprobados  = porMateria.OrderByDescending(m => m.Aprobados).FirstOrDefault()?.Materia ?? "-",
                MateriaConMasReprobados = porMateria.OrderByDescending(m => m.Reprobados).FirstOrDefault()?.Materia ?? "-"
            };

            return Ok(reporte);
        }

        // GET: api/reportes/notas-por-seccion
        [HttpGet("notas-por-seccion")]
        public async Task<ActionResult> GetNotasPorSeccion()
        {
            var resultado = await _context.Secciones
                .Include(s => s.Materia)
                .Include(s => s.Inscripciones)
                    .ThenInclude(i => i.Notas)
                .Select(s => new {
                    Seccion = s.codigo_seccion,
                    Materia = s.Materia != null ? s.Materia.nombre : "Sin materia",
                    TotalInscritos  = s.Inscripciones != null ? s.Inscripciones.Count : 0,
                    Aprobados       = s.Inscripciones != null ? s.Inscripciones.Count(i => i.Notas != null && i.Notas.estado == "Aprobado") : 0,
                    Reprobados      = s.Inscripciones != null ? s.Inscripciones.Count(i => i.Notas != null && i.Notas.estado == "Reprobado") : 0,
                    Promedio        = s.Inscripciones != null && s.Inscripciones.Any(i => i.Notas != null && i.Notas.nota_global.HasValue)
                                        ? Math.Round(s.Inscripciones.Where(i => i.Notas != null && i.Notas.nota_global.HasValue).Average(i => i.Notas!.nota_global!.Value), 2)
                                        : 0
                })
                .ToListAsync();

            return Ok(resultado);
        }
    }
}
