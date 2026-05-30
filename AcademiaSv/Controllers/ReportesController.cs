using AcademiaSV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSV.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AcademiaSVContext _context;

        public ReportesController(AcademiaSVContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalAlumnos = await _context.Alumnos.CountAsync();
            ViewBag.TotalMaestros = await _context.Maestros.CountAsync();
            ViewBag.TotalMaterias = await _context.Materias.CountAsync();
            ViewBag.TotalSecciones = await _context.Secciones.CountAsync();
            ViewBag.TotalInscripciones = await _context.Inscripciones.CountAsync();

            return View();
        }
    }
}
