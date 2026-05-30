using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaSV.Models
{
    public class Seccion
    {
        [Key]
        public int id_seccion { get; set; }

        [Required]
        [ForeignKey("Materia")]
        public int id_materia { get; set; }

        [Required]
        [ForeignKey("Maestro")]
        public int id_maestro { get; set; }

        [Required]
        [ForeignKey("Ciclo")]
        public int id_ciclo { get; set; }

        [Required]
        [StringLength(10)]
        public string codigo_seccion { get; set; }

        public int? cupo_maximo { get; set; }

        [StringLength(30)]
        public string? horario_dia { get; set; }

        public TimeOnly? hora_inicio { get; set; }
        public TimeOnly? hora_fin { get; set; }

        [StringLength(20)]
        public string estado { get; set; } = "Abierta";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public Materia? Materia { get; set; }
        public Maestro? Maestro { get; set; }
        public Ciclo? Ciclo { get; set; }
        public ICollection<Inscripcion>? Inscripciones { get; set; }
    }
}