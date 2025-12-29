using ReinventaLab.App.Helper.Entidades;
using ReinventaLab.App.Pages.NPS;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ReinventaLab.App.Helper.Repositorios
{
    public class Repositorio_NPS
    {
        private readonly HttpClient _http;

        public Repositorio_NPS(HttpClient http)
        {
            _http = http;
        }

        
        public async Task<EncuestaResponse> ObtenerEncuestas(string Token, int Id)
        {
            EncuestaRequest request = new EncuestaRequest();

            var token = Token; 
            _http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.PostAsJsonAsync("api/NPS/Encuesta", request);

            if (Id > 0)
            {
               response = await _http.PostAsJsonAsync(string.Concat("api/NPS/Encuesta/",Id), request);
            }           
                      

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<EncuestaResponse>();

                return result!;
            }

            return new EncuestaResponse
            {
               
            };

        }
    }
}
