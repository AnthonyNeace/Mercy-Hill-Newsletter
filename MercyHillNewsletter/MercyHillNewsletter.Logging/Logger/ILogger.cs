using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MercyHillNewsletter.Logging.Logger
{
    public interface ILogger
    {
        void WriteMessage(string message);
        void WriteWarning(string message);
        void WriteError(string message);
    }
}
