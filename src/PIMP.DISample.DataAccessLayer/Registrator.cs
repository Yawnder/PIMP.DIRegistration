using PIMP.DIRegistration.Core;
using PIMP.DISample.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace PIMP.DISample.DataAccessLayer
{
    class Registrator : UnityRegistrator
    {

        public override IEnumerable<string> GetDependancies(string context)
        {
            return new List<string>() { "Settings" };
        }

        public override void Register(IUnityContainer container, string context = null)
        {
            container.RegisterType<IRepository<Potato>, Repository<Potato>>(new HierarchicalLifetimeManager());

            container.RegisterType<IDbContext<Potato>, DbContext<Potato>>(
                new HierarchicalLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<string>("FooConnectionString")));

            var otherString = container.Resolve<string>("OtherString");
            Console.WriteLine($"Not sure why, but I needed that NOW: {otherString}");
        }

    }
}
