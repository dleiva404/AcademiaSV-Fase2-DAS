using AcademiaSv.Microservices.Data;
using AcademiaSv.Microservices.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSv.Microservices.Controllers
{
    [Route("api/notificaciones")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly MicroservicesContext _context;

        public NotificacionesController(MicroservicesContext context)
        {
            _context = context;
        }

        // GET: api/notificaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacionDto>>> GetNotificaciones()
        {
            var notificaciones = new List<NotificacionDto>();
            var ahora = DateTime.Now;

            // Alumnos inactivos
            var alumnosInactivos = await _context.Alumnos.CountAsync(a => a.estado == "Inactivo");
            if (alumnosInactivos > 0)
                notificaciones.Add(new NotificacionDto
                {
                    Tipo    = "Alumnos",
                    Mensaje = $"Hay {alumnosInactivos} alumno(s) con estado Inactivo.",
                    Nivel   = "warning",
                    FechaGeneracion = ahora
                });

            // Secciones sin cupo
            var seccionesSinCupo = await _context.Secciones
                .Where(s => s.estado == "Abierta")
                .Select(s => new {
                    s.id_seccion,
                    s.cupo_maximo,
                    Inscritos = s.Inscripciones != null ? s.Inscripciones.Count(i => i.estado == "Activa") : 0
                })
                .Where(s => s.cupo_maximo.HasValue && s.Inscritos >= s.cupo_maximo)
                .CountAsync();

            if (seccionesSinCupo > 0)
                notificaciones.Add(new NotificacionDto
                {
                    Tipo    = "Secciones",
                    Mensaje = $"{seccionesSinCupo} sección(es) han alcanzado el cupo máximo.",
                    Nivel   = "danger",
                    FechaGeneracion = ahora
                });

            // Alumnos con notas reprobadas
            var alumnosReprobados = await _context.Notas.CountAsync(n => n.estado == "Reprobado");
            if (alumnosReprobados > 0)
                notificaciones.Add(new NotificacionDto
                {
                    Tipo    = "Notas",
                    Mensaje = $"{alumnosReprobados} alumno(s) tienen materias reprobadas.",
                    Nivel   = "warning",
                    FechaGeneracion = ahora
                });

            // Ciclo activo
            var cicloActivo = await _context.Ciclos.FirstOrDefaultAsync(c => c.estado == "Activo");
            if (cicloActivo != null)
                notificaciones.Add(new NotificacionDto
                {
                    Tipo    = "Ciclo",
                    Mensaje = $"Ciclo {cicloActivo.numero_ciclo}-{cicloActivo.anio} activo.",
                    Nivel   = "info",
                    FechaGeneracion = ahora
                });

            if (!notificaciones.Any())
                notificaciones.Add(new NotificacionDto
                {
                    Tipo    = "Sistema",
                    Mensaje = "Todo en orden. No hay alertas pendientes.",
                    Nivel   = "info",
                    FechaGeneracion = ahora
                });

            return Ok(notificaciones);
        }
    }
}
