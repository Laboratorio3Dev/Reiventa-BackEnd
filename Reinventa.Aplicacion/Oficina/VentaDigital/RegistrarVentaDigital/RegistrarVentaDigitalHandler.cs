using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.Oficina;
using Reinventa.Infraestructura.Auth;
using Reinventa.Infraestructura.Email;
using Reinventa.Persistencia.NPS;
using Reinventa.Persistencia.Oficina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.VentaDigital.RegistrarVentaDigital
{
    public class RegistrarVentaDigitalHandler
     : IRequestHandler<RegistrarVentaDigitalCommand, ResponseTransacciones>
    {
        private readonly OFI_Context _context;
        private readonly ITokenService _tokenService;
        private readonly ICorreoService _correoService;

        public RegistrarVentaDigitalHandler(
            OFI_Context context,
             ITokenService tokenService,
              ICorreoService correoService
           )
        {
            _context = context;
            _tokenService = tokenService;
            _correoService = correoService;

        }

        public async Task<ResponseTransacciones> Handle(
            RegistrarVentaDigitalCommand command,
            CancellationToken ct)
        {
            var req = command.Request;


            var token = await _tokenService.ObtenerTokenAsync();

            var productos = await _context.OFI_Producto
                .Where(p => req.ProductosSeleccionados.Contains(p.IdProducto))
                .ToListAsync(ct);

            if (productos.Count != req.ProductosSeleccionados.Count)
                throw new Exception("Uno o más productos no existen");


            foreach (var producto in productos)
            {
                var registro = new OFI_VentaDigital
                {
                    CorreoCliente = req.CorreoCliente,
                    FechaRegistro = DateTime.Now.AddHours(-5),
                    UsuarioRegistro = req.UsuarioRegistro,
                    ProductoSeleccionado = producto.IdProducto,
                    OfertaEnviada = false,

                    DocumentoCliente = req.DocumentoCliente,
                    NombreCliente = req.NombreCliente,
                    CodigoVendedor = req.CodigoVendedor,
                    CodOficina = req.CodOficina,
                    HtmlEnviado = producto.FormatoCorreo
                };

                 _context.OFI_VentaDigital.Add(registro);
                await _context.SaveChangesAsync(ct);

                            

                var payloadCorreo = new
                {
                    enviarCorreo = new
                    {
                        templateCorreo = new
                        {
                            remetente = new
                            {
                                enderecoCorreo = "BancaDigital@banbif.com.pe"
                            },
                            asunto = producto.Asunto.Replace("{Nombre}", registro.NombreCliente),
                            contenido = producto.FormatoCorreo,
                          
                            destinatario = new[]
                            {
                                new
                                {
                                    enderecoCorreo = req.CorreoCliente
                                }
                            }
                        }
                    }
                };




                await _correoService.EnviarCorreoAsync(token, payloadCorreo);
                registro.OfertaEnviada = true;
                await _context.SaveChangesAsync(ct);
            }

            return new ResponseTransacciones
            {
                IsSuccess = true,
                Message = "Venta digital registrada correctamente"
            };
        }
    }
}

