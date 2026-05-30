using AcademiaSv.Microservices.Data;
using AcademiaSv.Microservices.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSv.Microservices.Controllers
{
    [Route("api/consultas")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly MicroservicesContext _context;

        public ConsultasController(MicroservicesContext context)
        {
            _context = context;
        }

        // GET: api/consultas/historial/{idAlumno}
        [HttpGet("historial/{idAlumno}")]
        public async Task<ActionResult<HistorialAlumnoDto>> GetHistorial(int idAlumno)
        {
            var alumno = await _context.Alumnos
                .Include(a => a.Carrera)
                .Include(a => a.Inscripciones!)
                    .ThenInclude(i => i.Seccion!)
                        .ThenInclude(s => s.Materia)
                .Include(a => a.Inscripciones!)
                    .ThenInclude(i => i.Notas)
                .FirstOrDefaultAsync(a => a.id_alumno == idAlumno);

            if (alumno == null) return NotFound("Alumno no encontrado.");

            var notas = alumno.Inscripciones?
                .Where(i => i.Notas != null)
                .Select(i => new NotaAlumnoDto
                {
                    NombreAlumno        = $"{alumno.nombre} {alumno.apellido}",
                    Materia             = i.Seccion?.Materia?.nombre ?? "Sin materia",
                    Seccion             = i.Seccion?.codigo_seccion ?? "-",
                    NotaParcial1        = i.Notas?.nota_parcial1,
                    NotaParcial2        = i.Notas?.nota_parcial2,
                    NotaUltimoParcial   = i.Notas?.nota_ultimo_parcial,
                    NotaGlobal          = i.Notas?.nota_global,
                    Estado              = i.Notas?.estado ?? "-"
                }).ToList() ?? new List<NotaAlumnoDto>();

            var historial = new HistorialAlumnoDto
            {
                NombreAlumno        = $"{alumno.nombre} {alumno.apellido}",
                Carnet              = alumno.carnet,
                Carrera             = alumno.Carrera?.nombre ?? "Sin carrera",
                TotalMaterias       = notas.Count,
                MateriasAprobadas   = notas.Count(n => n.Estado == "Aprobado"),
                MateriasReprobadas  = notas.Count(n => n.Estado == "Reprobado"),
                PromedioGeneral     = notas.Any(n => n.NotaGlobal.HasValue)
                                        ? Math.Round(notas.Where(n => n.NotaGlobal.HasValue).Average(n => n.NotaGlobal!.Value), 2)
                                        : 0,
                Notas = notas
            };

            return Ok(historial);
        }

        // GET: api/consultas/alumnos-por-carrera
        [HttpGet("alumnos-por-carrera")]
        public async Task<ActionResult> GetAlumnosPorCarrera()
        {
            var resultado = await _context.Carreras
                .Select(c => new {
                    Carrera  = c.nombre,
                    Total    = c.id_carrera,
                    Alumnos  = _context.Alumnos.Count(a => a.id_carrera == c.id_carrera)
                })
                .ToListAsync();

            return Ok(resultado);
        }

        // GET: api/consultas/inscripciones-por-seccion
        [HttpGet("inscripciones-por-seccion")]
        public async Task<ActionResult> GetInscripcionesPorSeccion()
        {
            var resultado = await _context.Secciones
                .Include(s => s.Materia)
                .Include(s => s.Maestro)
                .Select(s => new {
                    Seccion     = s.codigo_seccion,
                    Materia     = s.Materia != null ? s.Materia.nombre : "Sin materia",
                    Maestro     = s.Maestro != null ? s.Maestro.nombre + " " + s.Maestro.apellido : "Sin maestro",
                    Inscritos   = s.Inscripciones != null ? s.Inscripciones.Count(i => i.estado == "Activa") : 0,
                    CupoMaximo  = s.cupo_maximo ?? 0
                })
                .ToListAsync();

            return Ok(resultado);
        }

        // GET: api/consultas/buscar-alumno/{carnet}
        [HttpGet("buscar-alumno/{carnet}")]
        public async Task<ActionResult> BuscarAlumno(string carnet)
        {
            var alumno = await _context.Alumnos
                .Include(a => a.Carrera)
                .FirstOrDefaultAsync(a => a.carnet == carnet);

            if (alumno == null) return NotFound("Alumno no encontrado.");

            return Ok(new {
                alumno.id_alumno,
                NombreCompleto  = $"{alumno.nombre} {alumno.apellido}",
                alumno.carnet,
                Carrera         = alumno.Carrera?.nombre ?? "Sin carrera",
                alumno.estado
            });
        }
    }
}
