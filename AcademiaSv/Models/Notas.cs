using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaSV.Models
{
    public class Notas
    {
        [Key]
        public int id_nota { get; set; }

        [Required]
        [ForeignKey("Inscripcion")]
        public int id_inscripcion { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? nota_parcial1 { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? nota_parcial2 { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? nota_ultimo_parcial { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? nota_global { get; set; }

        [StringLength(20)]
        public string? estado { get; set; }

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public DateTime? fecha_modificacion { get; set; }

        // Navegación
        public Inscripcion? Inscripcion { get; set; }
    }
}