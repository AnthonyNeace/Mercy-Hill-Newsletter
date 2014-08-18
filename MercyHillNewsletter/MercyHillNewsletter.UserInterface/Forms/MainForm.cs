using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MercyHillNewsletter.UserInterface;
using System.Collections.Specialized;
using System.Configuration;

namespace MercyHillNewsletter.UserInterface
{
    public partial class MainForm : Form
    {
        private NameValueCollection AppSettings { get; set; }

        public MainForm()
        {
            InitializeComponent();
            statusLabel.Text = "Click Images > Create slideshow to begin.";
        }

        private void imageHandlingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Make a custom configuration class...
            // TODO: Set the default url as the most recent newsletter according to MailChimp's RSS feed

            ConfigurationForm frm = new ConfigurationForm();

            frm.Show(this);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // WebBrowser initialization for hiding toolbars and Mailchimp's floating bar.  
            // Note: While we're still using the webBrowser control, we need to set the default width greater than
            // the width of the div so that it doesn't shrink or try to float the content.

            webBrowser.ScrollBarsEnabled = false;
            webBrowser.Document.Body.Style = "overflow:hidden";
            webBrowser.Document.GetElementById("awesomebar").Style = "display:none";
            //MessageBox.Show("Loaded!");

            statusLabel.Text = "Loading webpage...";

            // TODO: Move this to another thread.

            HtmlElementCollection elements = this.webBrowser.Document.Body.All;

            int counter = 0;

            foreach (HtmlElement element in elements)
            {
                statusLabel.Text = "Analyzing web content...";

                string nameAttribute = element.GetAttribute("className");
                //if (!string.IsNullOrEmpty(nameAttribute) && nameAttribute == "mcnDividerBlockInner")
                if (!string.IsNullOrEmpty(nameAttribute) && nameAttribute == "mcnCaptionBlock")
                {
                    webBrowser.Size = new Size(element.ClientRectangle.Width, element.ClientRectangle.Height);

                    // Scroll into view
                    element.ScrollIntoView(true);

                    // Take screenshot
                    Bitmap bitmap = new Bitmap(webBrowser.Width, webBrowser.Height);
                    webBrowser.DrawToBitmap(bitmap, new Rectangle(0, 0, webBrowser.Width, webBrowser.Height));



                    bitmap.Save(string.Format(@"C:\temp\imagetest-{0}-{1}.bmp", DateTime.Now.ToString("yyyyMMdd-mmHH"), counter));
                    counter++;

                    // Save screenshot to dir

                    // Continue
                    //break;
                }
            }

            statusLabel.Text = "Operation complete.";

            // TODO: Use gmail API to email images to whoever needs them.
        }

        private void createSlideshowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshAppSettings();

            webBrowser.Navigate(AppSettings["NewsletterUrl"]);
        }

        private void refreshAppSettings()
        {
            AppSettings = ConfigurationManager.AppSettings;
        }
    }
}
