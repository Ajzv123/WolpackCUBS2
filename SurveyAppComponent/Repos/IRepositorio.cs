using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryModels.DTO;

namespace SurveyAppComponent.Repos
{
    public interface IRepositorio
    {
        Task<HttpsResponseWrapper<object>>Delete(string url);
        Task<HttpsResponseWrapper<T>> Get<T>(string url);
        Task<HttpsResponseWrapper<object>> Post<T>(string url, T enviar);
        Task<HttpsResponseWrapper<TResponse>> Post<T, TResponse>(string url, T enviar);
        Task<HttpsResponseWrapper<object>> Put<T>(string url, T enviar);
        List<UsuarioDTO> ObtenerUsuarios();
    }
}
