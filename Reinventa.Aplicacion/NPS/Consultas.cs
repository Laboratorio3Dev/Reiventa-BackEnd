using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Persistencia.NPS;
using Reinventa.Utilitarios.DTOS;


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
                            ImagenLogin= e.ImagenLogin,
                            TituloEncuesta= e.TituloEncuesta,
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
                                TextoValorMinimo= p.TextoValorMinimo,
                                TextoValorMaximo= p.TextoValorMaximo
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

            public class Manejador : IRequestHandler<EncuestaUnico, EncuestaDto>
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
                        .Where(e => e.IdEncuesta == request.Id) 
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
                        .FirstOrDefaultAsync(cancellationToken); // 👈 devuelve solo una encuesta o null

                    return encuesta;
                }


                //public async Task<NPS_Encuesta> Handle(EncuestaUnico request, CancellationToken cancellationToken)
                //{
                //    return await _context.NPS_Encuesta.
                //      Include(x => x.ClientesEncuesta).
                //      Include(x => x.EncuestaPreguntas).Where(x => x.IdEncuesta == request.Id)
                //      .FirstAsync();
                //}
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

                    var encuesta= _context.NPS_Encuesta.Where(x=> x.IdEncuesta== request.Id).FirstOrDefault();

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
                            LinkPersonalizado = string.Concat("https://reinventa.banbif.com.pe/", encuesta.NombreEncuesta, "/", c.LinkPersonalizado)
                        })
                        .ToListAsync(cancellationToken);

                    return data;
                }
            }
        }


        //public class RespuestasEncuesta
        //{
        //    public class EncuestaUnica : IRequest<Login>
        //    {
        //        public int IdEncuesta{ get; set; }
        //    }

        //    public class Manejador : IRequestHandler<EncuestaUnica, RespuestasEncuestaDTO>
        //    {
        //        private readonly NPS_Context _context;
        //        private readonly IJwtGenerador _jwtGenerador;
        //        public Manejador(NPS_Context context, IJwtGenerador jwtGenerador)
        //        {
        //            _context = context;
        //            _jwtGenerador = jwtGenerador;
        //        }


        //        public Task<RespuestasEncuestaDTO> Handle(EncuestaUnica request, CancellationToken cancellationToken)
        //        {
        //            //var param = new SqlParameter("@IDCLIENTE", request.IdCliente);

        //            //var listado = await _context.NPS_DetalleRespuesta
        //            //    .FromSqlRaw("EXEC SP_HUELLACARBONO_RESULTADO @IDCLIENTE", param)
        //            //    .ToListAsync();
        //        }
        //    }


        //}

       
    }
}
