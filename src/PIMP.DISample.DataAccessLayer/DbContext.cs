using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMP.DISample.DataAccessLayer
{
    public class DbContext<T> : IDbContext<T> where T : class, new()
    {

        public DbContext(string connectionString)
        {
            Console.WriteLine($"Please pretend I'm connected to '{connectionString}'.");
        }

        public T Get(string key)
        {
            T entity = null;
            if ((new Random()).Next(0, 10) > 2)
                entity = new T();

            return entity;
        }

    }
}
