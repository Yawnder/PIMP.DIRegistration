using PIMP.DIRegistration.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMP.DIRegistration.CoreTest.TestImplementation
{
    class RegistratorTest1 : Registrator<IContainer>
    {
        public static string ValidContext { get; set; }

        public static string Dependancy { get; set; }

        public override string DependencyName => nameof(RegistratorTest1);

        public override IEnumerable<string> GetDependancies(string context)
        {
            if (Dependancy != null)
                return new List<string>() { Dependancy };
            else
                return null;
        }

        public override void Register(IContainer container, string context = null)
        {
            if (ValidContext == null || ValidContext == context)
                container.Register(this.DependencyName);
        }
    }
}