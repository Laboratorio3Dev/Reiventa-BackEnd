using FluentValidation;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reinventa.Aplicacion.ManejadorErrores;
using Reinventa.Dominio.HuellaCarbono;
using Reinventa.Persistencia.HuellaCarbono;
using System.Net;


namespace Reinventa.Aplicacion.HuellaCarbono
{
    public class Transacciones
    {
        public class Nuevo
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public int IdCliente { get; set; }
                public string? Ciiu { get; set; }
                public string? NombreContacto { get; set; }
                public string? PuestoContacto { get; set; }
                public string? CorreoContacto { get; set; }
                public string? TelefonoContacto { get; set; }
                public decimal? DieselEquipo { get; set; }
                public decimal? GasolinaEquipo { get; set; }
                public decimal? GLPEquipo { get; set; }
                public decimal? GNVEquipo { get; set; }
                public decimal? DieselVehiculo { get; set; }
                public decimal? GasolinaVehiculo { get; set; }
                public decimal? GLPVehiculo { get; set; }
                public decimal? GNVVehiculo { get; set; }
                public decimal? Electricidad { get; set; }
                public decimal? Agua { get; set; }
                public decimal? HojasA4 { get; set; }
                public decimal? HojasA3 { get; set; }
                public decimal? Plasticos { get; set; }
                public decimal? PapelCarton { get; set; }
                public decimal? Residuos { get; set; }
                public decimal? Metal { get; set; }
                public decimal? Raee { get; set; }
            }

            public class EjecutaValidacion : AbstractValidator<Ejecuta>
            {
                public EjecutaValidacion()
                {
                    RuleFor(x => x.IdCliente).NotEmpty();
                }
            }

            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly Huella_Context _context;
                public Manejador(Huella_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(Ejecuta request, CancellationToken cancellation)
                {
                    var New_Registro = new HUELLACARBONO_SOLICITUDCLIENTE
                    {
                        FechaRegistro = DateTime.Now.AddHours(-5),
                        Agua = request.Agua,
                        Ciiu = request.Ciiu,
                        CorreoContacto = request.CorreoContacto,
                        DieselEquipo = request.DieselEquipo,
                        DieselVehiculo = request.DieselVehiculo,
                        Electricidad = request.Electricidad,
                        GasolinaEquipo = request.GasolinaEquipo,
                        GasolinaVehiculo =  request?.GasolinaVehiculo,   
                        GlpEquipo = request.GLPEquipo,
                        GlpVehiculo= request?.GLPVehiculo,
                        GnvEquipo   =request.GNVEquipo,
                        GnvVehiculo= request.GNVEquipo,
                        HojasA3 = request.HojasA3,
                        HojasA4 = request.HojasA4,
                        IdCliente = request.IdCliente,
                        Metal=request.Metal,    
                        NombreContacto= request.NombreContacto, 
                        PapelCarton= request.PapelCarton,
                        Plasticos= request.Plasticos,
                        PuestoContacto= request.PuestoContacto, 
                        Raee= request.Raee,
                        Residuos= request.Residuos,
                        TelefonoContacto = request.TelefonoContacto,
                        
                    };

                    _context.HUELLACARBONO_SOLICITUDCLIENTE.Add(New_Registro);
                    var retorno = await _context.SaveChangesAsync();


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

            }
        }

        public class Modifica
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public int IdCliente { get; set; }
                public string? Ciiu { get; set; }
                public string? NombreContacto { get; set; }
                public string? PuestoContacto { get; set; }
                public string? CorreoContacto { get; set; }
                public string? TelefonoContacto { get; set; }
                public decimal? DieselEquipo { get; set; }
                public decimal? GasolinaEquipo { get; set; }
                public decimal? GLPEquipo { get; set; }
                public decimal? GNVEquipo { get; set; }
                public decimal? DieselVehiculo { get; set; }
                public decimal? GasolinaVehiculo { get; set; }
                public decimal? GLPVehiculo { get; set; }
                public decimal? GNVVehiculo { get; set; }
                public decimal? Electricidad { get; set; }
                public decimal? Agua { get; set; }
                public decimal? HojasA4 { get; set; }
                public decimal? HojasA3 { get; set; }
                public decimal? Plasticos { get; set; }
                public decimal? PapelCarton { get; set; }
                public decimal? Residuos { get; set; }
                public decimal? Metal { get; set; }
                public decimal? Raee { get; set; }

            }

            public class EjecutaValidacion : AbstractValidator<Ejecuta>
            {
                public EjecutaValidacion()
                {
                    RuleFor(x => x.IdCliente).NotEmpty();
                }
            }

            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly Huella_Context _context;
                public Manejador(Huella_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(Ejecuta request, CancellationToken cancellation)
                {
                    var registro = await _context.HUELLACARBONO_SOLICITUDCLIENTE.Where(x=> x.IdCliente== request.IdCliente).FirstAsync();
                    if (registro == null)
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { Error = "El registro no existe, valide" });
                    }

                    registro.Agua= request.Agua;
                    registro.Ciiu = request.Ciiu;
                    registro.CorreoContacto = request.CorreoContacto;
                    registro.DieselEquipo= request.DieselEquipo;
                    registro.DieselVehiculo = request.DieselVehiculo;
                    registro.Electricidad = request.Electricidad;                   
                    registro.GnvVehiculo = request.GNVVehiculo;
                    registro.GnvEquipo = request.GNVEquipo;
                    registro.GasolinaEquipo = request.GasolinaEquipo;
                    registro.GasolinaVehiculo = request.GasolinaVehiculo;
                    registro.GlpEquipo = request.GLPEquipo;
                    registro.GlpVehiculo= request.GLPVehiculo;
                    registro.HojasA3 = request.HojasA3;
                    registro.HojasA4 = request.HojasA4;
                    registro.Metal=request.Metal;
                    registro.NombreContacto = request.NombreContacto;
                    registro.PapelCarton= request.PapelCarton;
                    registro.Plasticos = request.Plasticos;
                    registro.PuestoContacto = request.PuestoContacto;
                    registro.Residuos= request.Residuos;
                    registro.Raee = request.Raee;
                    registro.TelefonoContacto = request.TelefonoContacto;
                    registro.FechaRegistro = DateTime.Now.AddHours(-5);

                    var retorno = await _context.SaveChangesAsync();

                    if (retorno == 0)
                    {
                        return new ResponseTransacciones
                        {
                            Message = "Solicitud Incorrecta",
                            IsSuccess = false,
                            IdValue=0
                        };
                      
                    }

                    return new ResponseTransacciones
                    {
                        Message = "Solicitud Correcta",
                        IsSuccess = true,
                        IdValue = 1
                    };

                    
                }

            }
        }

        public class ActualizaEstadoUsuario
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public int IdCliente { get; set; }

                public int Flag { get; set; }
            }

            public class EjecutaValidacion : AbstractValidator<Ejecuta>
            {
                public EjecutaValidacion()
                {
                    RuleFor(x => x.IdCliente).NotEmpty();
                    RuleFor(x => x.Flag).NotEmpty();
                }
            }

            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly Huella_Context _context;
                public Manejador(Huella_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(Ejecuta request, CancellationToken cancellation)
                {
                    var registro = await _context.HUELLACARBONO_CLIENTE.FindAsync(request.IdCliente);
                    if (registro == null)
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { Error = "El registro no existe, valide" });
                    }

                    registro.FlagCompletado = request.Flag + 1;

                    var retorno = await _context.SaveChangesAsync();

                    if (retorno == 0)
                    {
                        return new ResponseTransacciones
                        {
                            Message = "Solicitud Incorrecta",
                            IsSuccess = false,
                            IdValue = 0
                        };

                    }

                    var param = new SqlParameter("@IDCLIENTE", request.IdCliente);
                    await _context.Database.ExecuteSqlRawAsync("EXEC SP_HUELLACARBONO_RESULTADO @IDCLIENTE", param);


                    return new ResponseTransacciones
                    {
                        Message = "Solicitud Correcta",
                        IsSuccess = true,
                        IdValue = 1
                    };
                }

            }
        }

        public class AgregaResultados
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public int IdCliente { get; set; }
            }

            public class EjecutaValidacion : AbstractValidator<Ejecuta>
            {
                public EjecutaValidacion()
                {
                    RuleFor(x => x.IdCliente).NotEmpty();
                }
            }

            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly Huella_Context _context;
                public Manejador(Huella_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(Ejecuta request, CancellationToken cancellation)
                {
                    var idClienteParam = new SqlParameter("@IDCLIENTE", request.IdCliente);

                    var resultado = await _context.HUELLACARBONO_RESULTADO
                        .FromSqlRaw("EXEC SP_HUELLACARBONO_RESULTADO @IDCLIENTE", idClienteParam)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cancellation);

                   
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
