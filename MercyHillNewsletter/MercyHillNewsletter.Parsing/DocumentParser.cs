using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using MercyHillNewsletter.Logging.Logger;

namespace MercyHillNewsletter.Parsing
{
    public class DocumentParser
    {
        private WebBrowser _webBrowser;
        private ILogger _logger;

        public DocumentParser(WebBrowser webBrowser, ILogger logger)
        {
            _webBrowser = webBrowser;
            _logger = logger;
        }

        public void TakeScreenshotsOfHtmlElements()
        {
            // WebBrowser initialization for hiding toolbars and Mailchimp's floating bar.  
            // Note: While we're still using the webBrowser control, we need to set the default width greater than
            // the width of the div so that it doesn't shrink or try to float the content.

            _webBrowser.ScrollBarsEnabled = false;
            _webBrowser.Document.Body.Style = "overflow:hidden";
            _webBrowser.Document.GetElementById("awesomebar").Style = "display:none";

            writeToLog(string.Format("Loading web content at {0}", _webBrowser.Url));

            // TODO: Move this to another thread.

            HtmlElementCollection elements = this._webBrowser.Document.Body.All;

            int counter = 0;

            writeToLog(@"Iterating HTML elements...");

            foreach (HtmlElement element in elements)
            {
                string nameAttribute = element.GetAttribute("className");

                if (!string.IsNullOrEmpty(nameAttribute) && nameAttribute == "mcnCaptionBlock")
                {
                    writeToLog(string.Format(@"Analyzing the {0} iterate of element {1}", counter, nameAttribute));

                    _webBrowser.Size = new Size(element.ClientRectangle.Width, element.ClientRectangle.Height);

                    // Scroll into view
                    element.ScrollIntoView(true);

                    // Take screenshot
                    Bitmap bitmap = new Bitmap(_webBrowser.Width, _webBrowser.Height);
                    _webBrowser.DrawToBitmap(bitmap, new Rectangle(0, 0, _webBrowser.Width, _webBrowser.Height));

                    // Save screenshot to dir
                    bitmap.Save(string.Format(@"C:\temp\imagetest-{0}-{1}.bmp", DateTime.Now.ToString("yyyyMMdd-mmHH"), counter));
                    counter++;
                }
            }

            writeToLog(@"HTML analysis complete.");
        }

        public void writeToLog(string log)
        {
            _logger.WriteMessage(log);
        }
    }
}
