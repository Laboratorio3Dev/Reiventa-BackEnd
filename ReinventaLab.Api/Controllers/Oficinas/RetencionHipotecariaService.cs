using Reinventa.Aplicacion.Oficina.RetencionHipotecaria.ListarRetencionHipotecaria;
using System.Net.Http.Headers;

namespace ReinventaLab.Api.Controllers.Oficinas
{
    public class RetencionHipotecariaService
    {
        private readonly HttpClient _http;

        public RetencionHipotecariaService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<List<RetencionHipotecariaSolicitudDTO>> Listar(
            string token,
            int page,
            int pageSize)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var url = $"api/Oficinas/RetencionHipotecaria?page={page}&pageSize={pageSize}";

            return await _http.GetFromJsonAsync<List<RetencionHipotecariaSolicitudDTO>>(url)
                   ?? new List<RetencionHipotecariaSolicitudDTO>();
        }
    }
}
