using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.ListarProductos
{
    public class ProductoDto
    {
        public int IdProducto { get; set; }
        public string? Titulo { get; set; }
        public string? Asunto { get; set; }
        public string? SubTitulo { get; set; }
        public int? Orden { get; set; }
        public bool Activo { get; set; }
    }
}
