using FluentValidation;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reinventa.Aplicacion.ManejadorErrores;
using Reinventa.Dominio.Aprendizaje;
using Reinventa.Dominio.NPS;
using Reinventa.Persistencia.Aprendizaje;
using Reinventa.Persistencia.NPS;
using Reinventa.Utilitarios;
using Reinventa.Utilitarios.DTOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Aprendizaje
{
    public class Transacciones
    {
        public class IniciarPlan
        {
            public class InicioPlanAccion : IRequest<ResponseTransacciones>
            {
                public int IdPlan { get; set; }
            }

            public class Manejador : IRequestHandler<InicioPlanAccion, ResponseTransacciones>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(InicioPlanAccion request, CancellationToken cancellationToken)
                {

                    var parametros = new[]
                        {
                            new SqlParameter("@ID_PLANACCION", request.IdPlan),
                            new SqlParameter("@FECHA",  DateTime.Now.AddHours(-5))
                        };

                    await _context.Database.ExecuteSqlRawAsync(
                            "EXEC USP_SA_INICIAR_ACCION @ID_PLANACCION, @FECHA",
                            parametros,
                            cancellationToken
                    );

                    return new ResponseTransacciones
                    {
                        Message = "Plan iniciado correctamente",
                        IsSuccess = true,
                        IdValue = request.IdPlan
                    };
                }
            }


        }

        public class EnviarGO
        {
            public class EnvioPlanGO : IRequest<ResponseTransacciones>
            {
                public int IdPlan { get; set; }
                public string? Comentario { get; set; }
                public byte[]? Evidencia { get; set; }
            }

            public class Manejador : IRequestHandler<EnvioPlanGO, ResponseTransacciones>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(EnvioPlanGO request, CancellationToken cancellationToken)
                {

                    var parametros = new[]
                         {
                            new SqlParameter("@ID_PLANACCION", SqlDbType.Int)
                            {
                                Value = request.IdPlan
                            },
                            new SqlParameter("@FECHA", SqlDbType.DateTime)
                            {
                                Value = DateTime.Now.AddHours(-5)
                            },
                            new SqlParameter("@COMENTARIO", SqlDbType.NVarChar)
                            {
                                Value = (object?)request.Comentario ?? DBNull.Value
                            },
                            new SqlParameter("@Evidencia", SqlDbType.VarBinary, -1)
                            {
                                Value = (object?)request.Evidencia ?? DBNull.Value
                            }
                        };


                    await _context.Database.ExecuteSqlRawAsync(
                            "EXEC USP_SA_ENVIAR_GO @ID_PLANACCION, @FECHA, @COMENTARIO, @Evidencia",
                            parametros,
                            cancellationToken
                    );

                    return new ResponseTransacciones
                    {
                        Message = "Plan Enviado a GO correctamente",
                        IsSuccess = true,
                        IdValue = request.IdPlan
                    };
                }
            }


        }

        public class RechazoGO
        {
            public class RechazoPlanGO : IRequest<ResponseTransacciones>
            {
                public int IdPlan { get; set; }
                public string? Comentario { get; set; }
                public DateTime FechaLimite { get; set; }
                public string? Usuario { get; set; }
            }

            public class Manejador : IRequestHandler<RechazoPlanGO, ResponseTransacciones>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(RechazoPlanGO request, CancellationToken cancellationToken)
                {

                    var parametros = new[]
                    {
                        new SqlParameter("@ID_PLANACCION", SqlDbType.Int)
                        {
                            Value = request.IdPlan
                        },
                        new SqlParameter("@FECHA", SqlDbType.DateTime)
                        {
                            Value = DateTime.Now.AddHours(-5)
                        },
                        new SqlParameter("@COMENTARIO", SqlDbType.NVarChar)
                        {
                            Value = (object?)request.Comentario ?? DBNull.Value
                        },
                        new SqlParameter("@USUARIO", SqlDbType.NVarChar, 40)
                        {
                            Value = (object?)request.Usuario ?? DBNull.Value
                        },
                        new SqlParameter("@FECHA_LIMITE", SqlDbType.DateTime)
                        {
                            Value = request.FechaLimite
                        }
                    };


                    await _context.Database.ExecuteSqlRawAsync(
                            "EXEC USP_SA_RECHAZO_GO @ID_PLANACCION, @FECHA, @COMENTARIO, @USUARIO, @FECHA_LIMITE",
                            parametros,
                            cancellationToken
                    );

                    return new ResponseTransacciones
                    {
                        Message = "Plan Rechazado por el GO correctamente",
                        IsSuccess = true,
                        IdValue = request.IdPlan
                    };
                }
            }


        }

        public class CompletarPlan
        {
            public class CompletarPlanGO : IRequest<ResponseTransacciones>
            {
                public int IdPlan { get; set; }
                public string? Usuario { get; set; }
            }

            public class Manejador : IRequestHandler<CompletarPlanGO, ResponseTransacciones>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(CompletarPlanGO request, CancellationToken cancellationToken)
                {
                    var parametros = new[]
                         {
                            new SqlParameter("@ID_PLANACCION", SqlDbType.Int)
                            {
                                Value = request.IdPlan
                            },
                            new SqlParameter("@FECHA", SqlDbType.DateTime)
                            {
                                Value = DateTime.Now.AddHours(-5)
                            },
                            new SqlParameter("@USUARIO", SqlDbType.NVarChar)
                            {
                                Value = (object?)request.Usuario ?? DBNull.Value
                            }
                        };

                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC USP_SA_COMPLETAR_TAREA @ID_PLANACCION, @FECHA, @USUARIO",
                        parametros,
                        cancellationToken
                    );

                    return new ResponseTransacciones
                    {
                        Message = "Plan Completado por el GO correctamente",
                        IsSuccess = true,
                        IdValue = request.IdPlan
                    };
                }
            }


        }

        public class AsignarTareas
        {
            public class EjecutaTareas : IRequest<ResponseTransacciones>
            {
                public List<CreaTareaDTO> creaTareaDTOs { get; set; }
            }
                    

            public class Manejador : IRequestHandler<EjecutaTareas, ResponseTransacciones>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(EjecutaTareas request, CancellationToken cancellationToken)
                {
                    try
                    {
                        foreach (var tarea in request.creaTareaDTOs)
                        {

                            var parametros = new[]
                            {
                                new SqlParameter("@ID_TAREA", SqlDbType.Int)
                                {
                                    Value = tarea.Id_Tarea
                                },
                                new SqlParameter("@FECHA", SqlDbType.DateTime)
                                {
                                    Value = tarea.FechaTarea
                                },
                                new SqlParameter("@USUARIO", SqlDbType.NVarChar, 40)
                                {
                                    Value = tarea.Usuario
                                }
                            };


                            await _context.Database.ExecuteSqlRawAsync(
                                    "EXEC USP_SA_ASIGNAR_TAREA @ID_TAREA, @FECHA, @USUARIO",
                                    parametros,
                                    cancellationToken
                            );

                        }

                        return new ResponseTransacciones
                        {
                            Message = "Planes creados correctamente",
                            IsSuccess = true,
                            IdValue = 1
                        };
                    }
                    catch (Exception ex)
                    {
                        return new ResponseTransacciones
                        {
                            Message = ex.Message,
                            IsSuccess = false,
                            IdValue = 0
                        };
                    }
                   
                }
            }


        }

        public class CargarDataDashoard
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public string Usuario { get; set; }
                public List<SA_DASHBOARD> sA_DASHBOARDs { get; set; }
                public List<SA_DASHBOARD_COMENTARIO> sA_DASHBOARD_COMENTARIOs { get; set; }
            }

          
            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly SA_Context _context;
                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    if (request.sA_DASHBOARD_COMENTARIOs.Count > 0)
                    {
                        _context.SA_DASHBOARD_COMENTARIO.AddRange(request.sA_DASHBOARD_COMENTARIOs.Select(dato =>
                         new SA_DASHBOARD_COMENTARIO
                         {
                             USUARIO = dato.USUARIO,
                             ID_PRODUCTO = dato.ID_PRODUCTO,
                             RESUMEN = dato.RESUMEN,
                             INSIGHT= dato.INSIGHT,
                             PREGUNTAS= dato.PREGUNTAS,
                             USUARIO_CARGA = request.Usuario,
                             MES= dato.MES,
                             ANIO=dato.ANIO,
                             FECHA_CARGA = DateTime.Now.AddHours(-5)
                         }));
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    if (request.sA_DASHBOARDs.Count > 0)
                    {
                        _context.SA_DASHBOARD.AddRange(request.sA_DASHBOARDs.Select(dato =>
                         new SA_DASHBOARD
                         {
                             ID_DIMENSION= dato.ID_DIMENSION,
                             ANIO= dato.ANIO,
                             USUARIO= dato.USUARIO,
                             CUMPLIMIENTO= dato.CUMPLIMIENTO,
                             ID_PRODUCTO= dato.ID_PRODUCTO,
                             INDICADOR=dato.INDICADOR,  
                             MES= dato.MES,
                             RESULTADO= dato.RESULTADO,
                             UMBRAL= dato.UMBRAL,
                             USUARIO_CARGA = request.Usuario,
                             FECHA_CARGA = DateTime.Now.AddHours(-5)
                         }
                     ));

                        var retorno = await _context.SaveChangesAsync(cancellationToken);
                        retorno = 1;

                        if (retorno == 0)
                        {
                            return new ResponseTransacciones
                            {
                                Message = "Solicitud Incorrecta",
                                IsSuccess = false,
                                IdValue = 0
                            };

                        }

                        return new ResponseTransacciones
                        {
                            Message = "Solicitud Correcta",
                            IsSuccess = true,
                            IdValue = 1
                        };
                    }
                    else
                    {
                        return new ResponseTransacciones
                        {
                            Message = "Solicitud Correcta",
                            IsSuccess = true,
                            IdValue = 1
                        };
                    }                    
                }
            }
        }

    }
}
