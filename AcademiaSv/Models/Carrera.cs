using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaSV.Models
{
    public class Carrera
    {
        [Key]
        public int id_carrera { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [StringLength(500)]
        public string? descripcion { get; set; }

        public int? duracion { get; set; }

        [StringLength(20)]
        public string estado { get; set; } = "Activa";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public ICollection<Alumno>? Alumnos { get; set; }
        public ICollection<Materia>? Materias { get; set; }
    }
}