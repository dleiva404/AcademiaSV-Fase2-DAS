using AcademiaSV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSV.Controllers
{
    [Route("api/alumnos")]
    [ApiController]
    public class AlumnosApiController : ControllerBase
    {
        private readonly AcademiaSVContext _context;

        public AlumnosApiController(AcademiaSVContext context)
        {
            _context = context;
        }

        // GET: api/alumnos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> GetAlumnos()
        {
            return await _context.Alumnos.Include(a => a.Carrera).ToListAsync();
        }

        // GET: api/alumnos/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Alumno>> GetAlumno(int id)
        {
            var alumno = await _context.Alumnos.Include(a => a.Carrera)
                                               .FirstOrDefaultAsync(a => a.id_alumno == id);
            if (alumno == null) return NotFound();
            return alumno;
        }

        // POST: api/alumnos
        [HttpPost]
        public async Task<ActionResult<Alumno>> PostAlumno(Alumno alumno)
        {
            _context.Alumnos.Add(alumno);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAlumno), new { id = alumno.id_alumno }, alumno);
        }

        // PUT: api/alumnos/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlumno(int id, Alumno alumno)
        {
            if (id != alumno.id_alumno) return BadRequest();
            _context.Entry(alumno).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}