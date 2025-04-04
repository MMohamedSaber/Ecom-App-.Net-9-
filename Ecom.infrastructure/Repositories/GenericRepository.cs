
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecom.infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public AppDbContext _context { get; }
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public  async Task DeleteAsync(int id)
        {
            var entity= await _context.Set<T>().FindAsync(id);

               _context.Remove(entity);
            await _context.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<T>> GetAllAsync() 
            =>  await _context.Set<T>().AsNoTracking().ToListAsync(); 
     

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] Include)
        {
            var  query =  _context.Set<T>().AsQueryable();

            foreach (var item in Include)
            {

                query=query.Include(item);

            }

            return  await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            var entity =await _context.Set<T>().FindAsync(Id);
            return entity;

        }

        public async Task<T> GetByIdAsync(int Id, params Expression<Func<T, object>>[] Include)
        {
            IQueryable<T> query =  _context.Set<T>();
            foreach (var item in Include)
            {

                query =   query.Include(item);
            }

            var entity =  query.FirstOrDefault(e => EF.Property<int>(e, "Id") == Id);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
             _context.Entry(entity).State=EntityState.Modified;
            await _context.SaveChangesAsync();

        }
    }
}
