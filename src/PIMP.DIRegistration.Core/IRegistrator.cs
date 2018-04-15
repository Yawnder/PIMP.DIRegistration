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
        /// The collection of <see cref="DependencyName"/>s on which this Registrator depends.
        /// </summary>
        IEnumerable<string> Dependancies { get; }

        /// <summary>
        /// Will be called when all <see cref="Dependancies"/> have been registered.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context">Used to discriminate on registration. Mainly for Solutions with multiple entry projects.</param>
        void Register(T container, string context = null);

    }
}
