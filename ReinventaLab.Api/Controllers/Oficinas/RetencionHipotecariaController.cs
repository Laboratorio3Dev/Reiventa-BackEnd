using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion.Oficina.RetencionHipotecaria.CrearRetencionHipotecaria;
using Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipoEntidad;
using Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecaria;
using Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecariaProducto;

namespace ReinventaLab.Api.Controllers.Oficinas
{
    [Route("api/Oficinas/[controller]")]
    [ApiController]
    public class RetencionHipotecariaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RetencionHipotecariaController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Listar(
    [FromQuery] ListarRetencionHipotecariaRequest request)
        {


            var query = new ListarRetencionHipotecariaQuery
            {
                Usuario = request.Usuario!,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear(
        [FromBody] CrearRetencionHipotecariaCommand command)
        {
      

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Productos")]
        public async Task<IActionResult> ListarProductos()
        {
            var result = await _mediator.Send(
                new ListarRetencionHipotecariaProductoQuery()
            );

            return Ok(result);
        }

        [HttpGet("Entidades")]
        public async Task<IActionResult> ListarEntidades()
        {
            var result = await _mediator.Send(
                new ListarRetencionHipoEntidadQuery()
            );

            return Ok(result);
        }
    }
}
