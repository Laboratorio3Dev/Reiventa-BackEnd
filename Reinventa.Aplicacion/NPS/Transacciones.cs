using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X500;
using Reinventa.Aplicacion.ManejadorErrores;
using Reinventa.Dominio.HuellaCarbono;
using Reinventa.Dominio.NPS;
using Reinventa.Persistencia.HuellaCarbono;
using Reinventa.Persistencia.NPS;
using Reinventa.Utilitarios;
using Reinventa.Utilitarios.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.NPS
{
    public class Transacciones
    {
        public class CargarClientesEncuesta
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public int IdEncuesta { get; set; }
                public string Usuario { get; set; }
                public List<NuevoCLienteEncuestaDTO> NPS_ClienteEncuesta { get; set; }
            }

            public class EjecutaValidacion : AbstractValidator<Ejecuta>
            {
                public EjecutaValidacion()
                {
                    RuleForEach(x => x.NPS_ClienteEncuesta)
                    .ChildRules(cliente =>
                    {
                       // cliente.RuleFor(c => c.IdEncuesta).NotEmpty();
                        cliente.RuleFor(c => c.NroDocumento).NotEmpty();
                        cliente.RuleFor(c => c.Celular).NotEmpty();
                    });

                }
            }

            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly NPS_Context _context;
                public Manejador(NPS_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                        _context.NPS_ClienteEncuesta.AddRange(request.NPS_ClienteEncuesta.Select(cliente =>
                            new NPS_ClienteEncuesta
                            {
                                Celular = cliente.Celular,
                                CodigoIBS = cliente.CodigoIBS,
                                Correo = cliente.Correo,
                                FlagContesta = 0,
                                LinkPersonalizado = CryptoHelper.Encrypt($"{cliente.NroDocumento}|{request.IdEncuesta}"),
                                IdEncuesta = request.IdEncuesta,
                                Nombre = cliente.Nombre,
                                NroDocumento = cliente.NroDocumento,
                                UsuarioCreacion= request.Usuario,
                                FechaCreacion= DateTime.Now.AddHours(-5),
                            }
                        ));

                    var retorno = await _context.SaveChangesAsync();

                    if (retorno == 0)
                    {
                        return new ResponseTransacciones
                        {
                            Message = "Solicitud Incorrecta",
                            IsSuccess = false,
                            IdValue = retorno
                        };
                    }

                    return new ResponseTransacciones
                    {
                        Message = "Solicitud Correcta",
                        IsSuccess = true,
                        IdValue = retorno
                    };
                }
            }
        }

        public class CrearEncuesta
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public string Usuario { get; set; }
                public NuevaEncuestaDTO DatosEncuesta { get; set; }
                public List<NuevaEncuestaPreguntaDTO> EncuestaPreguntas { get; set; }
                public List<NuevoCLienteEncuestaDTO> NPS_ClienteEncuesta { get; set; }
            }

            public class EjecutaValidacion : AbstractValidator<Ejecuta>
            {
                public EjecutaValidacion()
                {
                    RuleFor(x => x.Usuario).NotEmpty();
                    RuleFor(x => x.DatosEncuesta.NombreEncuesta).NotEmpty();
                    RuleFor(x => x.DatosEncuesta.FechaInicio).NotEmpty();
                    RuleFor(x => x.DatosEncuesta.FechaFin).NotEmpty();
                    RuleFor(x => x.DatosEncuesta.TipoPersona).NotEmpty();
                    RuleFor(x => x.DatosEncuesta.TituloEncuesta).NotEmpty();
                }
            }

            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly NPS_Context _context;
                public Manejador(NPS_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    var New_Registro = new NPS_Encuesta
                    {
                        FechaCreacion = DateTime.Now.AddHours(-5),
                        FechaFin = request.DatosEncuesta.FechaFin,
                        FechaInicio= request.DatosEncuesta.FechaInicio,
                        FlagAnalisis= true,
                        FlagBase= request.DatosEncuesta.FlagBase,
                        FlagLogin= request.DatosEncuesta.FlagLogin,
                        ImagenLogin= request.DatosEncuesta.ImagenLogin,
                        Link = request.DatosEncuesta.Link,
                        NombreEncuesta = request.DatosEncuesta.NombreEncuesta,
                        TipoPersona= request.DatosEncuesta.TipoPersona,
                        UsuarioCreacion= request.Usuario,     
                        TituloEncuesta= request.DatosEncuesta.TituloEncuesta
                    };

                    // 2. Agregar al contexto
                    _context.NPS_Encuesta.Add(New_Registro);
                    await _context.SaveChangesAsync(cancellationToken); 

                    foreach (var pregunta in request.EncuestaPreguntas)
                    {
                        var nuevaPregunta = new NPS_EncuestaPregunta
                        {
                            IdEncuesta = New_Registro.IdEncuesta,
                            Orden = pregunta.Orden,
                            FlagComentario = Convert.ToInt16(pregunta.FlagComentario),
                            Comentario= pregunta.Comentario,
                            RangoMaximo= pregunta.RangoMaximo,
                            RangoMinimo= pregunta.RangoMinimo,
                            TextoDetractor= pregunta.TextoDetractor,
                            TextoNeutro= pregunta.TextoNeutro,
                            TextoPromotor= pregunta.TextoPromotor,
                            ValoresX= pregunta.ValoresX,
                            ValoresY= pregunta.ValoresY,
                            TipoPregunta = pregunta.TipoPregunta,
                            FechaCreacion = DateTime.Now.AddHours(-5),
                            UsuarioCreacion = request.Usuario,
                            Pregunta= pregunta.Pregunta,
                            TextoValorMaximo = pregunta.TextoValorMaximo,
                            TextoValorMinimo = pregunta.TextoValorMinimo,
                            Estado= 1
                        };
                        _context.NPS_EncuestaPregunta.Add(nuevaPregunta);
                    }                                      
                                   

                    _context.NPS_ClienteEncuesta.AddRange(request.NPS_ClienteEncuesta.Select(cliente =>
                            new NPS_ClienteEncuesta
                            {
                                Celular = cliente.Celular,
                                CodigoIBS = cliente.CodigoIBS,
                                Correo = cliente.Correo,
                                FlagContesta = 0,
                                LinkPersonalizado = CryptoHelper.Encrypt($"{cliente.NroDocumento}|{New_Registro.IdEncuesta}"),
                                IdEncuesta = New_Registro.IdEncuesta,
                                Nombre = cliente.Nombre,
                                NroDocumento = cliente.NroDocumento,
                                UsuarioCreacion = request.Usuario,
                                FechaCreacion = DateTime.Now.AddHours(-5),
                            }
                        ));

                    var retorno = await _context.SaveChangesAsync(cancellationToken);

                    if (retorno == 0)
                    {
                        return new ResponseTransacciones
                        {
                            Message = "Solicitud Incorrecta",
                            IsSuccess = false,
                            IdValue = retorno
                        };
                    }

                    return new ResponseTransacciones
                    {
                        Message = "Solicitud Correcta",
                        IsSuccess = true,
                        IdValue = New_Registro.IdEncuesta
                    };
                }
            }
        }

        public class ActualizaEncuesta
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public string Usuario { get; set; }
                public List<PreguntasEliminar> preguntasEliminadas { get; set; }
                public ActualizaEncuestaDTO DatosEncuesta { get; set; }
                public List<ActualizaEncuestaPreguntaDTO> EncuestaPreguntas { get; set; }
                public List<NuevoCLienteEncuestaDTO> NPS_ClienteEncuesta { get; set; }

            }

            public class EjecutaValidacion : AbstractValidator<Ejecuta>
            {
                public EjecutaValidacion()
                {
                    RuleFor(x => x.Usuario).NotEmpty();
                    RuleFor(x => x.DatosEncuesta.FechaFin).NotEmpty();
                    RuleFor(x => x.DatosEncuesta.TituloEncuesta).NotEmpty();
                }
            }

            public class Manejador : IRequestHandler<Ejecuta, ResponseTransacciones>
            {
                private readonly NPS_Context _context;
                public Manejador(NPS_Context context)
                {
                    _context = context;
                }

                public async Task<ResponseTransacciones> Handle(Ejecuta request, CancellationToken cancellationToken)
                {                
                    var registro = await _context.NPS_Encuesta.Where(x => x.IdEncuesta == request.DatosEncuesta.IdEncuesta).FirstAsync();
                    if (registro == null)
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { Error = "El registro no existe, valide" });
                    }

                    registro.FechaFin = request.DatosEncuesta.FechaFin;
                    registro.FlagBase= request.DatosEncuesta.FlagBase;
                    registro.FlagLogin = request.DatosEncuesta.FlagLogin;
                    registro.ImagenLogin = request.DatosEncuesta.ImagenLogin;
                    registro.TituloEncuesta = request.DatosEncuesta.TituloEncuesta;

                    var retorno = await _context.SaveChangesAsync(cancellationToken);
                    retorno = 1;

                    foreach (var ElimimarPregunta in request.preguntasEliminadas)
                    {
                        var registroPreg = await _context.NPS_EncuestaPregunta.Where(x => x.IdEncuestaPregunta == ElimimarPregunta.IdPregunta).FirstAsync();
                        if (registroPreg == null)
                        {
                            throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { Error = "El registro no existe, valide" });
                        }

                        registroPreg.Estado = 0;

                        await _context.SaveChangesAsync(cancellationToken);
                        retorno = 1;
                    }

                    foreach (var pregunta in request.EncuestaPreguntas)
                    {
                        if(pregunta.IdEncuestaPregunta==0)
                        {
                            var nuevaPregunta = new NPS_EncuestaPregunta
                            {
                                IdEncuesta= request.DatosEncuesta.IdEncuesta,
                                Orden = pregunta.Orden,
                                FlagComentario = Convert.ToInt16(pregunta.FlagComentario),
                                Comentario = pregunta.Comentario,
                                RangoMaximo = pregunta.RangoMaximo,
                                RangoMinimo = pregunta.RangoMinimo,
                                TextoDetractor = pregunta.TextoDetractor,
                                TextoNeutro = pregunta.TextoNeutro,
                                TextoPromotor = pregunta.TextoPromotor,
                                ValoresX = pregunta.ValoresX,
                                ValoresY = pregunta.ValoresY,
                                TipoPregunta = pregunta.TipoPregunta,
                                FechaCreacion = DateTime.Now.AddHours(-5),
                                UsuarioCreacion = request.Usuario,
                                Pregunta = pregunta.Pregunta,
                                TextoValorMaximo = pregunta.TextoValorMaximo,
                                TextoValorMinimo = pregunta.TextoValorMinimo,
                                Estado=1
                            };
                            _context.NPS_EncuestaPregunta.Add(nuevaPregunta);
                            await _context.SaveChangesAsync(cancellationToken);
                            retorno = 1;
                        }
                        else
                        {
                            var registroPreg = await _context.NPS_EncuestaPregunta.Where(x => x.IdEncuestaPregunta == pregunta.IdEncuestaPregunta).FirstAsync();
                            if (registroPreg == null)
                            {
                                throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { Error = "El registro no existe, valide" });
                            }

                            registroPreg.RangoMinimo = pregunta.RangoMinimo;
                            registroPreg.RangoMaximo = pregunta.RangoMaximo;    
                            registroPreg.TextoValorMaximo= pregunta.TextoValorMaximo;
                            registroPreg.Comentario = pregunta.Comentario;  
                            registroPreg.Orden= pregunta.Orden;
                            registroPreg.Pregunta = pregunta.Pregunta;  
                            registroPreg.TextoDetractor= pregunta.TextoDetractor;
                            registroPreg.TextoNeutro=pregunta.TextoNeutro;
                            registroPreg.TextoPromotor = pregunta.TextoPromotor;
                            registroPreg.ValoresX= pregunta.ValoresX;
                            registroPreg.ValoresY = pregunta.ValoresY;

                            await _context.SaveChangesAsync(cancellationToken);
                            retorno = 1;
                        }
                    }

                    if(request.NPS_ClienteEncuesta.Count>0)
                    {
                        _context.NPS_ClienteEncuesta.AddRange(request.NPS_ClienteEncuesta.Select(cliente =>
                         new NPS_ClienteEncuesta
                         {
                             Celular = cliente.Celular,
                             CodigoIBS = cliente.CodigoIBS,
                             Correo = cliente.Correo,
                             FlagContesta = 0,
                             LinkPersonalizado = CryptoHelper.Encrypt($"{cliente.NroDocumento}|{request.DatosEncuesta.IdEncuesta}"),
                             IdEncuesta = (int)request.DatosEncuesta.IdEncuesta,
                             Nombre = cliente.Nombre,
                             NroDocumento = cliente.NroDocumento,
                             UsuarioCreacion = request.Usuario,
                             FechaCreacion = DateTime.Now.AddHours(-5),
                         }
                     ));

                        await _context.SaveChangesAsync(cancellationToken);
                        retorno = 1;
                    }
                   

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
    }
}
