using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Oficina
{
    [Table("RETENCION_HIPO_PRODUCTOS")]
    public class OFI_RetencionHipoProductos
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("PRODUCTO")]
        [StringLength(30)]
        public string PRODUCTO { get; set; } = null!;

        [Column("ESTADO")]
        public int? ESTADO { get; set; } // 1 = Activo, 0 = Inactivo
    }
}
