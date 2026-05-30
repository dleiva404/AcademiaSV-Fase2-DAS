using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaSV.Models
{
    public class Inscripcion
    {
        [Key]
        public int id_inscripcion { get; set; }

        [Required]
        [ForeignKey("Alumno")]
        public int id_alumno { get; set; }

        [Required]
        [ForeignKey("Seccion")]
        public int id_seccion { get; set; }

        public DateOnly fecha_inscripcion { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [StringLength(20)]
        public string estado { get; set; } = "Activa";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public Alumno? Alumno { get; set; }
        public Seccion? Seccion { get; set; }
        public Notas? Notas { get; set; }
    }
}