using AcademiaSV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSV.Controllers
{
    [Route("api/inscripciones")]
    [ApiController]
    public class InscripcionesApiController : ControllerBase
    {
        private readonly AcademiaSVContext _context;

        public InscripcionesApiController(AcademiaSVContext context)
        {
            _context = context;
        }

        // GET: api/inscripciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inscripcion>>> GetInscripciones()
        {
            return await _context.Inscripciones
                .Include(i => i.Alumno)
                .Include(i => i.Seccion)
                .ToListAsync();
        }

        // GET: api/inscripciones/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Inscripcion>> GetInscripcion(int id)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Alumno)
                .Include(i => i.Seccion)
                .FirstOrDefaultAsync(i => i.id_inscripcion == id);
            if (inscripcion == null) return NotFound();
            return inscripcion;
        }

        // POST: api/inscripciones
        [HttpPost]
        public async Task<ActionResult<Inscripcion>> PostInscripcion(Inscripcion inscripcion)
        {
            // Validacion 1: prerrequisitos
            var seccion = await _context.Secciones
                .Include(s => s.Materia)
                .ThenInclude(m => m.Prerrequisitos)
                .FirstOrDefaultAsync(s => s.id_seccion == inscripcion.id_seccion);

            if (seccion == null) return BadRequest("Sección no encontrada.");

            foreach (var prereq in seccion.Materia.Prerrequisitos)
            {
                var aprobado = await _context.Notas
                    .Include(n => n.Inscripcion)
                    .AnyAsync(n => n.Inscripcion.id_alumno == inscripcion.id_alumno
                                && n.Inscripcion.Seccion.id_materia == prereq.id_materia_prerequisito
                                && n.estado == "Aprobado");
                if (!aprobado)
                    return BadRequest("El alumno no cumple con los prerrequisitos.");
            }

            // Validacion 2: cupo disponible
            var inscritos = await _context.Inscripciones
                .CountAsync(i => i.id_seccion == inscripcion.id_seccion && i.estado == "Activa");
            if (inscritos >= seccion.cupo_maximo)
                return BadRequest("No hay cupo disponible en esta sección.");

            // Validacion 3: ya inscrito
            var yaInscrito = await _context.Inscripciones
                .AnyAsync(i => i.id_alumno == inscripcion.id_alumno
                            && i.id_seccion == inscripcion.id_seccion
                            && i.estado == "Activa");
            if (yaInscrito)
                return BadRequest("El alumno ya está inscrito en esta sección.");

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInscripcion), new { id = inscripcion.id_inscripcion }, inscripcion);
        }

        // PUT: api/inscripciones/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscripcion(int id, Inscripcion inscripcion)
        {
            if (id != inscripcion.id_inscripcion) return BadRequest();
            _context.Entry(inscripcion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}