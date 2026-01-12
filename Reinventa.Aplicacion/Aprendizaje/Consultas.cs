using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.Aprendizaje;
using Reinventa.Persistencia.Aprendizaje;
using Reinventa.Persistencia.BackOffice;
using Reinventa.Persistencia.NPS;
using Reinventa.Utilitarios;
using Reinventa.Utilitarios.DTOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.Aprendizaje
{
    public class Consultas
    {
        public class DatosGenerales
        {
            public class ListadosGenerales : IRequest<DatosGeneralesDTO>
            {
                public string? Usuario { get; set; }
            }

            public class Manejador : IRequestHandler<ListadosGenerales, DatosGeneralesDTO>
            {
                private readonly SA_Context _context;
                public Manejador(SA_Context context)
                {
                    _context = context;
                }


                public async Task<DatosGeneralesDTO> Handle(ListadosGenerales request, CancellationToken cancellationToken)
                {
                    var hoy = DateTime.Now;
                    DatosGeneralesDTO datosGeneralesDTO = new DatosGeneralesDTO();
                    datosGeneralesDTO.ListaDimensiones = await _context.SA_DIMENSION.Where(X => X.Estado == 1)
                        .Select(e => new CombosGeneral
                        {
                            Id= e.Id_Dimension,
                            Valor = e.Dimension,
                        })
                        .ToListAsync(cancellationToken);

                    datosGeneralesDTO.ListaProductos = await _context.SA_PRODUCTO.Where(X => X.Estado == 1)

                      .Select(e => new CombosGeneral
                      {
                          Id = e.Id_Producto,
                          Valor = e.Producto,
                      })
                      .ToListAsync(cancellationToken);

                    var colaboradores = await _context.Colaboradores
                     .FromSqlRaw("EXEC USP_SA_LISTAR_COLABORADORES @USUARIO = {0}", request.Usuario)
                     .ToListAsync(cancellationToken);

                    datosGeneralesDTO.ListaColaboradores = colaboradores
                        .Select(e => new CombosGeneral
                        {
                            Id = e.IdUsuario,
                            Valor = e.UsuarioWindows
                        })
                        .ToList();

                    return datosGeneralesDTO;
                }

                
            }
        }
        public class PlanesAccion
        {
            public class ListadosPlanesAccion : IRequest<List<PlanAccionDTO>>
            {
                public string? Usuario { get; set; }
            }

            public class Manejador : IRequestHandler<ListadosPlanesAccion, List<PlanAccionDTO>>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<List<PlanAccionDTO>> Handle(ListadosPlanesAccion request, CancellationToken cancellationToken)
                {
                    // Seguridad básica: si viene null lo pasamos como empty
                    var usuario = request.Usuario ?? string.Empty;

                    var planes = await _context.PlanAcciones
                        .FromSqlRaw("EXEC USP_SA_LISTAR_PLANES_ACCION @USUARIO = {0}", usuario)
                        .ToListAsync(cancellationToken);

                    if (planes == null || planes.Count == 0)
                        return new List<PlanAccionDTO>();

                    return planes.Select(e => new PlanAccionDTO
                    {
                        ID_PLANACCION = e.ID_PLANACCION,
                        PRODUCTO = e.PRODUCTO,
                        DIMENSION = e.DIMENSION,
                        ESTADO = e.ESTADO,
                        FECHA = e.FECHA,
                        TAREA = e.TAREA,
                        USUARIO = e.USUARIO,
                        Comentario = e.ComentarioEvidencia,
                        GerenteOficina = e.GerenteOficina,
                        Oficina = e.Oficina,
                        Zona=e.Zona,
                        Evidencia = e.Evidencia == null
                        ? null
                        : new EvidenciaDTO
                        {
                            Archivo = e.Evidencia,
                            ContentType = FileHelper.DetectContentType(e.Evidencia),
                            Nombre = $"Evidencia_Plan_{e.ID_PLANACCION}" +
                                     FileHelper.GetExtension(
                                         FileHelper.DetectContentType(e.Evidencia))
                        }
                    }).ToList();
                }
            }
        }
        public class Dashboard
        {
            public class ListadosDashboard : IRequest<List<ListadoDashboard_DTO>>
            {
                public string? Usuario { get; set; }
                public int? Anio { get; set; }
                public int? Mes { get; set; }
            }

            public class Manejador : IRequestHandler<ListadosDashboard, List<ListadoDashboard_DTO>>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<List<ListadoDashboard_DTO>> Handle(ListadosDashboard request, CancellationToken cancellationToken)
                {
                    // Seguridad básica: si viene null lo pasamos como empty

                    if(request.Anio==null)
                    {
                        request.Anio = 0;
                    }
                    if (request.Mes == null)
                    {
                        request.Mes = 0;
                    }

                    var parametros = new[]
                    {
                        new SqlParameter("@USUARIO", SqlDbType.VarChar)
                        {
                            Value = request.Usuario
                        },
                        new SqlParameter("@ANIO", SqlDbType.Int)
                        {
                            Value = request.Anio
                        },
                        new SqlParameter("@MES", SqlDbType.Int)
                        {
                            Value = request.Mes
                        },
                        
                    };


                    var planes = await _context.Dashboard
                        .FromSqlRaw("EXEC USP_SA_LISTAR_DATA_DASHBOARD @USUARIO, @ANIO, @MES", parametros)
                        .ToListAsync(cancellationToken);

                    if (planes == null || planes.Count == 0)
                        return new List<ListadoDashboard_DTO>();

                    return planes.Select(e => new ListadoDashboard_DTO
                    {
                        CODIGO = e.CODIGO,
                        PRODUCTO = e.PRODUCTO,
                        DIMENSION = e.DIMENSION,
                        CUMPLIMIENTO = e.CUMPLIMIENTO,
                        EJECUTIVO = e.EJECUTIVO,
                        ZONA = e.ZONA,
                        INDICADOR = e.INDICADOR,
                        RESULTADO = e.RESULTADO,
                        UMBRAL = e.UMBRAL,
                        OFICINA = e.OFICINA,
                        ANIO=e.ANIO,
                        MES= e.MES
                    }).ToList();
                }
            }
        }
        public class ListaTareas
        {
            public class ListadoTareas: IRequest<List<TareasDTO>>
            {
                public int? Dimension{ get; set; }
                public int? Producto { get; set; }
            }

            public class Manejador : IRequestHandler<ListadoTareas, List<TareasDTO>>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<List<TareasDTO>> Handle(ListadoTareas request, CancellationToken cancellationToken)
                {
                    // Seguridad básica: si viene null lo pasamos como empty
                    var producto = request.Producto ?? 0;
                    var dimension = request.Dimension ?? 0;

                    var task = await _context.Tareas
                        .FromSqlRaw(
                            "EXEC USP_SA_LISTARTAREAS @DIMENSION = {0}, @PRODUCTO = {1}",
                            dimension,
                            producto
                        )
                        .ToListAsync(cancellationToken);

                    if (task == null || task.Count == 0)
                        return new List<TareasDTO>();

                    return task.Select(e => new TareasDTO
                    {
                        ID_TAREA = e.ID_TAREA,                       
                        TAREA = e.TAREA
                    }).ToList();
                }
            }
        }
        public class ListarComentarios
        {
            public class Ejecuta : IRequest<List<ProductoInsightDTO>>
            {
                public int? Anio { get; set; }
                public int? Mes { get; set; }
                public string? Usuario { get; set; }
            }

            public class Manejador : IRequestHandler<Ejecuta, List<ProductoInsightDTO>>
            {
                private readonly SA_Context _context;

                public Manejador(SA_Context context)
                {
                    _context = context;
                }

                public async Task<List<ProductoInsightDTO>> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    // Seguridad básica: si viene null lo pasamos como empty
                    if (request.Anio == null)
                    {
                        request.Anio = 2025;
                    }
                    if (request.Mes == null)
                    {
                        request.Mes = 12;
                    }

                    var parametros = new[]
                    {
                        new SqlParameter("@USUARIO", SqlDbType.VarChar)
                        {
                            Value = request.Usuario
                        },
                        new SqlParameter("@ANIO", SqlDbType.Int)
                        {
                            Value = request.Anio
                        },
                        new SqlParameter("@MES", SqlDbType.Int)
                        {
                            Value = request.Mes
                        },

                    };


                    var planes = await _context.Comentarios
                        .FromSqlRaw("EXEC USP_LISTAR_COMENTARIOS @ANIO, @MES,@USUARIO", parametros)
                        .ToListAsync(cancellationToken);

                    if (planes == null || planes.Count == 0)
                        return new List<ProductoInsightDTO>();

                    return planes.Select(e => new ProductoInsightDTO
                    {
                        PRODUCTO= e.PRODUCTO,
                        INSIGHT= e.INSIGHT,
                        PREGUNTAS= e. PREGUNTAS,
                        RESUMEN= e.RESUMEN
                    }).ToList();
                }
            }
        }

        public class CargarDashboardRequest
        {
            public IFormFile File { get; set; }
            public string Usuario { get; set; }
        }
    }
}
