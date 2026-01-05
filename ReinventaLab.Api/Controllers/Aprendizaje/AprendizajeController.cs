using Aspose.Cells;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reinventa.Aplicacion;
using Reinventa.Aplicacion.Aprendizaje;
using Reinventa.Aplicacion.BackOffice;
using Reinventa.Dominio.Aprendizaje;
using Reinventa.Dominio.BackOffice;
using Reinventa.Dominio.UsuarioRoles;
using Reinventa.Utilitarios.DTOS;
using static Reinventa.Aplicacion.Aprendizaje.Transacciones;



namespace ReinventaLab.Api.Controllers.Aprendizaje
{
    //https://localhost:44396/api/BackOffice/Aprendizaje
    [Route("api/BackOffice/[controller]")]
    [ApiController]
    public class AprendizajeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AprendizajeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CombosGenerales")]
        public async Task<ActionResult<DatosGeneralesDTO>> CombosGenerales(Consultas.DatosGenerales.ListadosGenerales data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost("PlanesAccion")]
        public async Task<ActionResult<List<PlanAccionDTO>>> PlanesAccion(Consultas.PlanesAccion.ListadosPlanesAccion data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost("Tareas")]
        public async Task<ActionResult<List<TareasDTO>>> Tareas(Consultas.ListaTareas.ListadoTareas data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost("IniciarPlan")]
        public async Task<ActionResult<ResponseTransacciones>> IniciarPlan(Transacciones.IniciarPlan.InicioPlanAccion data)
        {
            return await _mediator.Send(data);
        }
        [HttpPost("EnviarGO")]
        public async Task<ActionResult<ResponseTransacciones>> EnviarGO(Transacciones.EnviarGO.EnvioPlanGO data)
        {
            return await _mediator.Send(data);
        }
        [HttpPost("RechazoGO")]
        public async Task<ActionResult<ResponseTransacciones>> RechazoGO(Transacciones.RechazoGO.RechazoPlanGO data)
        {
            return await _mediator.Send(data);
        }
        [HttpPost("AsignarTareas")]
        public async Task<ActionResult<ResponseTransacciones>> AsignarTareas(Transacciones.AsignarTareas.EjecutaTareas data)
        {
            return await _mediator.Send(data);
        }
        [HttpPost("CompletarPlanGO")]
        public async Task<ActionResult<ResponseTransacciones>> CompletarPlanGO(Transacciones.CompletarPlan.CompletarPlanGO data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost("DatosDashboard")]
        public async Task<List<ListadoDashboard_DTO>> DatosDashboard(Consultas.Dashboard.ListadosDashboard data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost("ListarComentarios")]
        public async Task<List<ProductoInsightDTO>> ListarComentarios(Consultas.ListarComentarios.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpPost("ListarUsuariosAprendizaje")]
        public async Task<List<UsuarioDTO>> ListarUsuariosAprendizaje(InicioSesion.ListarUsuario.Ejecutar data)
        {
            return await _mediator.Send(data);
        }

        private int ObtenerCumplimiento(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return 0;

            texto = texto.ToUpper();

            if (texto.Contains("NO CUMPLE") )
                return 0;

            return 1;
        }

        private int ObtenerIdDimension(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return 0;

            texto = texto.ToUpper();

            if (texto.Contains("PROSPECCION")) return 1;
            if (texto.Contains("PROSPECCIÓN")) return 1;
            if (texto.Contains("RENTABILIDAD")) return 2;
            if (texto.Contains("DOCUMENTACION")) return 3;
            if (texto.Contains("DOCUMENTACIÓN")) return 3;
            if (texto.Contains("RIESGOS")) return 4;
            if (texto.Contains("SEGUROS")) return 5;
            if (texto.Contains("COBRANZA")) return 6;
            if (texto.Contains("PROTOCOLO")) return 7;

            return 0;
        }
               

        [HttpPost("CargarDataDashoard")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ResponseTransacciones>> CargarDataDashoard([FromForm]  Consultas.CargarDashboardRequest request)
        {
            if (request.File == null || (request.File.Length == 0))
                return BadRequest("Archivo inválido");

            using var stream = request.File.OpenReadStream();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0;

            var workbook = new Workbook(ms);
            var worksheet = workbook.Worksheets[0];
            var cells = worksheet.Cells;

            int totalFilas = cells.MaxDataRow;
            var lista = new List<SA_DASHBOARD>();
            var listaComentarios = new List<SA_DASHBOARD_COMENTARIO>();

            for (int row = 1; row <= totalFilas; row++)
            {
                if (!short.TryParse(cells[row, 0].StringValue, out short mes)) continue;
                if (!short.TryParse(cells[row, 1].StringValue, out short anio)) continue;
                if (!short.TryParse(cells[row, 3].StringValue, out short idProducto)) continue;
                lista.Add(new SA_DASHBOARD
                {
                    MES = mes,
                    ANIO = anio,
                    USUARIO = cells[row, 2].StringValue,
                    ID_PRODUCTO = idProducto,
                    ID_DIMENSION = ObtenerIdDimension(cells[row, 4].StringValue),
                    INDICADOR = cells[row, 5].StringValue,
                    UMBRAL = cells[row, 6].StringValue,
                    RESULTADO = cells[row, 7].StringValue,
                    CUMPLIMIENTO = ObtenerCumplimiento(cells[row, 8].StringValue)
                });
            }


            // ========= HOJA 2 (OPCIONAL) =========
            if (workbook.Worksheets.Count > 1)
            {
                worksheet = workbook.Worksheets[1];
                cells = worksheet.Cells;
                totalFilas = cells.MaxDataRow;

                for (int row = 1; row <= totalFilas; row++)
                {
                    listaComentarios.Add(new SA_DASHBOARD_COMENTARIO
                    {
                        MES = Convert.ToInt16(cells[row, 0].StringValue),
                        ANIO = Convert.ToInt16(cells[row, 1].StringValue),
                        ID_PRODUCTO = Convert.ToInt16(cells[row, 3].StringValue),
                        USUARIO = cells[row, 2].StringValue,
                        RESUMEN = cells[row, 4].StringValue,
                        INSIGHT = cells[row, 5].StringValue,
                        PREGUNTAS = cells[row, 6].StringValue
                    });
                }
            }

            var command = new CargarDataDashoard.Ejecuta
            {
                Usuario = request.Usuario,
                sA_DASHBOARDs = lista,
                sA_DASHBOARD_COMENTARIOs = listaComentarios
            };

            return await _mediator.Send(command);
        }
    }

}

