using MediatR;
using Microsoft.EntityFrameworkCore;
using Reinventa.Aplicacion.Contratos;
using Reinventa.Aplicacion.Seguridad.HuellaCarbono;
using Reinventa.Dominio.HuellaCarbono;
using Reinventa.Dominio.NPS;
using Reinventa.Persistencia.HuellaCarbono;
using Reinventa.Persistencia.NPS;
using Reinventa.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.HuellaCarbono
{
    public class Consultas
    {
        public class LoginEmpresa
        {
            public class LoginUnico : IRequest<Login>
            {
                public string? RUC { get; set; }
                public string? Password { get; set; }
            }

            public class Manejador : IRequestHandler<LoginUnico, Login>
            {
                private readonly Huella_Context _context;
                private readonly IJwtGenerador _jwtGenerador;
                public Manejador(Huella_Context context, IJwtGenerador jwtGenerador)
                {
                    _context = context;
                    _jwtGenerador = jwtGenerador;
                }

                public async Task<Login?> Handle(LoginUnico request, CancellationToken cancellationToken)
                {
                    var usuario = await _context.HUELLACARBONO_CLIENTE.
                        Where(x => x.Ruc == request.RUC && x.Clave == CryptoHelper.Encrypt(request.Password)
                        && x.Estado == 1).FirstOrDefaultAsync();

                    var contador = 0;
                    if (usuario != null)
                    {
                        var registro = await _context.HUELLACARBONO_SOLICITUDCLIENTE.Where(x => x.IdCliente == usuario.Id).ToListAsync();
                        contador= registro.Count;
                    }
                    

                    if (usuario == null)
                    {
                        return new Login
                        {
                            ErrorMessage = "Usuario o Contraseña incorrecto",
                            IsSuccess = false,                           
                            Token = string.Empty  
                        };
                    }

                    return new Login
                    {
                        IdCliente= usuario.Id,
                        IsSuccess = true,
                        RazonSocial = usuario.RazonSocial,
                        Token = _jwtGenerador.CrearTokenLanding(usuario.Ruc),
                        ErrorMessage = "OK",
                        FlagSolicitud= contador,
                        FlagCompletado= usuario.FlagCompletado
                    };
                }


            }


        }

        public class SolicitudIdCliente
        {
            public class SolicitudUnica : IRequest<HUELLACARBONO_SOLICITUDCLIENTE>
            {
                public int IdCliente { get; set; }
            }

            public class Manejador : IRequestHandler<SolicitudUnica, HUELLACARBONO_SOLICITUDCLIENTE>
            {
                private readonly Huella_Context _context;
                public Manejador(Huella_Context context)
                {
                    _context = context;
                }

                public async Task<HUELLACARBONO_SOLICITUDCLIENTE> Handle(SolicitudUnica request, CancellationToken cancellationToken)
                {
                    var result= await _context.HUELLACARBONO_SOLICITUDCLIENTE.Where(x=> x.IdCliente==request.IdCliente).ToListAsync();
                    if(result.Count > 0)
                    {
                        return result.First();
                    }
                    else
                    {
                        return null;
                    }
                }

               
            }


        }

        public class ResultadosIdCliente
        {
            public class SolicitudUnica : IRequest<HUELLACARBONO_RESULTADO>
            {
                public int IdCliente { get; set; }
            }

            public class Manejador : IRequestHandler<SolicitudUnica, HUELLACARBONO_RESULTADO>
            {
                private readonly Huella_Context _context;
                public Manejador(Huella_Context context)
                {
                    _context = context;
                }

                public async Task<HUELLACARBONO_RESULTADO> Handle(SolicitudUnica request, CancellationToken cancellationToken)
                {
                    var result = await _context.HUELLACARBONO_RESULTADO.Where(x => x.IdCliente == request.IdCliente).ToListAsync();
                    if (result.Count > 0)
                    {
                        return result.First();
                    }
                    else
                    {
                        return null;
                    }
                }


            }


        }
    }
}
