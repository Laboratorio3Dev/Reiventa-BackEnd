using Azure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion;
using Reinventa.Aplicacion.Oficina.Productos.ActualizarProducto;
using Reinventa.Aplicacion.Oficina.Productos.CrearProducto;
using Reinventa.Aplicacion.Oficina.Productos.EliminarProducto;
using Reinventa.Aplicacion.Oficina.Productos.ListarProductos;
using Reinventa.Aplicacion.Oficina.Productos.ObtenerProducto;

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
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody]
           CrearProductoCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var producto = await _mediator.Send(
                new ObtenerProductoQuery(id)
            );

            if (producto == null)
            {
                return Ok(new ResponseTransacciones
                {
                    IsSuccess = false,
                    Message = "Producto no encontrado",
                    Data = null
                });
            }

            return Ok(new ResponseTransacciones
            {
                IsSuccess = true,
                Message = "Producto obtenido correctamente",
                Data = producto
            });
        }

        [HttpPut("Actualizar")]
        public async Task<IActionResult> Actualizar(ActualizarProductoCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPatch("Desactivar/{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var response = await _mediator.Send(
                new EliminarProductoCommand { IdProducto = id }
            );

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
