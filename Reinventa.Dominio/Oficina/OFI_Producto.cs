using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Dominio.Oficina
{
    [Table("OFI_Producto")]
    public class OFI_Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [MaxLength(50)]
        public string? Titulo { get; set; }

        [MaxLength(100)]
        public string? SubTitulo { get; set; }

        [MaxLength(100)]
        public string? Asunto { get; set; }

        public string? FormatoCorreo { get; set; }

        public int? Orden { get; set; }

        public bool Activo { get; set; }
    }
}
