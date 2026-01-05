using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Persistencia.NPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.ActualizarProducto
{
    internal class ActualizarProductoHandler
   : IRequestHandler<ActualizarProductoCommand, ResponseTransacciones>
    {
        private readonly NPS_Context _context;

        public ActualizarProductoHandler(NPS_Context context)
        {
            _context = context;
        }

        public async Task<ResponseTransacciones> Handle(
            ActualizarProductoCommand request,
            CancellationToken cancellationToken)
        {
            var producto = await _context.OFI_Producto
                .FirstOrDefaultAsync(p => p.IdProducto == request.IdProducto, cancellationToken);

            if (producto == null)
            {
                return new ResponseTransacciones
                {
                    IsSuccess = false,
                    Message = "Producto no encontrado"
                };
            }

            // 🔁 Reordenar solo si cambió el orden
            if (producto.Orden != request.Orden)
            {
                var ordenNuevo = request.Orden;
                var afectados = await _context.OFI_Producto
                    .Where(p => p.IdProducto != request.IdProducto &&
                                p.Orden >= request.Orden)
                    .OrderByDescending(p => p.Orden)
                    .ToListAsync(cancellationToken);

                foreach (var p in afectados)
                    p.Orden++;
                producto.Orden = ordenNuevo;
            }

            producto.Titulo = request.Titulo;
            producto.SubTitulo = request.SubTitulo;
            producto.Asunto = request.Asunto;
            producto.FormatoCorreo = request.FormatoCorreo;
            

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseTransacciones
            {
                IsSuccess = true,
                Message = "Producto actualizado correctamente",
                IdValue = producto.IdProducto
            };
        }
    }

}