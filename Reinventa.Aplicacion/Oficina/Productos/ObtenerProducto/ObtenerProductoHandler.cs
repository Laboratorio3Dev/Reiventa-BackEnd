using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Aplicacion.Oficina.Productos.ListarProductos;
using Reinventa.Persistencia.NPS;
using Reinventa.Persistencia.Oficina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.ObtenerProducto
{
    internal class ObtenerProductoHandler : IRequestHandler<ObtenerProductoQuery, ProductoDto?>
    {
        private readonly OFI_Context _context;

        public ObtenerProductoHandler(OFI_Context context)
        {
            _context = context;
        }

        public async Task<ProductoDto?> Handle(
            ObtenerProductoQuery request,
            CancellationToken cancellationToken)
        {
            var producto = await _context.OFI_Producto.FirstOrDefaultAsync(x => x.IdProducto == request.IdProducto);

            if (producto == null)
                return null;

            return new ProductoDto
            {
                IdProducto = producto.IdProducto,
                Titulo = producto.Titulo,
                SubTitulo = producto.SubTitulo,
                Asunto = producto.Asunto,
                FormatoCorreo = producto.FormatoCorreo,
                Orden = producto.Orden
            };
        }
    }
}