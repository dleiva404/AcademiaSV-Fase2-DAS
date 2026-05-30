using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;

namespace AcademiaSV.Models
{
    public class Ciclo
    {
        [Key]
        public int id_ciclo { get; set; }

        [Required]
        public int anio { get; set; }

        [Required]
        [StringLength(2)]
        public string numero_ciclo { get; set; }

        public DateOnly? fecha_inicio { get; set; }
        public DateOnly? fecha_fin { get; set; }

        [StringLength(20)]
        public string estado { get; set; } = "Activo";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public ICollection<Seccion>? Secciones { get; set; }
    }
}