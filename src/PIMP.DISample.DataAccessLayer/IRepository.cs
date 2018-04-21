using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PIMP.DISample.DataAccessLayer
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> whereClause);
    }
}