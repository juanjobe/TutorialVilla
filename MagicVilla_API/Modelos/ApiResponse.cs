using System.Net;

namespace MagicVilla_API.Modelos
{
    public class ApiResponse<T> where T : class
    {
        public HttpStatusCode  StatusCode { get; set; }
        public bool IsExitoso { get; set; } = true;

        public List<string>? ErrorMessages { get; set; }

        public T?    Dato { get; set; }
        public List<T>? ListaDatos { get; set; }

    }
}
