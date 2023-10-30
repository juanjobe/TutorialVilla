using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {
        Task Crear(T obj);
        //se declara que espera una expresion link donde espera un dato y devuelve un boleano para poder devolver registros de la base de datos
        Task<List<T>> ObtenerTodo(Expression<Func<T,bool>>? filtro = null); 
        Task<T?> Obtener(Expression<Func<T,bool>>? filtro=null,bool tracked=true);
        Task Remover(T entidad);

        Task Grabar();
    }
}
