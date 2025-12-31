using Reinventa.Infraestructura.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Reinventa.Infraestructura.Auth
{
    public interface ITokenService
    {
        Task<string> ObtenerTokenAsync();
    }
    public class TokenService : ITokenService
    {
        private readonly HttpClient _http;
        private readonly AuthApiSettings _settings;

        public TokenService(
            HttpClient http,
            IOptions<AuthApiSettings> settings)
        {
            _http = http;
            _settings = settings.Value;
        }

        public async Task<string> ObtenerTokenAsync()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _settings.ClientId,
                ["client_secret"] = _settings.ClientSecret
            });


            var response = await _http.PostAsync(_settings.TokenUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error obteniendo token: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement.GetProperty("access_token").GetString();
        }
    
    }
}
