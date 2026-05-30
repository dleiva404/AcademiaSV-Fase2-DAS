using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaSV.Models
{
    public class Alumno
    {
        [Key]
        public int id_alumno { get; set; }

        [Required]
        [ForeignKey("Carrera")]
        public int id_carrera { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string apellido { get; set; }

        [Required]
        [StringLength(20)]
        public string carnet { get; set; }

        [StringLength(10)]
        public string? dui { get; set; }

        [StringLength(15)]
        public string? telefono { get; set; }

        [StringLength(250)]
        public string? direccion { get; set; }

        public DateOnly? fecha_ingreso { get; set; }

        [StringLength(20)]
        public string estado { get; set; } = "Activo";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public Carrera? Carrera { get; set; }
        public ICollection<Inscripcion>? Inscripciones { get; set; }
        public Usuarios? Usuario { get; set; }
    }
}