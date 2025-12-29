using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion;
using Reinventa.Aplicacion.NPS;
using Reinventa.Utilitarios.DTOS;

namespace ReinventaLab.Api.Controllers.NPS
{
    //https://localhost:44396/api/NPS/Encuesta
    [Route("api/NPS/[controller]")]
    [ApiController]
    public class EncuestaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EncuestaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<EncuestaDto>>> Get()
        {
            return await _mediator.Send(new Consultas.Encuestas.ListaEncuesta());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EncuestaDto>> Detalle(int id)
        {
            return await _mediator.Send(new Consultas.EncuestaId.EncuestaUnico { Id = id });
        }

        [HttpPost("CargarClientes")]
        public async Task<ActionResult<ResponseTransacciones>> CargarClientes(Transacciones.CargarClientesEncuesta.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost("CrearEncuesta")]
        public async Task<ActionResult<ResponseTransacciones>> CrearEncuesta(Transacciones.CrearEncuesta.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost("ActualizarEncuesta")]
        public async Task<ActionResult<ResponseTransacciones>> ActualizarEncuesta(Transacciones.ActualizaEncuesta.Ejecuta data)
        {
            return await _mediator.Send(data);
        }


        [HttpGet("ClientesBaseEncuesta/{id}")]
        public async Task<ActionResult<List<ClienteEncuestaDto>>> ClientesBaseEncuesta(int id)
        {
            var result = await _mediator.Send(new Consultas.BaseClienteEncuesta.ListaBaseClientesEncuesta { Id = id });
            return Ok(result);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult<Unit>> Editar(int id, Transacciones.Pregunta_Modificar.Ejecuta data)
        //{
        //    data.IdPregunta = id;
        //    return await _mediator.Send(data);
        // }
    }
}
