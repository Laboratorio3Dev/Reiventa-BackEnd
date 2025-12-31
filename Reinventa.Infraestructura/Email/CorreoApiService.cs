using Microsoft.Extensions.Options;
using Reinventa.Infraestructura.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Infraestructura.Email
{
    public interface ICorreoService
    {
        Task EnviarCorreoAsync(string token, object payload);
    }

    public class CorreoApiService : ICorreoService
    {
        private readonly HttpClient _http;
        private readonly CorreoApiSettings _settings;

        public CorreoApiService(
            HttpClient http,
            IOptions<CorreoApiSettings> settings)
        {
            _http = http;
            _settings = settings.Value;
        }

        public async Task EnviarCorreoAsync(string token, object payload)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.PostAsJsonAsync(
                _settings.ApiUrl,
                payload
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error enviando correo: {error}");
            }
        }
    }
}
