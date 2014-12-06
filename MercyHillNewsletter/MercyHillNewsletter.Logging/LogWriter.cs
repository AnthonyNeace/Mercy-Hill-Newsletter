using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MercyHillNewsletter.Logging.Logger;
using System.ComponentModel;

namespace MercyHillNewsletter.Logging
{
    public class LogWriter
    {
        ILogger Logger { get; set; }

        public LogWriter()
        {

        }

        public LogWriter(ILogger logger)
        {
            initialize(logger);
        }

        private void initialize(ILogger logger)
        {
            Logger = logger;
        }

        public void WriteMessage(string message)
        {
            BackgroundWorker logWorker = new BackgroundWorker();
            logWorker.DoWork += new DoWorkEventHandler(lw_WriteMessage);

            logWorker.RunWorkerAsync(message);
        }

        public void WriteMessageSynchronously(string message)
        {
            Logger.WriteMessage(message);
        }

        private void lw_WriteMessage(object sender, DoWorkEventArgs e)
        {
            string message = e.Argument as string;

            Logger.WriteMessage(message);
        }
    }
}
