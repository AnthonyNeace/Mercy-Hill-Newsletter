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
    public class NewsletterParser
    {
        protected WebBrowser wb {get; set;}
        private ILogger _logger;

        public NewsletterParser(ILogger logger)
        {
            _logger = logger;
        }

        public void TakeScreenshotsOfHtmlElements(WebBrowser br)
        {
            wb = br;

            initializeWebBrowser();

            writeToLog(string.Format("Loading web content at {0}", wb.Url));

            HtmlElementCollection elements = this.wb.Document.Body.All;

            writeToLog(@"Iterating HTML elements...");

            iterateHtmlElements(elements);

            writeToLog(@"HTML analysis complete.");
        }

        #region Private Helper Methods

        private void initializeWebBrowser()
        {
            wb.Height = 600;
            wb.Width = 800;
            wb.ScrollBarsEnabled = false;
            wb.Document.Body.Style = "overflow:hidden";
            wb.Document.GetElementById("awesomebar").Style = "display:none";
        }

        private void iterateHtmlElements(HtmlElementCollection elements)
        {
            int counter = 0;

            foreach (HtmlElement element in elements)
            {
                string nameAttribute = element.GetAttribute("className");

                if (!string.IsNullOrEmpty(nameAttribute) && nameAttribute == "mcnCaptionBlock")
                {
                    writeToLog(string.Format(@"Analyzing the {0} iterate of element {1}", counter, nameAttribute));

                    wb.Size = new Size(element.ClientRectangle.Width, element.ClientRectangle.Height);

                    // Scroll into view
                    element.ScrollIntoView(true);

                    takeScreenshot(counter);

                    counter++;
                }
            }
        }

        private void takeScreenshot(int counter)
        {
            // Take screenshot
            Bitmap bitmap = new Bitmap(wb.Width, wb.Height);
            wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));

            // Save screenshot to dir
            string fileName = getDefaultFileName(counter);

            bitmap.Save(fileName);

            writeToLog(string.Format(@"Screenshot saved at {0}", fileName));
        }

        private string getDefaultFileName(int counter)
        {
            return string.Format(@"C:\temp\newsletter-{0}-{1}.bmp", DateTime.Now.ToString("yyyyMMdd-mmHH"), counter);
        }

        private void writeToLog(string log)
        {
            _logger.WriteMessage(log);
        }

        #endregion
    }
}
