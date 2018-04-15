using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMP.DIRegistration.Core
{
    public class MissingDependencyException : InvalidOperationException
    {

        #region Fields & Properties

        public IEnumerable<IEnumerable<string>> MissingDependancies { get; }

        public override string Message
        {
            get
            {
                if (_message == null)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("All of the remaining Registrator are awaiting on one or more registrations.");

                    if (this.MissingDependancies != null)
                    {
                        sb.AppendLine("One of these collection is required to progress:");
                        foreach(var subCollection in this.MissingDependancies)
                            sb.AppendLine($"- {{'{string.Join("','", subCollection.OrderBy(v => v))}'}}");

                        sb.Length--;

                        _message = sb.ToString();
                    }
                }

                return _message;
            }
        }
        private string _message;

        #endregion

        #region Constructor

        public MissingDependencyException(IEnumerable<IEnumerable<string>> missingDependancies = null)
        {
            if (missingDependancies != null && missingDependancies.Count() == 0)
                throw new ArgumentException("Can either be null, or non-empty.", nameof(missingDependancies));

            this.MissingDependancies = missingDependancies;
        }

        #endregion

    }
}
