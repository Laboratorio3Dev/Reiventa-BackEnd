using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion.Oficina.VentaDigital.RegistrarVentaDigital;

namespace ReinventaLab.Api.Controllers.Oficinas
{
    [ApiController]
    [Route("api/VentaDigital")]
    public class VentaDigitalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VentaDigitalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar(
            RegistrarVentaDigitalRequest request)
        {
            var result = await _mediator.Send(
                new RegistrarVentaDigitalCommand { Request = request });

            return Ok(result);
        }
    }
}
