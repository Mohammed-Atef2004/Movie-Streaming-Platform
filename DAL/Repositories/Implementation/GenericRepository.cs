using DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public bool Add(T entity)
        {
            _dbSet.Add(entity);
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predict=null, string? includeword= null)
        {
            IQueryable<T> query = _dbSet;
            if(predict != null)
            {
                query = query.Where(predict);
            }
            if(includeword != null)
            {
                foreach(var item in includeword.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? predict = null, string? includeword = null)
        {
            IQueryable<T> query = _dbSet;
            if (predict != null)
            {
                query = query.Where(predict);
            }
            if (includeword != null)
            {
                foreach (var item in includeword.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.SingleOrDefault();
        }

        public bool Remove(T entity)
        {
            _dbSet.Remove(entity);
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public bool RemoveRange(IEnumerable<T> entities)
        {
           _dbSet.RemoveRange(entities);
            if (_context.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
