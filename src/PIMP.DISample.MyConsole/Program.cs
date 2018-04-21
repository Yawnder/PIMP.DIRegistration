using PIMP.DIRegistration.Core;
using PIMP.DISample.BusinessProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace PIMP.DISample.MyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();

            RegistrationCensor<IUnityContainer>.RegisterAll(container, "PIMP.DISample.*");

            var potatoExtractor = container.Resolve<IPotatoExtractor>();
            int count = potatoExtractor.CountPotatoYield();
            Console.WriteLine($"Just extracted {count} potatos!");
            Console.ReadLine();
        }
    }
}
