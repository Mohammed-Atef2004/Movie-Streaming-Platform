using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Abstraction
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? predict = null, string? includeword = null);
        T GetFirstOrDefault(Expression<Func<T, bool>>? predict = null, string? includeword = null);
        bool Add(T entity);
        bool Remove(T entity);
        bool RemoveRange(IEnumerable<T> entities);

    }
}
