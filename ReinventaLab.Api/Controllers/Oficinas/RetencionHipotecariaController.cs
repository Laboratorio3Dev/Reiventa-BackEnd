using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecaria;

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
      [FromQuery] DateTime? fechaInicio,
      [FromQuery] DateTime? fechaFin,
      [FromQuery] int page = 1,
      [FromQuery] int pageSize = 10)
        {
            var usuario = User.Identity?.Name;

            var result = await _mediator.Send(
                new ListarRetencionHipotecariaQuery(
                    usuario!,
                    fechaInicio,
                    fechaFin,
                    page,
                    pageSize
                )
            );

            return Ok(result);
        }
    }
}
