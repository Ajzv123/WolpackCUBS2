using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LibraryModels.DTO;


namespace SurveyAppComponent.Repos
{
    public class Repositorio : IRepositorio
    {
        private readonly HttpClient httpClient;
        public Repositorio (HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("https://localhost:7290/");
        }

        private JsonSerializerOptions OpcionesPorDefectoJSON => new JsonSerializerOptions() 
        { 
            PropertyNameCaseInsensitive = true 
        };

        public async Task<HttpsResponseWrapper<object>> Delete(string url)
        {
            var responseHTTP = await httpClient.DeleteAsync(url);
            return new HttpsResponseWrapper<object>(null, 
                !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        public async Task<HttpsResponseWrapper<T>> Get<T>(string url)
        {
            var responseHTTP = await httpClient.GetAsync(httpClient.BaseAddress+url);
            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await DeserializarRespuesta<T>(responseHTTP, OpcionesPorDefectoJSON);
                return new HttpsResponseWrapper<T>(response, false, responseHTTP);
            }
            else
            {
                return new HttpsResponseWrapper<T>(default, true, responseHTTP);
            }
        }

        public List<UsuarioDTO> ObtenerUsuarios()
        {
            throw new NotImplementedException();
        }

        public async Task<HttpsResponseWrapper<object>> Post<T>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHTTP = await httpClient.PostAsync(url, enviarContent);
            return new HttpsResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        public async Task<HttpsResponseWrapper<TResponse>> Post<T, TResponse>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHTTP = await httpClient.PostAsync(url, enviarContent);
            
            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await DeserializarRespuesta<TResponse>(responseHTTP, OpcionesPorDefectoJSON);
                return new HttpsResponseWrapper<TResponse>(response, false, responseHTTP);
            }
            else
            {
                return new HttpsResponseWrapper<TResponse>(default, !responseHTTP.IsSuccessStatusCode, responseHTTP);
            }
        }

        public async Task<HttpsResponseWrapper<object>> Put<T>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHTTP = await httpClient.PutAsync(url, enviarContent);
            return new HttpsResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        private async Task<T> DeserializarRespuesta<T>(HttpResponseMessage httpResponseMessage, JsonSerializerOptions jsonSerializarOption)
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, jsonSerializarOption);
        }
    }
}
