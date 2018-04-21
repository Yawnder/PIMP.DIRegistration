using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PIMP.DISample.DataAccessLayer
{
    public class Repository<T> : IRepository<T>
    {

        private IDbContext<T> context { get; }

        public Repository(IDbContext<T> context)
        {
            this.context = context;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> whereClause)
        {
            List<T> result = new List<T>();

            int count = (new Random()).Next(0, 10);

            for (int i = 0; i < count; i++)
                result.Add(context.Get($"{i}"));

            return result;
        }

    }
}
