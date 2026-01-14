using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        //Se exportan las respuestas a un excel para poder ser visualizados
        [HttpGet("ExportarRespuestas")]
        public async Task<IActionResult> ExportarRespuestas([FromQuery] int idEncuesta, CancellationToken ct)
        {
            if (idEncuesta <= 0) return BadRequest("IdEncuesta inválido.");

            var result = await _mediator.Send(new Consultas.ExportarRespuestasEncuesta.Query { Id = idEncuesta }, ct);
            return Ok(result);
        }
        [HttpDelete("EliminarEncuesta/{id}")]
        public async Task<ActionResult<EncuestaDto>> GetEncuestaEncriptada(int id)
        {
            var command = new Transacciones.EliminarEncuesta.Ejecuta
            {
                IdEncuesta = id,
                Usuario = User.Identity?.Name ?? "SISTEMA"
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //Permite cargar la encuesta mediante la URL encriptada de la misma
        [AllowAnonymous]
        [HttpGet("CargarEncuestaEncriptada")]
        public async Task<ActionResult<EncuestaDto>> GetEncuestaEncriptada([FromQuery] string encuesta)
        {
            return await _mediator.Send(new Consultas.EncuestaId.EncuestaValorEncriptado { ValorEncriptado = encuesta });
        }
        //Valida el DNI del cliente con la URL encriptada que representa al cliente si es que se usa el login
        [AllowAnonymous]
        [HttpPost("ValidarClienteEncuesta")]
        public async Task<ActionResult<string>> PostURLUnicoCliente([FromBody] ValidarClienteRequest dniRequest, [FromQuery] string encuesta)
        {
            return await _mediator.Send(new Consultas.DNICliente.URLCliente { DNICliente = dniRequest.NroDocumento, ValorEncriptado = encuesta });
        }
        //Valida que la encuesta esté disponible y que además el cliente sea elegible para responder usando los valores encriptados
        [AllowAnonymous]
        [HttpGet("ExisteClienteEncuesta")]
        public async Task<IActionResult> ExisteClienteEncuesta([FromQuery] string encuesta, [FromQuery] string u)
        {
            var r = await _mediator.Send(new Consultas.DNICliente.ValidarClienteEnEncuesta.Query
            {
                ValorEncriptado = encuesta,
                LinkPersonalizado = u
            });

            return Ok(r);
        }
        //Guarda las respuestas del cliente, siempre y cuando el Flag Contesta esté en 0 y la encuesta esté disponible para contestar
        [AllowAnonymous]
        [HttpPost("GuardarRespuestas")]
        public async Task<IActionResult> GuardarRespuestas([FromBody] GuardarRespuestasRequest request)
        {
            var ok = await _mediator.Send(new Transacciones.Respuestas.GuardarRespuestas.Command { Request = request });
            if (!ok) return BadRequest(false);
            return Ok(true);
        }
    }
}
