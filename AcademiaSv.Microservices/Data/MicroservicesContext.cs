using AcademiaSv.Microservices.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademiaSv.Microservices.Data
{
    public class MicroservicesContext : DbContext
    {
        public MicroservicesContext(DbContextOptions<MicroservicesContext> options) : base(options) { }

        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Maestro> Maestros { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Seccion> Secciones { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Notas> Notas { get; set; }
        public DbSet<Ciclo> Ciclos { get; set; }
        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Prerrequisito> Prerrequisitos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Alumno>().ToTable("Alumno");
            modelBuilder.Entity<Maestro>().ToTable("Maestro");
            modelBuilder.Entity<Materia>().ToTable("Materia");
            modelBuilder.Entity<Seccion>().ToTable("Seccion");
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<Notas>().ToTable("Notas");
            modelBuilder.Entity<Ciclo>().ToTable("Ciclo");
            modelBuilder.Entity<Carrera>().ToTable("Carrera");
            modelBuilder.Entity<Prerrequisito>().ToTable("Prerrequisito");

            modelBuilder.Entity<Prerrequisito>()
                .HasOne(p => p.Materia)
                .WithMany()
                .HasForeignKey(p => p.id_materia)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prerrequisito>()
                .HasOne(p => p.MateriaRequerida)
                .WithMany()
                .HasForeignKey(p => p.id_materia_prerequisito)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
