using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.Oficina;
using Reinventa.Infraestructura.Auth;
using Reinventa.Infraestructura.Email;
using Reinventa.Persistencia.NPS;
using Reinventa.Persistencia.Oficina;
using Reinventa.Persistencia.Oficina.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Reinventa.Aplicacion.Oficina.RetencionHipotecaria.CrearRetencionHipotecaria
{
    public class CrearRetencionHipotecariaHandler
     : IRequestHandler<CrearRetencionHipotecariaCommand, ResponseTransacciones>
    {
        private readonly OFI_Context _context;
        private readonly ITokenService _tokenService;
        private readonly ICorreoService _correoService;
        public CrearRetencionHipotecariaHandler(OFI_Context context, ITokenService tokenService, ICorreoService correoService)
        {
            _context = context;
            _correoService = correoService;
            _tokenService = tokenService;

        }

        public async Task<ResponseTransacciones> Handle(
      CrearRetencionHipotecariaCommand request,
      CancellationToken cancellationToken)
        {
            // 1️⃣ Token
            var token = await _tokenService.ObtenerTokenAsync();

            // 2️⃣ Ejecutar SP
            var resultado = await _context
                .Set<RetencionHipoSolicitudSpResult>()
                .FromSqlRaw(
                    @"EXEC SP_RETENCION_HIPO_CREAR_SOLICITUD 
              @USUARIO,
              @PRODUCTO_ID,
              @DNI,
              @MONEDA,
              @TASA_SOL,
              @SALDO,
              @TASA_OFR,
              @ENTIDAD",
                    new SqlParameter("@USUARIO", request.Usuario),
                    new SqlParameter("@PRODUCTO_ID", request.ProductoId),
                    new SqlParameter("@DNI", request.NroPrestamo),
                    new SqlParameter("@MONEDA", request.Moneda),
                    new SqlParameter("@TASA_SOL", request.TasaSolicitada),
                    new SqlParameter("@SALDO", request.SaldoCredito),
                    new SqlParameter("@TASA_OFR", (object?)request.TasaOfrecida ?? DBNull.Value),
                    new SqlParameter("@ENTIDAD", request.EntidadId)
                )
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // 3️⃣ Validar
            if (!resultado.Any())
            {
                return new ResponseTransacciones
                {
                    IsSuccess = false,
                    Message = "El cliente no cuenta con una campaña de retención, seguir el flujo regular a través del Buzón de Retención Hipotecaria, adjuntando los sustentos correspondientes"
                };
            }

            var data = resultado.FirstOrDefault();
            
            // 4️⃣ Enviar correo (NO rompe la transacción)
           // await EnviarCorreoRespuestaAsync(token, data);

            // 5️⃣ Respuesta
            return new ResponseTransacciones
            {
                IsSuccess = true,
                Message = "Solicitud registrada correctamente",
                IdValue = data.ID,
                Data = data
            };
        }



        private async Task EnviarCorreoRespuestaAsync(
    string token,
    RetencionHipoSolicitudSpResult data)
        {
            string monedaTexto = data.MONEDA switch
            {
                "S" => "Soles",
                "D" => "Dólares",
                _ => "No definido"
            };

            string asunto =
                $"RESPUESTA DE TASA DE RETENCIÓN: Nro Préstamo: {data.DNI_CLIENTE}";

            string contenido = $@"
        <p>Estimado(a),</p>

        <p>Detallamos la respuesta a su solicitud.</p>

        <p><strong>SALDO DEL CRÉDITO:</strong> {data.SALDO_CREDITO:N2}</p>
        <p><strong>MONEDA DEL CRÉDITO:</strong> {monedaTexto}</p>
        <p><strong>TASA DE RETENCIÓN APROBADA:</strong> {data.TASA_OFRECIDA:0.00}%</p>

        <p>
            <em>Nota: Esta oferta de retención está vigente hasta el 
            {DateTime.Now.AddDays(30):dd/MM/yyyy}.</em>
        </p>

        <br/>
        <p>Saludos,<br/>
        <strong>Retención Hipotecaria</strong></p>
    ";

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
                        asunto,
                        contenido,
                        destinatario = new[]
                        {
                    new
                    {
                       // enderecoCorreo = data.CORREO_CLIENTE
                    }
                }
                    }
                }
            };

            await _correoService.EnviarCorreoAsync(token, payloadCorreo);
        }

    }

}
