using AcademiaSV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSV.Controllers.Mvc
{
    public class InscripcionesController : Controller
    {
        private readonly AcademiaSVContext _context;

        public InscripcionesController(AcademiaSVContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var inscripciones = await _context.Inscripciones
                .Include(i => i.Alumno)
                .Include(i => i.Seccion)
                    .ThenInclude(s => s.Materia)
                .ToListAsync();
            return View(inscripciones);
        }

        public IActionResult Create()
        {
            ViewBag.Alumnos = new SelectList(
                _context.Alumnos.Where(a => a.estado == "Activo").Select(a => new { a.id_alumno, NombreCompleto = a.nombre + " " + a.apellido }),
                "id_alumno", "NombreCompleto");
            ViewBag.Secciones = new SelectList(
                _context.Secciones.Include(s => s.Materia).Where(s => s.estado == "Abierta").Select(s => new { s.id_seccion, Descripcion = s.codigo_seccion + " - " + s.Materia.nombre }),
                "id_seccion", "Descripcion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inscripcion inscripcion)
        {
            if (ModelState.IsValid)
            {
                inscripcion.fecha_inscripcion = DateOnly.FromDateTime(DateTime.Now);
                inscripcion.fecha_creacion = DateTime.Now;
                _context.Add(inscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Alumnos = new SelectList(
                _context.Alumnos.Where(a => a.estado == "Activo").Select(a => new { a.id_alumno, NombreCompleto = a.nombre + " " + a.apellido }),
                "id_alumno", "NombreCompleto", inscripcion.id_alumno);
            ViewBag.Secciones = new SelectList(
                _context.Secciones.Include(s => s.Materia).Where(s => s.estado == "Abierta").Select(s => new { s.id_seccion, Descripcion = s.codigo_seccion + " - " + s.Materia.nombre }),
                "id_seccion", "Descripcion", inscripcion.id_seccion);
            return View(inscripcion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Notas)
                .FirstOrDefaultAsync(i => i.id_inscripcion == id);

            if (inscripcion != null)
            {
                if (inscripcion.Notas != null)
                    _context.Notas.Remove(inscripcion.Notas);
                _context.Inscripciones.Remove(inscripcion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
