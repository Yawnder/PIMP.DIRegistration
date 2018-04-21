using PIMP.DIRegistration.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PIMP.DISample.MyConsole
{
    class Registrator : UnityRegistrator
    {

        public override string DependencyName => "Settings";

        public override void Register(IUnityContainer container, string context = null)
        {
            container.RegisterInstance<string>("SecretPassword", Settings.Default.SecretPassword);
            container.RegisterInstance<string>("FooConnectionString", Settings.Default.FooConnectionString);
            container.RegisterInstance<string>("OtherString", Settings.Default.OtherString);
        }

    }
}
