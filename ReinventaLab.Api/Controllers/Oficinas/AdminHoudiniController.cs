using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion.Oficina.Productos.ListarProductos;

namespace ReinventaLab.Api.Controllers.Oficinas
{
    [Route("api/Oficinas/[controller]")]
    [ApiController]
    public class AdminHoudiniController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminHoudiniController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ LISTAR (ADMIN)
        [HttpGet]
        public async Task<IActionResult> Listar(
             [FromQuery] string? search
             )
        {
            var result = await _mediator.Send(
                new ListarProductosQuery(search)
            );

            return Ok(result);
        }
    }
}
