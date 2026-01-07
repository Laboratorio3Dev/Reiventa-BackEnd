using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.Oficina;
using Reinventa.Persistencia.NPS;
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


        public DbSet<OFI_Producto> OFI_Producto { get; set; }
        public DbSet<OFI_VentaDigital> OFI_VentaDigital { get; set; }
        public DbSet<OFI_RetencionHipoCliente> OFI_RetencionHipoCliente { get; set; }
        public DbSet<OFI_RetencionHipoClienteLog> OFI_RetencionHipoClienteLog { get; set; }
      //  public DbSet<RetencionHipotecariaSolicitudDTO> RetencionHipoSolicitudes { get; set; }
    }
    
}
