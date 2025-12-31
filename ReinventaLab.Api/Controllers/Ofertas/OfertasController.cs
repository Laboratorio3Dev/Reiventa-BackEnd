using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion;
using Reinventa.Aplicacion.Ofertas;
using Reinventa.Dominio.Ofertas;
using Reinventa.Utilitarios.DTOS;
using static Reinventa.Aplicacion.Ofertas.Transaccional;

namespace ReinventaLab.Api.Controllers.Ofertas
{
    //https://localhost:44396/api/BackOffice/Ofertas
    [Route("api/BackOffice/[controller]")]
    [ApiController]
    public class OfertasController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OfertasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("ValidarOfertaPLD")]
        public async Task<ActionResult<PLD_BaseCliente>> ValidarOfertaPLD(Transaccional.ValidarOferta.BusquedaEjecutar data)
        {
            return await _mediator.Send(data);
        }

        //[HttpPost("CargarBaseClientes")]
        //public async Task<ActionResult<ResponseTransacciones>> CargarBaseClientes(Transaccional.CargarBaseClientes.Ejecuta data)
        //{
        //    return await _mediator.Send(data);
        //}

        [HttpPost("CargarBaseClientes")]
        public async Task<IActionResult> CargarBaseClientes( IFormFile archivo, CancellationToken cancellationToken)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo inválido");

            var response = await _mediator.Send(
                new CargarBaseClientes.Ejecuta
                {
                    Archivo = archivo
                },
                cancellationToken
            );

            return Ok(response);
        }

    }
}
