using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.CrearProducto
{
    public record CrearProductoCommand(
     string Titulo,
     string SubTitulo,
     string Asunto,
     string FormatoCorreo,
     int Orden,
     bool Activo
 ) : IRequest<int>;
}
