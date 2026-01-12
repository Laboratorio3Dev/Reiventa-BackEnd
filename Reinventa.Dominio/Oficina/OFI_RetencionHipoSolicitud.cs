using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Oficina
{
    [Table("RETENCION_HIPO_SOLICITUD")]
    public class OFI_RetencionHipoSolicitud
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("NRO_PRESTAMO")]
        [StringLength(30)]
        public string NRO_PRESTAMO { get; set; } = null!;

        [Column("DNI_CLIENTE")]
        [StringLength(15)]
        public string? DNI_CLIENTE { get; set; }

        [Column("MONEDA")]
        [StringLength(10)]
        public string MONEDA { get; set; } = null!;

        [Column("SALDO_CREDITO")]
        public decimal SALDO_CREDITO { get; set; }

        [Column("TASA_SOLICITADA")]
        public decimal TASA_SOLICITADA { get; set; }

        [Column("TASA_OFRECIDA")]
        public decimal? TASA_OFRECIDA { get; set; }

        [Column("TASA_RESPUESTA")]
        public decimal? TASA_RESPUESTA { get; set; }

        [Column("PRODUCTO_ID")]
        public int PRODUCTO_ID { get; set; }

        //[Column("ENTIDAD_ID")]
        //public int ENTIDAD_ID { get; set; }

        [Column("ID_USUARIO")]
        [StringLength(30)]
        public string ID_USUARIO { get; set; } = null!;

        [Column("ID_GERENTE")]
        [StringLength(30)]
        public string? ID_GERENTE { get; set; }

        [Column("DNI_GERENTE")]
        [StringLength(15)]
        public string? DNI_GERENTE { get; set; }

        [Column("ESTADO")]
        [StringLength(20)]
        public string ESTADO { get; set; } = null!;

        [Column("FECHA_REGISTRO")]
        public DateTime FECHA_REGISTRO { get; set; }

        /* ================= RELACIONES ================= */

        //[ForeignKey("PRODUCTO_ID")]
        //public RETENCION_HIPO_PRODUCTOS? Producto { get; set; }

        //[ForeignKey("ENTIDAD_ID")]
        //public RETENCION_HIPO_ENTIDAD? Entidad { get; set; }
    }
}
