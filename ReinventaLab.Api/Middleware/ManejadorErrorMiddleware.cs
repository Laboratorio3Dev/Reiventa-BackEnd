using Newtonsoft.Json;
using Reinventa.Aplicacion.ManejadorErrores;
using System.Net;

namespace ReinventaLab.Api.Middleware
{
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorErrorMiddleware> _logger;
       
        public ManejadorErrorMiddleware(RequestDelegate next,ILogger<ManejadorErrorMiddleware> logger)
        {          
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {           
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
              await ManejadorExcepcionAsincrono(context, ex, _logger);
            }
        }

        private async Task ManejadorExcepcionAsincrono(HttpContext httpContext, Exception ex, ILogger<ManejadorErrorMiddleware> logger)
        {
            object? errores= null;
            switch (ex)
            {
                case ManejadorExcepcion me: logger.LogError(ex, "Manejador Error");
                    errores= me.Errores; 
                    httpContext.Response.StatusCode= (int)me.Codigo;
                    break;

                case Exception e:
                    logger.LogError(ex, "Error de Servidor");
                    errores = string.IsNullOrWhiteSpace(e.Message)? "Error": e.Message;
                    httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            httpContext.Response.ContentType = "application/json";
           
            if (errores != null)
            {
                var resultado =  JsonConvert.SerializeObject(new {errores});
                await httpContext.Response.WriteAsync(resultado);
            }
        }

    }
}
