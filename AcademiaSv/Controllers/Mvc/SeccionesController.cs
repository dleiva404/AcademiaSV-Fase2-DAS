using AcademiaSV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSV.Controllers.Mvc
{
    public class SeccionesController : Controller
    {
        private readonly AcademiaSVContext _context;

        public SeccionesController(AcademiaSVContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var secciones = await _context.Secciones
                .Include(s => s.Materia)
                .Include(s => s.Maestro)
                .Include(s => s.Ciclo)
                .ToListAsync();
            return View(secciones);
        }

        public IActionResult Create()
        {
            ViewBag.Materias = new SelectList(_context.Materias.Where(m => m.estado == "Activa"), "id_materia", "nombre");
            ViewBag.Maestros = new SelectList(_context.Maestros.Where(m => m.estado == "Activo"), "id_maestro", "nombre");
            ViewBag.Ciclos = new SelectList(_context.Ciclos.Where(c => c.estado == "Activo"), "id_ciclo", "numero_ciclo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seccion seccion)
        {
            if (ModelState.IsValid)
            {
                seccion.fecha_creacion = DateTime.Now;
                _context.Add(seccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Materias = new SelectList(_context.Materias.Where(m => m.estado == "Activa"), "id_materia", "nombre", seccion.id_materia);
            ViewBag.Maestros = new SelectList(_context.Maestros.Where(m => m.estado == "Activo"), "id_maestro", "nombre", seccion.id_maestro);
            ViewBag.Ciclos = new SelectList(_context.Ciclos.Where(c => c.estado == "Activo"), "id_ciclo", "numero_ciclo", seccion.id_ciclo);
            return View(seccion);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var seccion = await _context.Secciones.FindAsync(id);
            if (seccion == null) return NotFound();
            ViewBag.Materias = new SelectList(_context.Materias.Where(m => m.estado == "Activa"), "id_materia", "nombre", seccion.id_materia);
            ViewBag.Maestros = new SelectList(_context.Maestros.Where(m => m.estado == "Activo"), "id_maestro", "nombre", seccion.id_maestro);
            ViewBag.Ciclos = new SelectList(_context.Ciclos.Where(c => c.estado == "Activo"), "id_ciclo", "numero_ciclo", seccion.id_ciclo);
            return View(seccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seccion seccion)
        {
            if (id != seccion.id_seccion) return NotFound();
            if (ModelState.IsValid)
            {
                seccion.fecha_modificacion = DateTime.Now;
                _context.Update(seccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Materias = new SelectList(_context.Materias.Where(m => m.estado == "Activa"), "id_materia", "nombre", seccion.id_materia);
            ViewBag.Maestros = new SelectList(_context.Maestros.Where(m => m.estado == "Activo"), "id_maestro", "nombre", seccion.id_maestro);
            ViewBag.Ciclos = new SelectList(_context.Ciclos.Where(c => c.estado == "Activo"), "id_ciclo", "numero_ciclo", seccion.id_ciclo);
            return View(seccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var seccion = await _context.Secciones
                .Include(s => s.Inscripciones)
                    .ThenInclude(i => i.Notas)
                .FirstOrDefaultAsync(s => s.id_seccion == id);

            if (seccion != null)
            {
                foreach (var inscripcion in seccion.Inscripciones ?? new List<Inscripcion>())
                {
                    if (inscripcion.Notas != null)
                        _context.Notas.Remove(inscripcion.Notas);
                }
                _context.Inscripciones.RemoveRange(seccion.Inscripciones ?? new List<Inscripcion>());
                _context.Secciones.Remove(seccion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
