using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMP.DISample.DataAccessLayer
{
    public interface IDbContext<T>
    {
        T Get(string key);
    }
}
