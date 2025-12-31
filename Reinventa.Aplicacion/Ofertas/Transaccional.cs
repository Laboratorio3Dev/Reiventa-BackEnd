using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reinventa.Aplicacion.Contratos;
using Reinventa.Dominio.NPS;
using Reinventa.Dominio.Ofertas;
using Reinventa.Persistencia.Aprendizaje;
using Reinventa.Persistencia.BackOffice;
using Reinventa.Persistencia.HuellaCarbono;
using Reinventa.Persistencia.NPS;
using Reinventa.Utilitarios;
using Reinventa.Utilitarios.DTOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Ofertas
{
    public class Transaccional
    {
        public class ValidarOferta
        {
            public class BusquedaEjecutar : IRequest<PLD_BaseCliente>
            {
                public string? Documento { get; set; }
                public string? Usuario { get; set; }                
            }

            public class Manejador : IRequestHandler<BusquedaEjecutar, PLD_BaseCliente>
            {
                private readonly LAB_Context _context;
                private readonly IJwtGenerador _jwtGenerador;
                public Manejador(LAB_Context context, IJwtGenerador jwtGenerador)
                {
                    _context = context;
                    _jwtGenerador = jwtGenerador;
                }

                public async Task<PLD_BaseCliente?> Handle(BusquedaEjecutar request,CancellationToken cancellationToken)
                {
                    var oferta = await _context.PLD_BaseCliente
                        .FirstOrDefaultAsync(x => x.DOCUMENTO == request.Documento, cancellationToken);

                    int flagBase = oferta != null ? 1 : 0;

                    var parametros = new[]
                    {
                        new SqlParameter("@USUARIO", SqlDbType.VarChar, 40)
                        {
                            Value = string.IsNullOrWhiteSpace(request.Usuario)
                                ? "SISTEMA"
                                : request.Usuario
                        },
                        new SqlParameter("@DOCUMENTO", SqlDbType.VarChar, 20)
                        {
                            Value = request.Documento ?? string.Empty
                        },
                        new SqlParameter("@FECHA", SqlDbType.DateTime)
                        {
                            Value = DateTime.Now.AddHours(-5)
                        },
                        new SqlParameter("@FLAGBASE", SqlDbType.Int)
                        {
                            Value = flagBase
                        }
                    };

                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC USP_PLD_AGREGARSOLICITUD @USUARIO, @DOCUMENTO, @FECHA, @FLAGBASE",
                        parametros,
                        cancellationToken
                    );

                    // 👉 Si NO hay oferta, retornar null (controlado)
                    if (oferta == null)
                        return null;

                    // 👉 Si hay oferta, mapear
                    return new PLD_BaseCliente
                    {
                        Id = oferta.Id,
                        DOCUMENTO = oferta.DOCUMENTO,
                        OF_S_PLDD_12 = oferta.OF_S_PLDD_12,
                        OF_S_PLDD_18 = oferta.OF_S_PLDD_18,
                        OF_S_PLDD_24 = oferta.OF_S_PLDD_24,
                        OF_S_PLDD_36 = oferta.OF_S_PLDD_36,
                        TASA_S_PLDD_12_FIN = oferta.TASA_S_PLDD_12_FIN,
                        TASA_S_PLDD_18_FIN = oferta.TASA_S_PLDD_18_FIN,
                        TASA_S_PLDD_24_FIN = oferta.TASA_S_PLDD_24_FIN,
                        TASA_S_PLDD_36_FIN = oferta.TASA_S_PLDD_36_FIN
                    };
                }


            }


        }

        public class CargarBaseClientes
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public IFormFile Archivo { get; set; } = default!;
            }
           

            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly LAB_Context _context;
                public Manejador(LAB_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    var connection = (SqlConnection)_context.Database.GetDbConnection();
                    await connection.OpenAsync(cancellationToken);

                    using var transaction = connection.BeginTransaction();

                    try
                    {
                        // TRUNCATE
                        new SqlCommand(
                            "TRUNCATE TABLE PLD_BaseCliente",
                            connection,
                            transaction
                        ).ExecuteNonQuery();

                        using var bulk = new SqlBulkCopy(
                            connection,
                            SqlBulkCopyOptions.TableLock,
                            transaction)
                        {
                            DestinationTableName = "PLD_BaseCliente",
                            BatchSize = 10000,
                            BulkCopyTimeout = 0
                        };

                        // columnas...
                        bulk.ColumnMappings.Add("DOCUMENTO", "DOCUMENTO");
                        bulk.ColumnMappings.Add("OF_S_PLDD_12", "OF_S_PLDD_12");
                        bulk.ColumnMappings.Add("OF_S_PLDD_18", "OF_S_PLDD_18");
                        bulk.ColumnMappings.Add("OF_S_PLDD_24", "OF_S_PLDD_24");
                        bulk.ColumnMappings.Add("OF_S_PLDD_36", "OF_S_PLDD_36");
                        bulk.ColumnMappings.Add("TASA_S_PLDD_12_FIN", "TASA_S_PLDD_12_FIN");
                        bulk.ColumnMappings.Add("TASA_S_PLDD_18_FIN", "TASA_S_PLDD_18_FIN");
                        bulk.ColumnMappings.Add("TASA_S_PLDD_24_FIN", "TASA_S_PLDD_24_FIN");
                        bulk.ColumnMappings.Add("TASA_S_PLDD_36_FIN", "TASA_S_PLDD_36_FIN");
                        
                        using var reader = new StreamReader(
                            request.Archivo.OpenReadStream(),
                            Encoding.UTF8
                        );

                        var table = CrearDataTable();

                        bool primera = true;
                        int total = 0;

                        while (!reader.EndOfStream)
                        {
                            var linea = await reader.ReadLineAsync();
                            if (string.IsNullOrWhiteSpace(linea)) continue;

                            if (primera) { primera = false; continue; }

                            var c = linea.Split('\t');

                            table.Rows.Add(
                                c[0], c[1], c[2], c[3], c[4],
                                c[5], c[6], c[7], c[8]
                            );

                            total++;

                            if (table.Rows.Count == 10000)
                            {
                                await bulk.WriteToServerAsync(table, cancellationToken);
                                table.Clear();
                            }
                        }

                        if (table.Rows.Count > 0)
                            await bulk.WriteToServerAsync(table, cancellationToken);

                        transaction.Commit();

                        return new ResponseTransacciones
                        {
                            IsSuccess = true,
                            Message = $"Carga masiva finalizada. Registros: {total:N0}",
                            IdValue = total
                        };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                private static DataTable CrearDataTable()
                {
                    var table = new DataTable();

                    table.Columns.Add("DOCUMENTO", typeof(string));
                    table.Columns.Add("OF_S_PLDD_12", typeof(decimal));
                    table.Columns.Add("OF_S_PLDD_18", typeof(decimal));
                    table.Columns.Add("OF_S_PLDD_24", typeof(decimal));
                    table.Columns.Add("OF_S_PLDD_36", typeof(decimal));
                    table.Columns.Add("TASA_S_PLDD_12_FIN", typeof(decimal));
                    table.Columns.Add("TASA_S_PLDD_18_FIN", typeof(decimal));
                    table.Columns.Add("TASA_S_PLDD_24_FIN", typeof(decimal));
                    table.Columns.Add("TASA_S_PLDD_36_FIN", typeof(decimal));

                    return table;
                }


            }


        }
    }
}
