using System.Diagnostics;
using AcademiaSv.Models;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaSV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string MicroserviceUrl = "https://localhost:58655";

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("microservices");

            // Resumen general
            try
            {
                var resumen = await client.GetFromJsonAsync<ResumenDto>($"{MicroserviceUrl}/api/reportes/resumen");
                ViewBag.Resumen = resumen;
            }
            catch { ViewBag.Resumen = null; }

            // Notificaciones
            try
            {
                var notificaciones = await client.GetFromJsonAsync<List<NotificacionDto>>($"{MicroserviceUrl}/api/notificaciones");
                ViewBag.Notificaciones = notificaciones;
            }
            catch { ViewBag.Notificaciones = new List<NotificacionDto>(); }

            return View();
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    // DTOs locales para deserializar respuesta del microservicio
    public class ResumenDto
    {
        public int TotalAlumnos { get; set; }
        public int TotalMaestros { get; set; }
        public int TotalMaterias { get; set; }
        public int TotalSecciones { get; set; }
        public int TotalInscripciones { get; set; }
        public int AlumnosActivos { get; set; }
        public int InscripcionesActivas { get; set; }
    }

    public class NotificacionDto
    {
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public string Nivel { get; set; }
        public DateTime FechaGeneracion { get; set; }
    }
}
