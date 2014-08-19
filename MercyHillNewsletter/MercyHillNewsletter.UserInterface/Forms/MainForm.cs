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
using System.IO;
using MercyHillNewsletter.Logging.Logger;
using MercyHillNewsletter.Parsing;
using System.Threading;

namespace MercyHillNewsletter.UserInterface
{
    public partial class MainForm : Form
    {
        private NameValueCollection AppSettings { get; set; }
        private static TextBoxLogger _logger { get; set; }
        private NewsletterParser _parser { get; set; }

        // threads
        private BackgroundWorker _lw { get; set; }

        public MainForm()
        {
            InitializeComponent();

            _lw = new BackgroundWorker();
            _lw.DoWork += new DoWorkEventHandler(lw_DoWork); 

            _logger = new TextBoxLogger(txtLog);
            _parser = new NewsletterParser(_logger);

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

        private void createSlideshowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshAppSettings();

            runBrowserThread(new Uri(AppSettings["NewsletterUrl"]));
        }

        private void refreshAppSettings()
        {
            AppSettings = ConfigurationManager.AppSettings;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            writeToLog(@"Mercy Hill Newsletter Parser
Track latest updates and changes at https://github.com/AnthonyNeace/Mercy-Hill-Newsletter");
        }

        public void writeToLog(string log)
        {
            _lw.RunWorkerAsync(log);
        }

        #region Threads

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

            _parser.TakeScreenshotsOfHtmlElements(br);

            Application.ExitThread();
        }

        private void lw_DoWork(object sender, DoWorkEventArgs e)
        {
            string message = e.Argument as string;

            _logger.WriteMessage(message);
        }

        #endregion
    }
}
