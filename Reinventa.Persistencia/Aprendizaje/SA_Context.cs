using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.Aprendizaje;
using Reinventa.Dominio.BackOffice;



namespace Reinventa.Persistencia.Aprendizaje
{
    public class SA_Context : DbContext
    {
        public SA_Context(DbContextOptions<SA_Context> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 👇 Aquí configuras que Menu no tiene clave (porque viene de un SP)
            modelBuilder.Entity<Colaborador>().HasNoKey();
            modelBuilder.Entity<PlanAccion>().HasNoKey();
            modelBuilder.Entity<Tareas>().HasNoKey();
            modelBuilder.Entity<ProductoInsightDTO>().HasNoKey();
            modelBuilder.Entity<Dashboard_DTO>().HasNoKey();
            modelBuilder.Entity<SpResultado>().HasNoKey();
        }
        

        public DbSet<SpResultado> SpResultado { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Tareas> Tareas { get; set; }
        public DbSet<ProductoInsightDTO> Comentarios { get; set; }        
        public DbSet<PlanAccion> PlanAcciones { get; set; }
        public DbSet<Dashboard_DTO> Dashboard { get; set; }
        public DbSet<SA_PRODUCTO> SA_PRODUCTO { get; set; }
        public DbSet<SA_DIMENSION> SA_DIMENSION { get; set; }
        public DbSet<SA_DASHBOARD> SA_DASHBOARD { get; set; }
        public DbSet<SA_DASHBOARD_COMENTARIO> SA_DASHBOARD_COMENTARIO { get; set; }
        
    }

    
}
