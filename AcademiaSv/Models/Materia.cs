using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Collections.Specialized.BitVector32;

namespace AcademiaSV.Models
{
    public class Materia
    {
        [Key]
        public int id_materia { get; set; }

        [Required]
        [ForeignKey("Carrera")]
        public int id_carrera { get; set; }

        [Required]
        [StringLength(20)]
        public string codigo { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [StringLength(500)]
        public string? descripcion { get; set; }

        public int? anio_plan { get; set; }
        public int? unidades_valorativas { get; set; }

        [StringLength(20)]
        public string estado { get; set; } = "Activa";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public Carrera? Carrera { get; set; }
        public ICollection<Seccion>? Secciones { get; set; }
        public ICollection<Prerrequisito>? Prerrequisitos { get; set; }
    }
}