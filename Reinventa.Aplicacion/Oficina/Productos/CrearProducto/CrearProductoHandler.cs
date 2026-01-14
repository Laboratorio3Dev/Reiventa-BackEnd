using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reinventa.Dominio.Oficina;
using Reinventa.Persistencia.NPS;
using Reinventa.Persistencia.Oficina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.Productos.CrearProducto
{
    public class CrearProductoHandler
     : IRequestHandler<CrearProductoCommand, ResponseTransacciones>
    {
        private readonly OFI_Context _context;
        private readonly ILogger<CrearProductoHandler> _logger;
        public CrearProductoHandler(OFI_Context context, ILogger<CrearProductoHandler> logger)
        {
             _context = context;
            _logger = logger;
        }
   
        public async Task<ResponseTransacciones> Handle(
            CrearProductoCommand request,
            CancellationToken cancellationToken)
        {
 
            var ordenNuevo = request.Orden;
            // Desplazar los existentes
            var productosAfectados = await _context.OFI_Producto
                .Where(p => p.Orden >= ordenNuevo)
                .OrderByDescending(p => p.Orden)
                .ToListAsync(cancellationToken);

            foreach (var p in productosAfectados)
            {
                p.Orden++;
            }

            var producto = new OFI_Producto
            {
                Titulo = request.Titulo,
                SubTitulo = request.SubTitulo,
                Asunto = request.Asunto,
                FormatoCorreo = request.FormatoCorreo,
                Orden = ordenNuevo,
                Activo = true
            };

            _context.OFI_Producto.Add(producto);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result == 0)
            {
                return new ResponseTransacciones
                {
                    IsSuccess = false,
                    Message = "No se pudo registrar el producto",
                    IdValue = 0
                };
            }
         
            return new ResponseTransacciones
            {
                IsSuccess = true,
                Message = "Producto registrado correctamente",
                IdValue = producto.IdProducto
            };

         
        }
    }
}
