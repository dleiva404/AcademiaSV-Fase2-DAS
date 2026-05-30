using AcademiaSV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSV.Controllers.Mvc
{
    public class AlumnosController : Controller
    {
        private readonly AcademiaSVContext _context;

        public AlumnosController(AcademiaSVContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var alumnos = await _context.Alumnos
                .Include(a => a.Carrera)
                .ToListAsync();
            return View(alumnos);
        }

        public IActionResult Create()
        {
            ViewBag.Carreras = new SelectList(_context.Carreras.Where(c => c.estado == "Activa"), "id_carrera", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                alumno.fecha_creacion = DateTime.Now;
                _context.Add(alumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Carreras = new SelectList(_context.Carreras.Where(c => c.estado == "Activa"), "id_carrera", "nombre", alumno.id_carrera);
            return View(alumno);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();
            ViewBag.Carreras = new SelectList(_context.Carreras.Where(c => c.estado == "Activa"), "id_carrera", "nombre", alumno.id_carrera);
            return View(alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Alumno alumno)
        {
            if (id != alumno.id_alumno) return NotFound();
            if (ModelState.IsValid)
            {
                alumno.fecha_modificacion = DateTime.Now;
                _context.Update(alumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Carreras = new SelectList(_context.Carreras.Where(c => c.estado == "Activa"), "id_carrera", "nombre", alumno.id_carrera);
            return View(alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var alumno = await _context.Alumnos
                .Include(a => a.Inscripciones)
                    .ThenInclude(i => i.Notas)
                .FirstOrDefaultAsync(a => a.id_alumno == id);

            if (alumno != null)
            {
                foreach (var inscripcion in alumno.Inscripciones ?? new List<Inscripcion>())
                {
                    if (inscripcion.Notas != null)
                        _context.Notas.Remove(inscripcion.Notas);
                }
                _context.Inscripciones.RemoveRange(alumno.Inscripciones ?? new List<Inscripcion>());
                _context.Alumnos.Remove(alumno);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
