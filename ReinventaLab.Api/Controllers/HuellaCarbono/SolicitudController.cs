using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion;
using Reinventa.Aplicacion.HuellaCarbono;
using Reinventa.Dominio.HuellaCarbono;
using Reinventa.Dominio.NPS;

namespace ReinventaLab.Api.Controllers.HuellaCarbono
{
    //https://localhost:44396/api/HuellaCarbono/Solicitud
    [Route("api/HuellaCarbono/[controller]")]
    [ApiController]

    public class SolicitudController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SolicitudController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseTransacciones>> Crear(Transacciones.Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseTransacciones>> Modificar(Transacciones.Modifica.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost("UpdateCompletadoUsuario")]
        public async Task<ActionResult<ResponseTransacciones>> UpdateCompletadoUsuario(Transacciones.ActualizaEstadoUsuario.Ejecuta data)
        {
            return await _mediator.Send(data);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<HUELLACARBONO_SOLICITUDCLIENTE>> Detalle(int id)
        {
            return await _mediator.Send(new Consultas.SolicitudIdCliente.SolicitudUnica { IdCliente = id });
        }

        [HttpGet("Resultados/{id}")]
        public async Task<ActionResult<HUELLACARBONO_RESULTADO>> Resultados(int id)
        {
            return await _mediator.Send(new Consultas.ResultadosIdCliente.SolicitudUnica { IdCliente = id });
        }
    }
}
