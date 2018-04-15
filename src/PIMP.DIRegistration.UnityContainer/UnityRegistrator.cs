using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;
using Unity.RegistrationByConvention;

namespace PIMP.DIRegistration.Core
{
    public abstract class UnityRegistrator : Registrator<IUnityContainer>, IRegistrator<IUnityContainer>
    {

        /// <summary>
        /// Registers all interfaces matching classes using HierarchicalLifetimeManager.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context">Used to discriminate on registration. Mainly for Solutions with multiple entry projects.</param>
        public override void Register(IUnityContainer container, string context = null)
        {
            container.RegisterTypes(
                AllClasses.FromAssemblies(Assembly.GetCallingAssembly()),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                (t) => { return new HierarchicalLifetimeManager(); });
        }
    }
}
