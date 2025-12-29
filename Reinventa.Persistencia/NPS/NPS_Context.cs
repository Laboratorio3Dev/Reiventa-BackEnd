using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.Aprendizaje;
using Reinventa.Dominio.BackOffice;
using Reinventa.Dominio.NPS;
using Reinventa.Dominio.Oficina;
using Reinventa.Dominio.UsuarioRoles;


namespace Reinventa.Persistencia.NPS
{
    public class NPS_Context: DbContext
    {
        public NPS_Context(DbContextOptions<NPS_Context> options)
        : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NPS_ClienteEncuesta>()
              .HasOne(c => c.Encuesta)
              .WithMany(e => e.ClientesEncuesta)
              .HasForeignKey(c => c.IdEncuesta);

            modelBuilder.Entity<NPS_DetalleRespuesta>()
                .HasOne(d => d.Respuesta)
                .WithMany(r => r.DetallesRespuesta)
                .HasForeignKey(d => d.IdRespuesta);

            modelBuilder.Entity<NPS_EncuestaPregunta>()
                .HasOne(ep => ep.Encuesta)
                .WithMany(e => e.EncuestaPreguntas)
                .HasForeignKey(ep => ep.IdEncuesta);

            modelBuilder.Entity<NPS_Respuesta>()
                .HasOne(r => r.Encuesta)
                .WithMany(e => e.Respuestas)
                .HasForeignKey(r => r.IdEncuesta);

        }

        public DbSet<NPS_ClienteEncuesta> NPS_ClienteEncuesta { get; set; }
        public DbSet<NPS_DetalleRespuesta> NPS_DetalleRespuesta { get; set; }
        public DbSet<NPS_Encuesta> NPS_Encuesta { get; set; }
        public DbSet<NPS_EncuestaPregunta> NPS_EncuestaPregunta { get; set; }
        public DbSet<NPS_Respuesta> NPS_Respuesta { get; set; }
        public DbSet<SA_PRODUCTO> SA_PRODUCTO { get; set; }
        public DbSet<SA_DIMENSION> SA_DIMENSION { get; set; }
        public DbSet<OFI_Producto> OFI_Producto { get; set; }

    }
}
