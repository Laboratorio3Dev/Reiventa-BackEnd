using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
                // 👇 AQUÍ VA
                private readonly FrontendSettings _front;

                public Manejador(
                    NPS_Context context,
                    IOptions<FrontendSettings> frontOptions)
                {
                    _context = context;
                    _front = frontOptions.Value;
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
                                LinkPersonalizado =CryptoHelper.Encrypt($"{cliente.NroDocumento}|{New_Registro.IdEncuesta}"),
                                IdEncuesta = New_Registro.IdEncuesta,
                                Nombre = cliente.Nombre,
                                NroDocumento = cliente.NroDocumento,
                                UsuarioCreacion = request.Usuario,
                                FechaCreacion = DateTime.Now.AddHours(-5),
                            }
                        ));

                    var token = CryptoHelper.Encrypt($"{New_Registro.IdEncuesta}|{New_Registro.TipoPersona}");
                    New_Registro.Link = $"{_front.BaseUrl.TrimEnd('/')}{_front.EncuestaPath}?encuesta={Uri.EscapeDataString(token)}";
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
                    registro.FechaInicio = request.DatosEncuesta.FechaInicio;
                    registro.NombreEncuesta = request.DatosEncuesta.NombreEncuesta;
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
        public class Respuestas
        {
            public static class GuardarRespuestas
            {
                public class Command : IRequest<bool>
                {
                    public GuardarRespuestasRequest Request { get; set; } = new();
                }
            }

            public class GuardarRespuestasCliente
                : IRequestHandler<GuardarRespuestas.Command, bool>
            {
                private readonly NPS_Context _context;

                public GuardarRespuestasCliente(NPS_Context context)
                {
                    _context = context;
                }

                public async Task<bool> Handle(GuardarRespuestas.Command cmd, CancellationToken cancellationToken)
                {
                    var req = cmd.Request;

                    if (req is null ||
                        string.IsNullOrWhiteSpace(req.EncuestaToken) ||
                        string.IsNullOrWhiteSpace(req.UsuarioToken) ||
                        req.Detalles is null || req.Detalles.Count == 0)
                        return false;

                    int idEncuesta;
                    try
                    {
                        var desencriptado = CryptoHelper.Decrypt(req.EncuestaToken).Trim();
                        var partes = desencriptado.Split('|', StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(x => x.Trim())
                                                  .ToList();
                        idEncuesta = int.Parse(partes[0]);
                    }
                    catch
                    {
                        return false;
                    }

                    var hoy = DateTime.Now;
                    //Se valida si la encuesta es valida para poder guardar la respuesta
                    var encuestaValida = await _context.NPS_Encuesta
                        .AnyAsync(e =>
                            e.IdEncuesta == idEncuesta &&
                            (e.FechaInicio == null || e.FechaInicio <= hoy) &&
                            (e.FechaFin == null || e.FechaFin >= hoy),
                            cancellationToken);

                    if (!encuestaValida) return false;
                    //Se carga el cliente mediante el link personalizado del mismo, además se valida si es elegible para responder
                    var cliente = await _context.NPS_ClienteEncuesta
                        .Where(c => c.IdEncuesta == idEncuesta && c.LinkPersonalizado == req.UsuarioToken)
                        .Select(c => new { c.IdCliente, c.FlagContesta,c.NroDocumento })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (cliente == null) return false;
                    if (cliente.FlagContesta == 1) return false;

                    var idsPreguntas = req.Detalles.Select(d => d.IdPregunta).Distinct().ToList();
                    //Se valida si las preguntas escritas por el cliente tienen el id correcto
                    var preguntasValidas = await _context.NPS_EncuestaPregunta
                        .Where(p => p.IdEncuesta == idEncuesta && idsPreguntas.Contains(p.IdEncuestaPregunta))
                        .Select(p => p.IdEncuestaPregunta)
                        .ToListAsync(cancellationToken);

                    if (preguntasValidas.Count != idsPreguntas.Count)
                        return false;

                    await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        var respuesta = new NPS_Respuesta
                        {
                            IdEncuesta = idEncuesta,
                            Fecha = hoy,

                            NroDocumentoCliente = cliente.NroDocumento,

                            CodigoLogAsociado = req.UsuarioToken,

                            LinkInicio = req.EncuestaToken
                        };

                        _context.NPS_Respuesta.Add(respuesta);
                        await _context.SaveChangesAsync(cancellationToken);

                        var idRespuesta = respuesta.IdRespuesta;
                        foreach (var d in req.Detalles)
                        {
                            var det = new NPS_DetalleRespuesta
                            {
                                IdRespuesta = idRespuesta,
                                IdPregunta = d.IdPregunta,
                                ValorRespuesta = d.ValorRespuesta,
                                ValorComentario = d.ValorComentario,
                            };

                            _context.NPS_DetalleRespuesta.Add(det);
                        }

                        // Se modifica el FlagContesta a 1 para registrar que el cliente guardó sus respuestas
                        var entityCliente = await _context.NPS_ClienteEncuesta
                            .FirstAsync(x => x.IdCliente == cliente.IdCliente, cancellationToken);

                        entityCliente.FlagContesta = 1;
                        entityCliente.FechaCreacion = hoy; // si existe

                        await _context.SaveChangesAsync(cancellationToken);
                        await tx.CommitAsync(cancellationToken);

                        return true;
                    }
                    catch
                    {
                        await tx.RollbackAsync(cancellationToken);
                        return false;
                    }
                }
            }
        }
    }
}
