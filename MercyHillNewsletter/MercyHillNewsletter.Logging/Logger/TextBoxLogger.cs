using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MercyHillNewsletter.Logging.Logger
{
    public class TextBoxLogger : ILogger
    {
        private TextBox _txtLog;

        public TextBoxLogger(TextBox txtLog)
        {
            _txtLog = txtLog;
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
                        _txtLog.Text += string.Format("{0}{1}{2}", DateTime.UtcNow.ToString("yyyyMMdd-HHmm: "), line, Environment.NewLine);
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
