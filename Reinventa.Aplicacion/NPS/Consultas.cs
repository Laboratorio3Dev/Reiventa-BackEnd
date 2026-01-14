using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Dominio.NPS;
using Reinventa.Persistencia.NPS;
using Reinventa.Utilitarios;
using Reinventa.Utilitarios.DTOS;
using static Reinventa.Aplicacion.NPS.Consultas.DNICliente.ValidarClienteEnEncuesta;


namespace Reinventa.Aplicacion.NPS
{
    public class Consultas
    {
        public class Encuestas
        {
            public class ListaEncuesta : IRequest<List<EncuestaDto>>
            {
                
            }

            public class Manejador : IRequestHandler<ListaEncuesta, List<EncuestaDto>>
            {
                private readonly NPS_Context _context;
                public Manejador(NPS_Context context)
                {
                    _context = context;
                }


                public async Task<List<EncuestaDto>> Handle(ListaEncuesta request, CancellationToken cancellationToken)
                {
                    var hoy = DateTime.Now;

                    var data = await _context.NPS_Encuesta.Where(e=>e.Estado==1)
                        .Select(e => new EncuestaDto
                        {
                            IdEncuesta = e.IdEncuesta,
                            NombreEncuesta = e.NombreEncuesta,
                            FechaInicio = e.FechaInicio,
                            FechaFin = e.FechaFin,
                            TipoPersona = e.TipoPersona,
                            FlagLogin = e.FlagLogin,
                            FlagBase = e.FlagBase,
                            Link=e.Link,
                            ImagenLogin = e.ImagenLogin,
                            TituloEncuesta = e.TituloEncuesta,
                            TipoEncuesta = e.FlagLogin && e.FlagBase
                                ? "Login y Base"
                                : e.FlagLogin
                                    ? "Login"
                                    : e.FlagBase
                                        ? "Base"
                                        : string.Empty,

                            Estado = e.FechaFin != null && e.FechaFin < hoy
                                ? "Cerrada"
                                : "En curso",

                            CantidadRespuestas = e.Respuestas.Count(),

                            EncuestaPreguntas = e.EncuestaPreguntas.Where(x=> x.Estado==1).Select(p => new PreguntaDto
                            {
                                IdEncuestaPregunta = p.IdEncuestaPregunta,
                                IdEncuesta = p.IdEncuesta,
                                TipoPregunta = p.TipoPregunta,
                                Pregunta = p.Pregunta,
                                Orden = p.Orden,
                                RangoMinimo = p.RangoMinimo,
                                RangoMaximo = p.RangoMaximo,
                                FlagComentario = Convert.ToBoolean(p.FlagComentario),
                                Comentario = p.Comentario,
                                TextoDetractor = p.TextoDetractor,
                                TextoNeutro = p.TextoNeutro,
                                TextoPromotor = p.TextoPromotor,
                                ValoresX = p.ValoresX,
                                ValoresY = p.ValoresY,
                                TextoValorMinimo = p.TextoValorMinimo,
                                TextoValorMaximo = p.TextoValorMaximo
                            }).ToList(),

                            //ClientesEncuesta = e.ClientesEncuesta.Select(c => new ClienteEncuestaDto
                            //{
                            //    IdCliente = c.IdCliente,
                            //    IdEncuesta = c.IdEncuesta,
                            //    NroDocumento = c.NroDocumento,
                            //    CodigoIBS = c.CodigoIBS,
                            //    Nombre = c.Nombre,
                            //    Correo = c.Correo,
                            //    Celular = c.Celular,
                            //    FlagContesta = c.FlagContesta,
                            //    LinkPersonalizado = c.LinkPersonalizado
                            //}).ToList(),

                            //Respuestas = e.Respuestas.Select(r => new RespuestaDto
                            //{
                            //    CodigoLogAsociado = r.CodigoLogAsociado,
                            //    NroDocumentoCliente = r.NroDocumentoCliente,
                            //    Fecha = r.Fecha,
                            //    IdEncuesta = r.IdEncuesta,
                            //    IdRespuesta = r.IdRespuesta,
                            //    LinkInicio = r.LinkInicio,

                            //    DetallesRespuesta = r.DetallesRespuesta.Select(d => new DetalleRespuestaDto
                            //    {
                            //        Abandono = d.Abandono,
                            //        CategoriaComentario = d.CategoriaComentario,
                            //        Emocion = d.Emocion,
                            //        IdDetalleRespuesta = d.IdDetalleRespuesta,
                            //        IdPregunta = d.IdPregunta,
                            //        PalabraClave = d.PalabraClave,
                            //        TipoComentario = d.TipoComentario,
                            //        ValorComentario = d.ValorComentario,
                            //        ValorRespuesta = d.ValorRespuesta,
                            //        IdRespuesta = d.IdRespuesta
                            //    }).ToList()
                            //}).ToList()
                        })
                        .ToListAsync(cancellationToken);

                    return data;
                }

            }
        }
        public class EncuestaId
        {
            public class EncuestaUnico : IRequest<EncuestaDto>
            {
                public int Id { get; set; }
            }
            public class EncuestaValorEncriptado : IRequest<EncuestaDto>
            {
                public string ValorEncriptado;
            }
            public class Manejador : IRequestHandler<EncuestaUnico, EncuestaDto>, IRequestHandler<EncuestaValorEncriptado, EncuestaDto>
            {
                private readonly NPS_Context _context;
                public Manejador(NPS_Context context)
                {
                    _context = context;
                }

                public async Task<EncuestaDto?> Handle(EncuestaUnico request, CancellationToken cancellationToken)
                {
                    var hoy = DateTime.Now;

                    var encuesta = await _context.NPS_Encuesta
                        .Where(e => e.IdEncuesta == request.Id && e.Estado==1) 
                        .Select(e => new EncuestaDto
                        {
                            IdEncuesta = e.IdEncuesta,
                            NombreEncuesta = e.NombreEncuesta,
                            FechaInicio = e.FechaInicio,
                            FechaFin = e.FechaFin,
                            TipoPersona = e.TipoPersona,
                            FlagLogin = e.FlagLogin,
                            FlagBase = e.FlagBase,
                            TituloEncuesta = e.TituloEncuesta,
                            TipoEncuesta = e.FlagLogin && e.FlagBase
                                ? "Login y Base"
                                : e.FlagLogin
                                    ? "Login"
                                    : e.FlagBase
                                        ? "Base"
                                        : string.Empty,

                            Estado = e.FechaFin != null && e.FechaFin < hoy
                                ? "Cerrada"
                                : "En curso",

                            CantidadRespuestas = e.Respuestas.Count(),

                            EncuestaPreguntas = e.EncuestaPreguntas.Where(x => x.Estado == 1).Select(p => new PreguntaDto
                            {
                                IdEncuestaPregunta = p.IdEncuestaPregunta,
                                IdEncuesta = p.IdEncuesta,
                                TipoPregunta = p.TipoPregunta,
                                Pregunta = p.Pregunta,
                                Orden = p.Orden,
                                RangoMinimo = p.RangoMinimo,
                                RangoMaximo = p.RangoMaximo,
                                FlagComentario = Convert.ToBoolean(p.FlagComentario),
                                Comentario = p.Comentario,
                                TextoDetractor = p.TextoDetractor,
                                TextoNeutro = p.TextoNeutro,
                                TextoPromotor = p.TextoPromotor,
                                ValoresX = p.ValoresX,
                                ValoresY = p.ValoresY,
                                TextoValorMinimo = p.TextoValorMinimo,
                                TextoValorMaximo = p.TextoValorMaximo
                            }).ToList(),

                            //ClientesEncuesta = e.ClientesEncuesta.Select(c => new ClienteEncuestaDto
                            //{
                            //    IdCliente = c.IdCliente,
                            //    IdEncuesta = c.IdEncuesta,
                            //    NroDocumento = c.NroDocumento,
                            //    CodigoIBS = c.CodigoIBS,
                            //    Nombre = c.Nombre,
                            //    Correo = c.Correo,
                            //    Celular = c.Celular,
                            //    FlagContesta = c.FlagContesta,
                            //    LinkPersonalizado = c.LinkPersonalizado
                            //}).ToList(),

                            //Respuestas = e.Respuestas.Select(r => new RespuestaDto
                            //{
                            //    CodigoLogAsociado = r.CodigoLogAsociado,
                            //    NroDocumentoCliente = r.NroDocumentoCliente,
                            //    Fecha = r.Fecha,
                            //    IdEncuesta = r.IdEncuesta,
                            //    IdRespuesta = r.IdRespuesta,
                            //    LinkInicio = r.LinkInicio,

                            //    DetallesRespuesta = r.DetallesRespuesta.Select(d => new DetalleRespuestaDto
                            //    {
                            //        Abandono = d.Abandono,
                            //        CategoriaComentario = d.CategoriaComentario,
                            //        Emocion = d.Emocion,
                            //        IdDetalleRespuesta = d.IdDetalleRespuesta,
                            //        IdPregunta = d.IdPregunta,
                            //        PalabraClave = d.PalabraClave,
                            //        TipoComentario = d.TipoComentario,
                            //        ValorComentario = d.ValorComentario,
                            //        ValorRespuesta = d.ValorRespuesta,
                            //        IdRespuesta = d.IdRespuesta
                            //    }).ToList()
                            //}).ToList()
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    return encuesta;
                }
                public async Task<EncuestaDto?> Handle(EncuestaValorEncriptado request, CancellationToken cancellationToken)
                {
                    int idEncuesta = 0;
                    string tipoPersona = "";
                    try
                    {
                        string ValorDesencriptado = Utilitarios.CryptoHelper.Decrypt(request.ValorEncriptado).Trim();
                        List<string> ValoresSeparados = ValorDesencriptado.Split("|").ToList();
                        idEncuesta = Convert.ToInt32(ValoresSeparados.First().Replace(" ", ""));
                        tipoPersona = ValoresSeparados.Last();
                    }
                    catch
                    {
                        Console.WriteLine("Error al desencriptar la URL");
                        return null;
                    }
                    var hoy = DateTime.Now;
                    var encuesta = await _context.NPS_Encuesta
                        .Where(e =>
                                e.IdEncuesta == idEncuesta &&
                                (e.FechaInicio == null || e.FechaInicio <= hoy) &&
                                (e.FechaFin == null || e.FechaFin >= hoy) && e.Estado == 1
                               )
                        .Select(e => new EncuestaDto
                        {
                            IdEncuesta = e.IdEncuesta,
                            NombreEncuesta = e.NombreEncuesta,
                            FechaInicio = e.FechaInicio,
                            FechaFin = e.FechaFin,
                            TipoPersona = e.TipoPersona,
                            FlagLogin = e.FlagLogin,
                            ImagenLogin= e.ImagenLogin,
                            FlagBase = e.FlagBase,
                            TituloEncuesta = e.TituloEncuesta,
                            TipoEncuesta = e.FlagLogin && e.FlagBase
                                ? "Login y Base"
                                : e.FlagLogin
                                    ? "Login"
                                    : e.FlagBase
                                        ? "Base"
                                        : string.Empty,

                            Estado = e.FechaFin != null && e.FechaFin < hoy
                                ? "Cerrada"
                                : "En curso",

                            CantidadRespuestas = e.Respuestas.Count(),

                            EncuestaPreguntas = e.EncuestaPreguntas.Where(x => x.Estado == 1).Select(p => new PreguntaDto
                            {
                                IdEncuestaPregunta = p.IdEncuestaPregunta,
                                IdEncuesta = p.IdEncuesta,
                                TipoPregunta = p.TipoPregunta,
                                Pregunta = p.Pregunta,
                                Orden = p.Orden,
                                RangoMinimo = p.RangoMinimo,
                                RangoMaximo = p.RangoMaximo,
                                FlagComentario = Convert.ToBoolean(p.FlagComentario),
                                Comentario = p.Comentario,
                                TextoDetractor = p.TextoDetractor,
                                TextoNeutro = p.TextoNeutro,
                                TextoPromotor = p.TextoPromotor,
                                ValoresX = p.ValoresX,
                                ValoresY = p.ValoresY,
                                TextoValorMinimo = p.TextoValorMinimo,
                                TextoValorMaximo = p.TextoValorMaximo
                            }).ToList(),
                        })
                        .FirstOrDefaultAsync(cancellationToken);
                    var encuestaValida = await _context.NPS_Encuesta
                        .AnyAsync(e =>
                            e.IdEncuesta == idEncuesta &&
                            (e.FechaInicio == null || e.FechaInicio <= hoy) &&
                            (e.FechaFin == null || e.FechaFin >= hoy),
                            cancellationToken);
                    return encuesta;
                }
            }


        }
        public class DNICliente()
        {
            public static class ValidarClienteEnEncuesta
            {
                public class Result
                {
                    public bool Existe { get; set; }
                    public int? FlagContesta { get; set; }
                    public bool YaRespondio => FlagContesta == 1;
                }

                public class Query : IRequest<Result>
                {
                    public string LinkPersonalizado { get; set; } = "";
                    public string ValorEncriptado { get; set; } = "";
                }

                public class Handler : IRequestHandler<Query, Result>
                {
                    private readonly NPS_Context _context;

                    public Handler(NPS_Context context)
                    {
                        _context = context;
                    }

                    public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
                    {
                        // Respuesta por defecto (no existe / inválido)
                        var result = new Result { Existe = false, FlagContesta = null };

                        if (string.IsNullOrWhiteSpace(request.LinkPersonalizado) ||
                            string.IsNullOrWhiteSpace(request.ValorEncriptado))
                            return result;

                        int idEncuesta;

                        try
                        {
                            var valorDesencriptado = Utilitarios.CryptoHelper.Decrypt(request.ValorEncriptado).Trim();
                            var valoresSeparados = valorDesencriptado.Split("|", StringSplitOptions.RemoveEmptyEntries)
                                                                     .Select(x => x.Trim())
                                                                     .ToList();

                            if (valoresSeparados.Count == 0) return result;

                            idEncuesta = Convert.ToInt32(valoresSeparados.First().Replace(" ", ""));
                        }
                        catch
                        {
                            return result;
                        }

                        // validar encuesta vigente/activa
                        var hoy = DateTime.Now;
                        var encuestaVigente = await _context.NPS_Encuesta
                            .AnyAsync(e =>
                                e.IdEncuesta == idEncuesta &&
                                (e.FechaInicio == null || e.FechaInicio <= hoy) &&
                                (e.FechaFin == null || e.FechaFin >= hoy),
                                cancellationToken);

                        if (!encuestaVigente) return result;

                        // Buscar cliente en la base de esa encuesta y traer FlagContesta
                        var cliente = await _context.NPS_ClienteEncuesta
                            .Where(c => c.IdEncuesta == idEncuesta &&
                                        c.LinkPersonalizado == request.LinkPersonalizado)
                            .Select(c => new { c.FlagContesta })
                            .FirstOrDefaultAsync(cancellationToken);

                        if (cliente == null) return result;

                        result.Existe = true;
                        result.FlagContesta = cliente.FlagContesta;
                        return result;
                    }
                }
            }
            public class URLCliente : IRequest<string>
            {
                public string DNICliente { get; set; }
                public string ValorEncriptado;
            }
            public class Manejador : IRequestHandler<URLCliente, string>
            {
                private readonly NPS_Context _context;
                public Manejador(NPS_Context context)
                {
                    _context = context;
                }
                public async Task<string> Handle(URLCliente Dni, CancellationToken cancellationToken)
                {
                    int idEncuesta = 0;
                    string tipoPersona = "";
                    try
                    {
                        string ValorDesencriptado = Utilitarios.CryptoHelper.Decrypt(Dni.ValorEncriptado).Trim();
                        List<string> ValoresSeparados = ValorDesencriptado.Split("|").ToList();
                        idEncuesta = Convert.ToInt32(ValoresSeparados.First().Replace(" ", ""));
                        tipoPersona = ValoresSeparados.Last();


                        var result = new Result { Existe = false, FlagContesta = null };
                        

                        // validar encuesta vigente/activa
                        var hoy = DateTime.Now;
                        var encuestaVigente = await _context.NPS_Encuesta
                            .AnyAsync(e =>
                                e.IdEncuesta == idEncuesta &&
                                (e.FechaInicio == null || e.FechaInicio <= hoy) &&
                                (e.FechaFin == null || e.FechaFin >= hoy),
                                cancellationToken);

                        var encuesta = new EncuestaDto();
                        if (encuestaVigente)
                        {
                            encuesta = await _context.NPS_Encuesta
                            .Where(e => e.IdEncuesta == idEncuesta)
                            .Select(e => new EncuestaDto
                            {
                                IdEncuesta = e.IdEncuesta,
                                NombreEncuesta = e.NombreEncuesta,
                                FechaInicio = e.FechaInicio,
                                FechaFin = e.FechaFin,
                                FlagLogin = e.FlagLogin,
                                FlagBase = e.FlagBase

                            }).FirstOrDefaultAsync();
                        }



                        if (!encuestaVigente) return null;

                        // Buscar cliente en la base de esa encuesta y traer FlagContesta
                        string? url = await _context.NPS_ClienteEncuesta.Where(e => e.NroDocumento == Dni.DNICliente && idEncuesta == e.IdEncuesta)
                                        .Select(e => e.LinkPersonalizado)
                                        .FirstOrDefaultAsync(cancellationToken);

                        if (url == null && (encuesta.FlagLogin))
                        {
                            var clienteGuardado = new NPS_ClienteEncuesta
                            {
                                IdEncuesta = idEncuesta,
                                NroDocumento = Dni.DNICliente,
                                Nombre="nombreGenerico",
                                FlagContesta = 0,
                                LinkPersonalizado = CryptoHelper.Encrypt($"{Dni.DNICliente}|{idEncuesta}"),
                                UsuarioCreacion = "Sistema",
                                FechaCreacion = DateTime.Now
                            };

                            _context.NPS_ClienteEncuesta.Add(clienteGuardado);
                            await _context.SaveChangesAsync();
                            url = clienteGuardado.LinkPersonalizado;
                        }

                        return $"{Uri.EscapeDataString(url)}";
                    }
                    catch
                    {
                        Console.WriteLine("Error al desencriptar la URL");
                        return "";
                    }

                }

                public async Task<string> Handle(string dni, CancellationToken cancellationToken)
                {
                    var hoy = DateTime.Now;

                    var data = await _context.NPS_Encuesta
                        .Select(e => new EncuestaDto
                        {
                            IdEncuesta = e.IdEncuesta,
                            NombreEncuesta = e.NombreEncuesta,
                            FechaInicio = e.FechaInicio,
                            FechaFin = e.FechaFin,
                            TipoPersona = e.TipoPersona,
                            FlagLogin = e.FlagLogin,
                            FlagBase = e.FlagBase,
                            ImagenLogin = e.ImagenLogin,
                            TituloEncuesta = e.TituloEncuesta,
                            TipoEncuesta = e.FlagLogin && e.FlagBase
                                ? "Login y Base"
                                : e.FlagLogin
                                    ? "Login"
                                    : e.FlagBase
                                        ? "Base"
                                        : string.Empty,

                            Estado = e.FechaFin != null && e.FechaFin < hoy
                                ? "Cerrada"
                                : "En curso",

                            CantidadRespuestas = e.Respuestas.Count(),

                            EncuestaPreguntas = e.EncuestaPreguntas.Where(x => x.Estado == 1).Select(p => new PreguntaDto
                            {
                                IdEncuestaPregunta = p.IdEncuestaPregunta,
                                IdEncuesta = p.IdEncuesta,
                                TipoPregunta = p.TipoPregunta,
                                Pregunta = p.Pregunta,
                                Orden = p.Orden,
                                RangoMinimo = p.RangoMinimo,
                                RangoMaximo = p.RangoMaximo,
                                FlagComentario = Convert.ToBoolean(p.FlagComentario),
                                Comentario = p.Comentario,
                                TextoDetractor = p.TextoDetractor,
                                TextoNeutro = p.TextoNeutro,
                                TextoPromotor = p.TextoPromotor,
                                ValoresX = p.ValoresX,
                                ValoresY = p.ValoresY,
                                TextoValorMinimo = p.TextoValorMinimo,
                                TextoValorMaximo = p.TextoValorMaximo
                            }).ToList(),
                        })
                        .ToListAsync(cancellationToken);

                    return "";
                }
            }
        }
        public class BaseClienteEncuesta
        {
            public class ListaBaseClientesEncuesta : IRequest<List<ClienteEncuestaDto>>
            {
                public int Id { get; set; }
            }

            public class Manejador : IRequestHandler<ListaBaseClientesEncuesta, List<ClienteEncuestaDto>>
            {
                private readonly NPS_Context _context;

                public Manejador(NPS_Context context)
                {
                    _context = context;
                }

                public async Task<List<ClienteEncuestaDto>> Handle(ListaBaseClientesEncuesta request, CancellationToken cancellationToken)
                {

                    var encuesta = _context.NPS_Encuesta.Where(x => x.IdEncuesta == request.Id).FirstOrDefault();
                    var encuestaLink = encuesta.Link;
                    var data = await _context.NPS_ClienteEncuesta
                        .Where(x => x.IdEncuesta == request.Id)
                        .Select(c => new ClienteEncuestaDto
                        {
                            IdCliente = c.IdCliente,
                            IdEncuesta = c.IdEncuesta,
                            NroDocumento = c.NroDocumento,
                            CodigoIBS = c.CodigoIBS,
                            Nombre = c.Nombre,
                            Correo = c.Correo,
                            Celular = c.Celular,
                            FlagContesta = c.FlagContesta,
                            LinkPersonalizado = string.Concat(encuestaLink, "&u=", $"{Uri.EscapeDataString(c.LinkPersonalizado)}")
                        })
                        .ToListAsync(cancellationToken);

                    return data;
                }
            }
        }
        public class ExportarRespuestasEncuesta
        {
            public class Query : IRequest<ExportRespuestasEncuestaDto>
            {
                public int Id { get; set; } // IdEncuesta
            }

            public class Manejador : IRequestHandler<Query, ExportRespuestasEncuestaDto>
            {
                private readonly NPS_Context _context;

                public Manejador(NPS_Context context)
                {
                    _context = context;
                }

                public async Task<ExportRespuestasEncuestaDto> Handle(Query request, CancellationToken cancellationToken)
                {
                    // 1) Traer preguntas 
                    var preguntas = await _context.NPS_EncuestaPregunta
                        .AsNoTracking()
                        .Where(p => p.IdEncuesta == request.Id)
                        .OrderBy(p => p.Orden)
                        .Select(p => new
                        {
                            IdPregunta = p.IdEncuestaPregunta,
                            Orden = p.Orden ?? 0,
                            Tipo = p.TipoPregunta,
                            Texto = p.Pregunta,
                            ValoresY = p.ValoresY,
                            ValoresX = p.ValoresX,
                        })
                        .ToListAsync(cancellationToken);

                    // 2) Traer respuestas cabecera 
                    var respuestas = await _context.NPS_Respuesta
                        .AsNoTracking()
                        .Where(r => r.IdEncuesta == request.Id)
                        .Select(r => new
                        {
                            r.IdRespuesta,
                            r.NroDocumentoCliente,
                            r.CodigoLogAsociado,
                            r.Fecha,
                            r.LinkInicio
                        })
                        .ToListAsync(cancellationToken);

                    var idsRespuesta = respuestas.Select(r => r.IdRespuesta).ToList();

                    // 3) Traer detalle 
                    var detalles = await _context.NPS_DetalleRespuesta
                        .AsNoTracking()
                        .Where(d => d.IdRespuesta != null && idsRespuesta.Contains(d.IdRespuesta.Value))
                        .Select(d => new
                        {
                            IdRespuesta = d.IdRespuesta!.Value,
                            IdPregunta = d.IdPregunta!.Value,
                            d.ValorRespuesta,
                            d.ValorComentario
                        })
                        .ToListAsync(cancellationToken);

                    var detIndex = detalles.ToDictionary(
                        d => (d.IdRespuesta, d.IdPregunta),
                        d => d
                    );

                    var encuesta = await _context.NPS_Encuesta
                        .Where(e =>
                                e.IdEncuesta == request.Id)
                        .Select(e => new EncuestaDto
                        {
                            NombreEncuesta=e.NombreEncuesta,
                            IdEncuesta = e.IdEncuesta

                        }).FirstOrDefaultAsync();
                            // 4) Armar estructura exportable
                            var export = new ExportRespuestasEncuestaDto();

                    // Columnas fijas
                    export.Columns.Add(new ExportColumnDto("Documento", "Documento"));
                    export.Columns.Add(new ExportColumnDto("Area", "Area evaluada"));
                    export.Columns.Add(new ExportColumnDto("Fecha", "Fecha y hora Resulta"));
                    export.Columns.Add(new ExportColumnDto("Ip", "IP"));

                    // Columnas dinámicas por pregunta
                    foreach (var p in preguntas)
                    {
                        if (p.Tipo == "Escala Numérica")
                        {
                            export.Columns.Add(new ExportColumnDto($"Q{p.Orden}_escala", $"[{p.Texto}]"));
                            export.Columns.Add(new ExportColumnDto($"Q{p.Orden}_comentario", $"[{p.Texto}_comentario]"));
                        }
                        else if (p.Tipo == "Pregunta")
                        {
                            export.Columns.Add(new ExportColumnDto($"Q{p.Orden}_texto", $"[{p.Texto}]"));
                        }
                        else if (p.Tipo == "Escala de Likert")
                        {
                            var ys = (p.ValoresY ?? "")
                                .Split('|', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                                .ToList();

                            for (int i = 0; i < ys.Count; i++)
                            {
                                export.Columns.Add(new ExportColumnDto(
                                    $"Q{p.Orden}_likert_{i + 1}",
                                    $"{p.Texto}_{ys[i]}"
                                ));
                            }
                        }

                    }
                    // 5) Filas 
                    foreach (var r in respuestas)
                    {
                        var row = new Dictionary<string, string?>()
                        {
                            ["Documento"] = r.NroDocumentoCliente,
                            ["Area"] = encuesta.NombreEncuesta,
                            ["Fecha"] = r.Fecha?.ToString("dd/MM/yyyy HH:mm"),
                            ["LinkInicio"] = r.LinkInicio
                        };

                        foreach (var p in preguntas)
                        {
                            detIndex.TryGetValue((r.IdRespuesta, p.IdPregunta), out var det);

                            if (p.Tipo == "Escala Numérica")
                            {
                                row[$"Q{p.Orden}_escala"] = det?.ValorRespuesta;
                                row[$"Q{p.Orden}_comentario"] = det?.ValorComentario;
                            }
                            else if (p.Tipo == "Pregunta")
                            {
                                
                                row[$"Q{p.Orden}_texto"] = det?.ValorComentario ?? det?.ValorRespuesta;
                            }
                            else if (p.Tipo == "Escala de Likert")
                            {
                                var parts = (det?.ValorRespuesta ?? "")
                                    .Split('|', StringSplitOptions.None)
                                    .Select(x => x.Trim())
                                    .ToList();

                                var xs = (p.ValoresX ?? "")
                                    .Split('|', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => x.Trim())
                                    .ToList();

                                var yCount = (p.ValoresY ?? "")
                                    .Split('|', StringSplitOptions.RemoveEmptyEntries)
                                    .Length;

                                for (int i = 0; i < yCount; i++)
                                {
                                   
                                    string? textoSeleccionado = null;

                                    if (i < parts.Count && int.TryParse(parts[i], out var idx))
                                    {
                                      
                                        if (idx >= 0 && idx < xs.Count)
                                            textoSeleccionado = xs[idx];
                                        else
                                            textoSeleccionado = parts[i]; 
                                    }

                                    row[$"Q{p.Orden}_likert_{i + 1}"] = textoSeleccionado;
                                }
                            }
                        }

                        export.Rows.Add(row);
                    }

                    return export;
                }
            }
        }
        
    
    }
}

