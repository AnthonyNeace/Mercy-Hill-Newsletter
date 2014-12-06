using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using MercyHillNewsletter.Logging.Logger;
using MercyHillNewsletter.Logging;
using System.Threading;

namespace MercyHillNewsletter.Parsing
{
    public class NewsletterParser
    {
        protected WebBrowser wb {get; set;}
        private LogWriter _logWriter;

        public NewsletterParser(LogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        public void TakeScreenshotsOfHtmlElements(Uri url)
        {
            runBrowserThread(url);
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

        private void runBrowserThread(Uri url)
        {
            // Courtesy of http://stackoverflow.com/questions/4269800/webbrowser-control-in-a-new-thread
            // You must create a STA thread to use a WebBrowser (or similar ActiveX component). 
            var th = new Thread(() =>
            {
                var br = new WebBrowser();
                br.DocumentCompleted += browser_DocumentCompleted;
                br.Navigate(url);
                Application.Run();
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var br = sender as WebBrowser;

            TakeScreenshotsOfHtmlElements(br);

            Application.ExitThread();
        }

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

                if (!string.IsNullOrEmpty(nameAttribute) && nameAttribute == "mcnCaptionBlockInner"/*"mcnCaptionBlock"*/)
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
            _logWriter.WriteMessage(log);
        }

        #endregion
    }
}
