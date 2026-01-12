using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Oficina
{
    [Table("RETENCION_HIPO_ENTIDAD")]
    public class OFI_RetencionHipoEntidad
    {
        public int ID { get; set; }
        public string? ENTIDAD { get; set; }
        public int ESTADO { get; set; }
    }
}
