using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SurveyAppComponent.Repos
{
    public class HttpsResponseWrapper<T>
    {
        public HttpsResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }
        public bool Error { get; set; }
        public T? Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        
        public async Task<string> ObtenerMensajeErro() 
        {
            if (!Error) 
            { 
                return null;
            }
            var codigoEstatus = HttpResponseMessage.StatusCode;
            if(codigoEstatus == HttpStatusCode.NotFound)
            {
                return "No se encontro el recurso";
            }
            else if (codigoEstatus == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (codigoEstatus == HttpStatusCode.Unauthorized)
            {
                return "Necesitas iniciar sesión para realizar esta acción";
            }
            else if(codigoEstatus == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para realizar esta acción";
            }
            else
            {
                return "Ocurrio un error inesperado";
            }
            
        }
    }

}
