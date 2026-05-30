using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaSV.Models
{
    public class Usuarios
    {
        [Key]
        public int id_usuario { get; set; }

        [Required]
        [ForeignKey("Roles")]
        public int id_rol { get; set; }

        public int? id_alumno { get; set; }
        public int? id_maestro { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre_usuario { get; set; }

        [Required]
        [StringLength(150)]
        public string correo { get; set; }

        [Required]
        [StringLength(255)]
        public string password_hash { get; set; }

        [StringLength(20)]
        public string estado { get; set; } = "Activo";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public Roles? Roles { get; set; }

        [ForeignKey("id_alumno")]
        public Alumno? Alumno { get; set; }

        [ForeignKey("id_maestro")]
        public Maestro? Maestro { get; set; }
    }
}