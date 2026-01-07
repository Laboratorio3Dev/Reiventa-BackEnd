using Aspose.Cells;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.Oficina;
using Reinventa.Persistencia.NPS;
using Reinventa.Persistencia.Oficina;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Oficina.CargarRetencion
{
    public class CargarRetencionExcelHandler
     : IRequestHandler<CargarRetencionExcelCommand, ResponseTransacciones>
    {
        private readonly OFI_Context _context;

        public CargarRetencionExcelHandler(OFI_Context context)
        {
            _context = context;
        }

        public async Task<ResponseTransacciones> Handle(
            CargarRetencionExcelCommand request,
            CancellationToken cancellationToken)
        {
            var errores = new List<string>();
            var listaPrincipal = new List<OFI_RetencionHipoCliente>();
            var listaLog = new List<OFI_RetencionHipoClienteLog>();

            using var stream = request.Archivo.OpenReadStream();
            var workbook = new Workbook(stream);
            var cells = workbook.Worksheets[0].Cells;
            int totalFilas = cells.MaxDataRow;

      
            for (int i = 1; i <= totalFilas; i++)
            {
                var nroDocumento = cells[i, 0].StringValue?.Trim();
                var tasaValor = cells[i, 1].Value;

                if (string.IsNullOrEmpty(nroDocumento)) continue;

                try
                {
                    decimal? tasa = tasaValor != null ? Convert.ToDecimal(tasaValor) * 100 : null;

                    // Agregamos a la lista de carga exitosa
                    listaPrincipal.Add(new OFI_RetencionHipoCliente
                    {
                        NroDocumento = nroDocumento,
                        Tasa = tasa
                    });

                    // Agregamos al log como éxito
                    listaLog.Add(new OFI_RetencionHipoClienteLog
                    {
                        NroDocumento = nroDocumento,
                        Tasa = tasa==null? "": tasa.ToString(),
                        FechaCarga = DateTime.Now.AddHours(-5),
                        Observacion = "CARGA EXITOSA"
                    });
                }
                catch (Exception ex)
                {
                    string msg = $"Fila {i + 1}: {ex.Message}";
                    errores.Add(msg);

                    listaLog.Add(new OFI_RetencionHipoClienteLog
                    {
                        NroDocumento = nroDocumento,
                        Observacion = "ERROR: " + msg,
                        FechaCarga = DateTime.Now
                    });
                }
            }

            // 2. Operaciones de Base de Datos en bloque
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // Limpiar tabla principal
                await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE RETENCION_HIPO_CLIENTE", cancellationToken);

                // Inserción masiva de datos válidos
                if (listaPrincipal.Any())
                {
                    await _context.OFI_RetencionHipoCliente.AddRangeAsync(listaPrincipal, cancellationToken);
                }

                // Inserción masiva de Logs (éxitos y errores)
                if (listaLog.Any())
                {
                    await _context.OFI_RetencionHipoClienteLog.AddRangeAsync(listaLog, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ResponseTransacciones { IsSuccess = false, Message = "Falla crítica en base de datos: " + ex.Message };
            }

            return new ResponseTransacciones
            {
                IsSuccess = errores.Count == 0,
                Message = errores.Any()
                    ? $"Procesado. {listaPrincipal.Count} cargados, {errores.Count} errores registrados en log."
                    : "Carga masiva completada con éxito."
            };
        }
    }
}
