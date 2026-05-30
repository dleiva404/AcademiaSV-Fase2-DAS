using AcademiaSv.Microservices.Data;
using AcademiaSv.Microservices.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSv.Microservices.Controllers
{
    [Route("api/validaciones")]
    [ApiController]
    public class ValidacionesController : ControllerBase
    {
        private readonly MicroservicesContext _context;

        public ValidacionesController(MicroservicesContext context)
        {
            _context = context;
        }

        // POST: api/validaciones/inscripcion
        [HttpPost("inscripcion")]
        public async Task<ActionResult<ValidacionInscripcionDto>> ValidarInscripcion(SolicitudValidacionDto solicitud)
        {
            var resultado = new ValidacionInscripcionDto { PuedeInscribirse = true };

            // Verificar que el alumno existe y está activo
            var alumno = await _context.Alumnos.FindAsync(solicitud.IdAlumno);
            if (alumno == null)
            {
                resultado.PuedeInscribirse = false;
                resultado.Errores.Add("El alumno no existe.");
                return Ok(resultado);
            }
            if (alumno.estado != "Activo")
            {
                resultado.PuedeInscribirse = false;
                resultado.Errores.Add("El alumno no está activo en el sistema.");
            }

            // Verificar que la sección existe y está abierta
            var seccion = await _context.Secciones
                .Include(s => s.Materia)
                    .ThenInclude(m => m!.Prerrequisitos)
                .Include(s => s.Inscripciones)
                .FirstOrDefaultAsync(s => s.id_seccion == solicitud.IdSeccion);

            if (seccion == null)
            {
                resultado.PuedeInscribirse = false;
                resultado.Errores.Add("La sección no existe.");
                return Ok(resultado);
            }
            if (seccion.estado != "Abierta")
            {
                resultado.PuedeInscribirse = false;
                resultado.Errores.Add("La sección no está abierta para inscripciones.");
            }

            // Verificar cupo disponible
            var inscritos = seccion.Inscripciones?.Count(i => i.estado == "Activa") ?? 0;
            if (seccion.cupo_maximo.HasValue && inscritos >= seccion.cupo_maximo)
            {
                resultado.PuedeInscribirse = false;
                resultado.Errores.Add($"La sección no tiene cupo disponible ({inscritos}/{seccion.cupo_maximo}).");
            }
            else if (seccion.cupo_maximo.HasValue)
            {
                var disponible = seccion.cupo_maximo.Value - inscritos;
                if (disponible <= 3)
                    resultado.Advertencias.Add($"Quedan solo {disponible} lugar(es) disponibles.");
            }

            // Verificar que no esté ya inscrito
            var yaInscrito = await _context.Inscripciones
                .AnyAsync(i => i.id_alumno == solicitud.IdAlumno
                            && i.id_seccion == solicitud.IdSeccion
                            && i.estado == "Activa");
            if (yaInscrito)
            {
                resultado.PuedeInscribirse = false;
                resultado.Errores.Add("El alumno ya está inscrito en esta sección.");
            }

            // Verificar prerrequisitos
            if (seccion.Materia?.Prerrequisitos != null)
            {
                foreach (var prereq in seccion.Materia.Prerrequisitos)
                {
                    var aprobado = await _context.Notas
                        .Include(n => n.Inscripcion)
                            .ThenInclude(i => i!.Seccion)
                        .AnyAsync(n => n.Inscripcion!.id_alumno == solicitud.IdAlumno
                                    && n.Inscripcion.Seccion!.id_materia == prereq.id_materia_prerequisito
                                    && n.estado == "Aprobado");

                    if (!aprobado)
                    {
                        var materiaReq = await _context.Materias.FindAsync(prereq.id_materia_prerequisito);
                        resultado.PuedeInscribirse = false;
                        resultado.Errores.Add($"El alumno no ha aprobado el prerrequisito: {materiaReq?.nombre ?? "materia requerida"}.");
                    }
                }
            }

            return Ok(resultado);
        }

        // GET: api/validaciones/cupo/{idSeccion}
        [HttpGet("cupo/{idSeccion}")]
        public async Task<ActionResult> VerificarCupo(int idSeccion)
        {
            var seccion = await _context.Secciones
                .Include(s => s.Inscripciones)
                .FirstOrDefaultAsync(s => s.id_seccion == idSeccion);

            if (seccion == null) return NotFound("Sección no encontrada.");

            var inscritos = seccion.Inscripciones?.Count(i => i.estado == "Activa") ?? 0;
            var disponible = (seccion.cupo_maximo ?? 0) - inscritos;

            return Ok(new {
                IdSeccion       = idSeccion,
                CupoMaximo      = seccion.cupo_maximo,
                Inscritos       = inscritos,
                CupoDisponible  = disponible,
                HayCupo         = disponible > 0
            });
        }
    }
}
