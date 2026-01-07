using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Oficina
{
    [Table("OFI_RETENCION_HIPO_CLIENTE_LOG")]
    public class OFI_RetencionHipoClienteLog
    {
        [Key]
        public int ID { get; set; }
        public string? NroDocumento { get; set; }
        public string? Tasa { get; set; }
        public DateTime? FechaCarga { get; set; }
        public string? Observacion { get; set; }
    }
}
