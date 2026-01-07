using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Oficina
{
    [Table("RETENCION_HIPO_CLIENTE")]
    public class OFI_RetencionHipoCliente
    {
       
        [Key] 
        public int ID { get; set; }
        public string? NroDocumento { get; set; }
        public decimal? Tasa { get; set; }
    }
}
