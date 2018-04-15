using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PIMP.DIRegistration.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type, usually an interface, of the specific container for the Inversion of Control framework.</typeparam>
    public abstract class Registrator<T> : IRegistrator<T>
    {

        /// <summary>
        /// Optional name on which other IRegistrator will depend.
        /// Should be unique as much as possible to avoid side effects.
        /// Should be overridden if inherited because the default is costly to obtain.
        /// </summary>
        public virtual string DependencyName
        {
            get
            {
                if (_dependancyName == null)
                {
                    lock(_dependancyNameLock)
                    {
                        if (_dependancyName == null)
                        {
                            _dependancyName = Assembly.GetAssembly(this.GetType()).FullName;
                        }
                    }
                }

                return _dependancyName;
            }
        }
        private string _dependancyName;
        private object _dependancyNameLock = new object();

        /// <summary>
        /// The collection of <see cref="DependencyName"/>s on which this Registrator depends.
        /// </summary>
        public virtual IEnumerable<string> Dependancies => null;

        /// <summary>
        /// Will be called when all <see cref="Dependancies"/> have been registered.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context">Used to discriminate on registration. Mainly for Solutions with multiple entry projects.</param>
        public abstract void Register(T container, string context = null);

    }
}
