using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaSv.Microservices.Models
{
    public class Alumno
    {
        [Key] public int id_alumno { get; set; }
        public int id_carrera { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string carnet { get; set; }
        public string? telefono { get; set; }
        public string estado { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public Carrera? Carrera { get; set; }
        public ICollection<Inscripcion>? Inscripciones { get; set; }
    }

    public class Maestro
    {
        [Key] public int id_maestro { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string? especialidad { get; set; }
        public string? correo { get; set; }
        public string estado { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public ICollection<Seccion>? Secciones { get; set; }
    }

    public class Carrera
    {
        [Key] public int id_carrera { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public string estado { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
    }

    public class Ciclo
    {
        [Key] public int id_ciclo { get; set; }
        public int anio { get; set; }
        public string numero_ciclo { get; set; }
        public string estado { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
    }

    public class Materia
    {
        [Key] public int id_materia { get; set; }
        public int id_carrera { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int? unidades_valorativas { get; set; }
        public string estado { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public ICollection<Prerrequisito>? Prerrequisitos { get; set; }
    }

    public class Seccion
    {
        [Key] public int id_seccion { get; set; }
        public int id_materia { get; set; }
        public int id_maestro { get; set; }
        public int id_ciclo { get; set; }
        public string codigo_seccion { get; set; }
        public int? cupo_maximo { get; set; }
        public string? horario_dia { get; set; }
        public string estado { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public Materia? Materia { get; set; }
        public Maestro? Maestro { get; set; }
        public Ciclo? Ciclo { get; set; }
        public ICollection<Inscripcion>? Inscripciones { get; set; }
    }

    public class Inscripcion
    {
        [Key] public int id_inscripcion { get; set; }
        public int id_alumno { get; set; }
        public int id_seccion { get; set; }
        public DateOnly fecha_inscripcion { get; set; }
        public string estado { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public Alumno? Alumno { get; set; }
        public Seccion? Seccion { get; set; }
        public Notas? Notas { get; set; }
    }

    public class Notas
    {
        [Key] public int id_nota { get; set; }
        [ForeignKey("Inscripcion")] public int id_inscripcion { get; set; }
        public decimal? nota_parcial1 { get; set; }
        public decimal? nota_parcial2 { get; set; }
        public decimal? nota_ultimo_parcial { get; set; }
        public decimal? nota_global { get; set; }
        public string? estado { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public Inscripcion? Inscripcion { get; set; }
    }

    public class Prerrequisito
    {
        [Key] public int id_prerequisito { get; set; }
        public int id_materia { get; set; }
        public int id_materia_prerequisito { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
        public Materia? Materia { get; set; }
        [ForeignKey("id_materia_prerequisito")] public Materia? MateriaRequerida { get; set; }
    }
}
