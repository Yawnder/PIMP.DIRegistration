using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMP.DIRegistration.Core
{
    public interface IRegistrator<T>
    {

        /// <summary>
        /// Optional name on which other IRegistrator will depend.
        /// Should be unique as much as possible to avoid side effects.
        /// Should be overridden if inherited because the default is costly to obtain.
        /// </summary>
        string DependencyName { get; }

        /// <summary>
        /// Gets the collection of <see cref="DependencyName"/>s on which this Registrator depends.
        /// <param name="context">Can be passed to the <see cref="IRegistrator{T}"/> to filter dependancies.</param>
        /// </summary>
        IEnumerable<string> GetDependancies(string context = null);

        /// <summary>
        /// Will be called when all Dependancies obtained from <see cref="GetDependancies(string)"/> have been registered.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context">Used to discriminate on registration. Mainly for Solutions with multiple entry projects.</param>
        void Register(T container, string context = null);

    }
}
