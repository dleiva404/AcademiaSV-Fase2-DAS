using Microsoft.EntityFrameworkCore;

namespace AcademiaSV.Models
{
    public class AcademiaSVContext : DbContext
    {
        public AcademiaSVContext(DbContextOptions<AcademiaSVContext> options) : base(options) { }

        public DbSet<Carrera> Carreras { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Maestro> Maestros { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Prerrequisito> Prerrequisitos { get; set; }
        public DbSet<Ciclo> Ciclos { get; set; }
        public DbSet<Seccion> Secciones { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Notas> Notas { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Nombres de tablas en BD
            modelBuilder.Entity<Carrera>().ToTable("Carrera");
            modelBuilder.Entity<Alumno>().ToTable("Alumno");
            modelBuilder.Entity<Maestro>().ToTable("Maestro");
            modelBuilder.Entity<Materia>().ToTable("Materia");
            modelBuilder.Entity<Prerrequisito>().ToTable("Prerrequisito");
            modelBuilder.Entity<Ciclo>().ToTable("Ciclo");
            modelBuilder.Entity<Seccion>().ToTable("Seccion");
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<Notas>().ToTable("Notas");
            modelBuilder.Entity<Roles>().ToTable("Roles");
            modelBuilder.Entity<Usuarios>().ToTable("Usuarios");

            // Relación Prerrequisito → Materia (doble FK)
            modelBuilder.Entity<Prerrequisito>()
                .HasOne(p => p.Materia)
                .WithMany(m => m.Prerrequisitos)
                .HasForeignKey(p => p.id_materia)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prerrequisito>()
                .HasOne(p => p.MateriaRequerida)
                .WithMany()
                .HasForeignKey(p => p.id_materia_prerequisito)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuarios → Alumno
            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Alumno)
                .WithOne(a => a.Usuario)
                .HasForeignKey<Usuarios>(u => u.id_alumno)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuarios → Maestro
            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Maestro)
                .WithOne(m => m.Usuario)
                .HasForeignKey<Usuarios>(u => u.id_maestro)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}