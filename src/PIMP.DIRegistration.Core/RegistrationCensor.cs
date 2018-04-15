using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PIMP.DIRegistration.Core
{
    public class RegistrationCensor<T>
    {

        #region Fields & Properties

        private static RegistrationCensor<T> instance { get; set; }

        private Dictionary<IRegistrator<T>, List<string>> queuedRegistrators { get; set; }
        private List<string> processedRegistrators { get; set; }

        #endregion

        #region Constructor

        private RegistrationCensor()
        {
            this.queuedRegistrators = new Dictionary<IRegistrator<T>, List<string>>();
            this.processedRegistrators = new List<string>();
        }

        #endregion

        #region Methods: Public Static

        public static void RegisterAll(T container, string context = null, string registratorInstance = null)
        {
            if (instance == null)
                instance = new RegistrationCensor<T>();

            instance.EnqueueAll(container, context);
            instance.ProcessAll(container);
        }

        #endregion

        #region Methods: Private

        private void EnqueueAll(T container, string context = null)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
                Register(container, assembly, context);
        }

        private void ProcessAll(T container, string context = null)
        {

            while (true)
            {
                bool hasRegistered = false;

                foreach (var registratorKvp in queuedRegistrators)
                {
                    bool canRegister = true;

                    var registrator = registratorKvp.Key;
                    var dependancies = registratorKvp.Value;

                    if (dependancies != null)
                    {
                        for (int i = 0; i< dependancies.Count; i++)
                        {
                            var dependancy = dependancies[i];

                            if (processedRegistrators.Contains(dependancy))
                            {
                                dependancies.Remove(dependancy);
                                i--;
                            }
                            else
                            {
                                canRegister = false;
                                break;
                            }
                        }
                    }

                    if (canRegister)
                    {
                        registrator.Register(container, context);
                        hasRegistered = true;

                        var dependancyName = registrator.DependencyName;
                        if (!string.IsNullOrWhiteSpace(dependancyName))
                            processedRegistrators.Add(dependancyName);

                        queuedRegistrators.Remove(registrator);
                        break;
                    }
                }

                if (queuedRegistrators.Count == 0)
                    break;

                if (!hasRegistered)
                    throw new MissingDependencyException(queuedRegistrators.Values);
            }
        }

        #endregion

        #region Methods: Private Static

        private void Register(T container, Assembly assembly, string context)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
                Register(container, type, context);
        }

        private void Register(T container, Type type, string context)
        {
            if (typeof(IRegistrator<T>).IsAssignableFrom(type) && type != typeof(IRegistrator<T>) && !type.IsAbstract)
            {
                if (!string.IsNullOrWhiteSpace(context))
                    throw new NotImplementedException();

                var registrator = (IRegistrator<T>)Activator.CreateInstance(type);
                queuedRegistrators.Add(registrator, registrator.Dependancies?.ToList());
            }
        }

        #endregion

    }
}
