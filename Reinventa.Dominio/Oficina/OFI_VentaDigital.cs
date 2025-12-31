using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Oficina
{
    [Table("OFI_VentaDigital")]
    public class OFI_VentaDigital
    {
        [Key]
        public int IdVentaDigital { get; set; }

        [MaxLength(200)]
        public string CorreoCliente { get; set; }

        public DateTime FechaRegistro { get; set; }

        [MaxLength(50)]
        public string UsuarioRegistro { get; set; }

        // FK lógica al producto
        public int ProductoSeleccionado { get; set; }

        public bool OfertaEnviada { get; set; }

        // HTML REAL enviado al cliente
        [Column(TypeName = "varchar(max)")]
        public string HtmlEnviado { get; set; }

        [MaxLength(20)]
        public string DocumentoCliente { get; set; }

        [MaxLength(200)]
        public string NombreCliente { get; set; }

        [MaxLength(20)]
        public string CodigoVendedor { get; set; }

        [MaxLength(10)]
        public string CodOficina { get; set; }
    }
}
