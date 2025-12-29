using FluentValidation;
using MediatR;
using Reinventa.Dominio.BackOffice;
using Reinventa.Dominio.NPS;
using Reinventa.Persistencia.BackOffice;
using Reinventa.Persistencia.NPS;
using Reinventa.Utilitarios;
using Reinventa.Utilitarios.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Aplicacion.BackOffice
{
    public class Log
    {
        public class CrearLog
        {
            public class Ejecuta : IRequest<ResponseTransacciones>
            {
                public string? CodigoUnico { get; set; }
                public int? Paso { get; set; }
                public string? DetalleLog { get; set; }
                public string? IpCliente { get; set; }
                public string? Utm { get; set; }
                public string? Source { get; set; }
                public string? Medium { get; set; }
                public string? Campaign { get; set; }
                public string? SistemaOperativo { get; set; }
            }

            public class EjecutaValidacion : AbstractValidator<Ejecuta>
            {
                public EjecutaValidacion()
                {
                    RuleFor(x => x.CodigoUnico).NotEmpty();
                    RuleFor(x => x.Paso).NotEmpty();
                    RuleFor(x => x.DetalleLog).NotEmpty();
                    RuleFor(x => x.IpCliente).NotEmpty();
                }
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
                    var New_Registro = new LAB_Log
                    {
                        Fecha_Log = DateTime.Now.AddHours(-5),
                        Codigo_Unico= request.CodigoUnico,
                        Campaign= request.Campaign,
                        Detalle_Log= request.DetalleLog,
                        Ip_Cliente= request.IpCliente,
                        Medium= request.Medium,
                        Paso= request.Paso,
                        Sistema_Operativo= request.SistemaOperativo,
                        Source= request.Source,
                        Utm= request.Utm
                    };

                    // 2. Agregar al contexto
                    _context.LAB_Log.Add(New_Registro);                     

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
                        IdValue = New_Registro.Id_Log
                    };
                }
            }
        }
    }
}
