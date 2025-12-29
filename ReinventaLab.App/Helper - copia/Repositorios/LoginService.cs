using ReinventaLab.App.Helper.Entidades;
using System.Net.Http.Json;

namespace ReinventaLab.App.Helper.Repositorios
{
    public class LoginService
    {
        private readonly HttpClient _http;

        public LoginService(HttpClient http)
        {
            _http = http;
        }

        //var token = "tu_token_aqui"; // normalmente lo obtienes al iniciar sesión
        //_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //var response = await _http.PostAsJsonAsync("api/NPS/Login/acceso", request);
        public async Task<LoginResponse> LoginAsync(string Usuario, string Password)
        {
            LoginRequest request = new LoginRequest();
            request.Usuario = Usuario;// "cvillavicencio";
            request.Password = Password;//"12345";    
            var response = await _http.PostAsJsonAsync("api/NPS/Login/acceso", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                return result!;
            }

            return new LoginResponse
            {
                IsSuccess = false,
                ErrorMessage = "Usuario o contraseña incorrectos"
            };
        }
    }
}
