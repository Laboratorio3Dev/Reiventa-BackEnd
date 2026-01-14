using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.Oficina;
using Reinventa.Persistencia.NPS;
using Reinventa.Persistencia.Oficina.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Persistencia.Oficina
{
    public class OFI_Context : DbContext
    {
        public OFI_Context(DbContextOptions<OFI_Context> options)
        : base(options)
        { }

        // Añade esta línea:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<RetencionHipoSolicitudSpResult>()
                .HasNoKey(); 
        }
        public DbSet<OFI_Producto> OFI_Producto { get; set; }
        public DbSet<OFI_VentaDigital> OFI_VentaDigital { get; set; }
        public DbSet<OFI_RetencionHipoCliente> OFI_RetencionHipoCliente { get; set; }
        public DbSet<OFI_RetencionHipoClienteLog> OFI_RetencionHipoClienteLog { get; set; }
       public DbSet<OFI_RetencionHipoSolicitud> OFI_RetencionHipoSolicitud { get; set; }
       public DbSet<OFI_RetencionHipoProductos> OFI_RetencionHipoProductos { get; set; }
       public DbSet<OFI_RetencionHipoEntidad> OFI_RetencionHipoEntidad { get; set; }
        
    }
    
}
