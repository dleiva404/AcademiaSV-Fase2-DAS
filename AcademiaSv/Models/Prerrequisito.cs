using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaSV.Models
{
    public class Prerrequisito
    {
        [Key]
        public int id_prerequisito { get; set; }

        [Required]
        [ForeignKey("Materia")]
        public int id_materia { get; set; }

        [Required]
        public int id_materia_prerequisito { get; set; }

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public Materia? Materia { get; set; }

        [ForeignKey("id_materia_prerequisito")]
        public Materia? MateriaRequerida { get; set; }
    }
}