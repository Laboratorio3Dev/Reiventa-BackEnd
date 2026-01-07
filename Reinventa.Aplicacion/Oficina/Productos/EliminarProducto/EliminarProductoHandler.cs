using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Persistencia.NPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.EliminarProducto
{
    public class EliminarProductoHandler : IRequestHandler<EliminarProductoCommand, ResponseTransacciones>
    {
        private readonly NPS_Context _context;

        public EliminarProductoHandler(NPS_Context context)
        {
            _context = context;
        }

        public async Task<ResponseTransacciones> Handle(
            EliminarProductoCommand request,
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

            if (!producto.Activo)
            {
                return new ResponseTransacciones
                {
                    IsSuccess = false,
                    Message = "El producto ya está inactivo"
                };
            }

            var ordenDesactivado = producto.Orden;

            // 🔴 Desactivar
            producto.Activo = false;

            // 🔄 Reordenar solo los activos
            var productosAfectados = await _context.OFI_Producto
                .Where(p => p.Activo && p.Orden > ordenDesactivado)
                .OrderBy(p => p.Orden)
                .ToListAsync(cancellationToken);

            foreach (var p in productosAfectados)
            {
                p.Orden--;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseTransacciones
            {
                IsSuccess = true,
                Message = "Producto desactivado correctamente",
                IdValue = producto.IdProducto
            };
        }
    }

}