using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext context)
        {
                _context = context;
            dbSet=_context.Set<T>();
        }


        public async Task Crear(T obj)
        {
            await dbSet.AddAsync(obj);
            await Grabar();
        }



        public async Task Grabar()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true)
        {
            IQueryable<T> query = dbset;
        }

        public Task<List<T>> ObtenerTodo(Expression<Func<T, bool>>? filtro = null)
        {
            throw new NotImplementedException();
        }

        public Task Remover(T entidad)
        {
            throw new NotImplementedException();
        }
    }
}
