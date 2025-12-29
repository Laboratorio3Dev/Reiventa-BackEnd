using MediatR;
using Reinventa.Dominio.Oficina;
using Reinventa.Persistencia.NPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.CrearProducto
{
    public class CrearProductoHandler
     : IRequestHandler<CrearProductoCommand, int>
    {
        private readonly NPS_Context _context;

        public CrearProductoHandler(NPS_Context context)
        {
            _context = context;
        }

        public async Task<int> Handle(
            CrearProductoCommand request,
            CancellationToken cancellationToken)
        {
            var producto = new OFI_Producto
            {
                Titulo = request.Titulo,
                SubTitulo = request.SubTitulo,
                Asunto = request.Asunto,
                FormatoCorreo = request.FormatoCorreo,
                Orden = request.Orden,
                Activo = request.Activo
            };

            _context.OFI_Producto.Add(producto);
            await _context.SaveChangesAsync(cancellationToken);

            return producto.IdProducto;
        }
    }
}
