using Microsoft.EntityFrameworkCore;

using Reinventa.Dominio.HuellaCarbono;
using Reinventa.Dominio.NPS;
using Reinventa.Persistencia.NPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Persistencia.HuellaCarbono
{
    public class Huella_Context: DbContext
    {


        public Huella_Context(DbContextOptions<Huella_Context> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
        }

        public DbSet<HUELLACARBONO_SOLICITUDCLIENTE> HUELLACARBONO_SOLICITUDCLIENTE { get; set; }
        public DbSet<HUELLACARBONO_CLIENTE> HUELLACARBONO_CLIENTE { get; set; }
        public DbSet<HUELLACARBONO_RESULTADO> HUELLACARBONO_RESULTADO { get; set; }

        
    }
}
