using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;

namespace AcademiaSV.Models
{
    public class Maestro
    {
        [Key]
        public int id_maestro { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string apellido { get; set; }

        [StringLength(10)]
        public string? dui { get; set; }

        [StringLength(100)]
        public string? especialidad { get; set; }

        [StringLength(100)]
        public string? titulo { get; set; }

        [StringLength(15)]
        public string? telefono { get; set; }

        [StringLength(150)]
        public string? correo { get; set; }

        [StringLength(20)]
        public string estado { get; set; } = "Activo";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public ICollection<Seccion>? Secciones { get; set; }
        public Usuarios? Usuario { get; set; }
    }
}