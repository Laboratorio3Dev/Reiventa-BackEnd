using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion.Oficina.CargarRetencion;

namespace ReinventaLab.Api.Controllers.Oficinas
{
    [ApiController]
    [Route("api/Oficinas/[controller]")]
    public class RetencionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RetencionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CargarExcel")]
        public async Task<IActionResult> CargarExcel(IFormFile Archivo)
        {
            var response = await _mediator.Send(
                new CargarRetencionExcelCommand(Archivo)
            );

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
