using System.ComponentModel.DataAnnotations;

namespace AcademiaSV.Models
{
    public class Roles
    {
        [Key]
        public int id_rol { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre_rol { get; set; }

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public ICollection<Usuarios>? Usuarios { get; set; }
    }
}