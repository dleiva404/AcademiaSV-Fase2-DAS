namespace AcademiaSv.Microservices.DTOs
{
    // Reportes
    public class ResumenGeneralDto
    {
        public int TotalAlumnos { get; set; }
        public int TotalMaestros { get; set; }
        public int TotalMaterias { get; set; }
        public int TotalSecciones { get; set; }
        public int TotalInscripciones { get; set; }
        public int AlumnosActivos { get; set; }
        public int InscripcionesActivas { get; set; }
    }

    public class ReporteNotasDto
    {
        public int TotalAprobados { get; set; }
        public int TotalReprobados { get; set; }
        public decimal PromedioGlobal { get; set; }
        public string MateriaConMasAprobados { get; set; }
        public string MateriaConMasReprobados { get; set; }
    }

    public class NotaAlumnoDto
    {
        public string NombreAlumno { get; set; }
        public string Materia { get; set; }
        public string Seccion { get; set; }
        public decimal? NotaParcial1 { get; set; }
        public decimal? NotaParcial2 { get; set; }
        public decimal? NotaUltimoParcial { get; set; }
        public decimal? NotaGlobal { get; set; }
        public string Estado { get; set; }
    }

    // Notificaciones
    public class NotificacionDto
    {
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public string Nivel { get; set; } // info, warning, danger
        public DateTime FechaGeneracion { get; set; }
    }

    // Validaciones
    public class ValidacionInscripcionDto
    {
        public bool PuedeInscribirse { get; set; }
        public List<string> Errores { get; set; } = new();
        public List<string> Advertencias { get; set; } = new();
    }

    public class SolicitudValidacionDto
    {
        public int IdAlumno { get; set; }
        public int IdSeccion { get; set; }
    }

    // Consultas Académicas
    public class HistorialAlumnoDto
    {
        public string NombreAlumno { get; set; }
        public string Carnet { get; set; }
        public string Carrera { get; set; }
        public int TotalMaterias { get; set; }
        public int MateriasAprobadas { get; set; }
        public int MateriasReprobadas { get; set; }
        public decimal PromedioGeneral { get; set; }
        public List<NotaAlumnoDto> Notas { get; set; } = new();
    }
}
