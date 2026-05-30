using AcademiaSV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSV.Controllers.Mvc
{
    public class MateriasController : Controller
    {
        private readonly AcademiaSVContext _context;

        public MateriasController(AcademiaSVContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var materias = await _context.Materias
                .Include(m => m.Carrera)
                .ToListAsync();
            return View(materias);
        }

        public IActionResult Create()
        {
            ViewBag.Carreras = new SelectList(_context.Carreras.Where(c => c.estado == "Activa"), "id_carrera", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Materia materia)
        {
            if (ModelState.IsValid)
            {
                materia.fecha_creacion = DateTime.Now;
                _context.Add(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Carreras = new SelectList(_context.Carreras.Where(c => c.estado == "Activa"), "id_carrera", "nombre", materia.id_carrera);
            return View(materia);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null) return NotFound();
            ViewBag.Carreras = new SelectList(_context.Carreras.Where(c => c.estado == "Activa"), "id_carrera", "nombre", materia.id_carrera);
            return View(materia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Materia materia)
        {
            if (id != materia.id_materia) return NotFound();
            if (ModelState.IsValid)
            {
                materia.fecha_modificacion = DateTime.Now;
                _context.Update(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Carreras = new SelectList(_context.Carreras.Where(c => c.estado == "Activa"), "id_carrera", "nombre", materia.id_carrera);
            return View(materia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var materia = await _context.Materias
                .Include(m => m.Prerrequisitos)
                .Include(m => m.Secciones)
                    .ThenInclude(s => s.Inscripciones)
                        .ThenInclude(i => i.Notas)
                .FirstOrDefaultAsync(m => m.id_materia == id);

            if (materia != null)
            {
                // Borrar prerrequisitos donde esta materia es requerida
                var prereqsComoRequerida = await _context.Prerrequisitos
                    .Where(p => p.id_materia_prerequisito == id)
                    .ToListAsync();
                _context.Prerrequisitos.RemoveRange(prereqsComoRequerida);

                // Borrar prerrequisitos de esta materia
                _context.Prerrequisitos.RemoveRange(materia.Prerrequisitos ?? new List<Prerrequisito>());

                // Borrar secciones y sus inscripciones/notas
                foreach (var seccion in materia.Secciones ?? new List<Seccion>())
                {
                    foreach (var inscripcion in seccion.Inscripciones ?? new List<Inscripcion>())
                    {
                        if (inscripcion.Notas != null)
                            _context.Notas.Remove(inscripcion.Notas);
                    }
                    _context.Inscripciones.RemoveRange(seccion.Inscripciones ?? new List<Inscripcion>());
                }
                _context.Secciones.RemoveRange(materia.Secciones ?? new List<Seccion>());
                _context.Materias.Remove(materia);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
