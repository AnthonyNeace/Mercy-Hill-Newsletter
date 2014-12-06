using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace MercyHillNewsletter.Logging.Logger
{
    public class FileLogger : ILogger
    {
        private string _directory;
        private string _filepath;

        public FileLogger(string directory)
        {
            _directory = directory;

            // Current use case prefers that we generate a single file per run of the job.
            _filepath = string.Format(@"{0}NewsletterParser-{1}.log", directory, DateTime.UtcNow.ToString("yyyyMMdd-HHmm"));
        }

        public void WriteMessage(string message)
        {
            using (StringReader reader = new StringReader(message))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        string formattedLine = string.Format("{0}{1}", DateTime.UtcNow.ToString("yyyyMMdd-HHmm: "), line);

                        // TODO: This is really ugly.  Make it a bit more robust if you ever get more time to work on this.
                        int counter = 0;

                        while (counter < 30)
                        {
                            try
                            {
                                using (StreamWriter w = File.AppendText(_filepath))
                                {
                                    w.WriteLine(formattedLine);
                                }

                                counter = 30;
                            }
                            catch (IOException)
                            {
                                Console.WriteLine(string.Format("FileLogger is busy in another thread. Waiting for 1 second. Wait count: {0}", counter));

                                counter++;

                                //wait and retry
                                Thread.Sleep(1000);
                            }
                        }
                    }

                } while (line != null);
            }
        }

        public void WriteWarning(string message)
        {

        }

        public void WriteError(string message)
        {

        }
    }
}
