using AcademiaSV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSV.Controllers.Mvc
{
    public class MaestrosController : Controller
    {
        private readonly AcademiaSVContext _context;

        public MaestrosController(AcademiaSVContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Maestros.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Maestro maestro)
        {
            if (ModelState.IsValid)
            {
                maestro.fecha_creacion = DateTime.Now;
                _context.Add(maestro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(maestro);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var maestro = await _context.Maestros.FindAsync(id);
            if (maestro == null) return NotFound();
            return View(maestro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Maestro maestro)
        {
            if (id != maestro.id_maestro) return NotFound();
            if (ModelState.IsValid)
            {
                maestro.fecha_modificacion = DateTime.Now;
                _context.Update(maestro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(maestro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var maestro = await _context.Maestros
                .Include(m => m.Secciones)
                    .ThenInclude(s => s.Inscripciones)
                        .ThenInclude(i => i.Notas)
                .FirstOrDefaultAsync(m => m.id_maestro == id);

            if (maestro != null)
            {
                foreach (var seccion in maestro.Secciones ?? new List<Seccion>())
                {
                    foreach (var inscripcion in seccion.Inscripciones ?? new List<Inscripcion>())
                    {
                        if (inscripcion.Notas != null)
                            _context.Notas.Remove(inscripcion.Notas);
                    }
                    _context.Inscripciones.RemoveRange(seccion.Inscripciones ?? new List<Inscripcion>());
                }
                _context.Secciones.RemoveRange(maestro.Secciones ?? new List<Seccion>());
                _context.Maestros.Remove(maestro);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
