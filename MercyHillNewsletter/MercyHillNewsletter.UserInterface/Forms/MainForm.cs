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
using MercyHillNewsletter.Slideshow;
using MercyHillNewsletter.Parsing.RSS;
using MercyHillNewsletter.Logging;

namespace MercyHillNewsletter.UserInterface
{
    public partial class MainForm : Form
    {
        private NameValueCollection AppSettings { get; set; }
        private NewsletterParser _parser { get; set; }

        private static TextBoxLogger _logger { get; set; }
        private static LogWriter _logWriter { get; set; }

        public MainForm()
        {
            InitializeComponent();

            _logger = new TextBoxLogger(txtLog);
            _logWriter = new LogWriter(_logger);
            _parser = new NewsletterParser(_logWriter);

            statusLabel.Text = "Click Newsletter > Slice Elements to begin.";
        }

        private void newsletterToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void sliceElementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshAppSettings();

            runBrowserThread(new Uri(AppSettings["NewsletterUrl"]));

            statusLabel.Text = "Click Export > To PowerPoint to continue.";
        }

        public void writeToLog(string log)
        {
            _logWriter.WriteMessage(log);
        }

        private void refreshAppSettings()
        {
            AppSettings = ConfigurationManager.AppSettings;
        }

        #region Newsletter Parsing

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

        private void getNewestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshAppSettings();

            FeedReader feedReader = new FeedReader(AppSettings["NewsletterFeed"]);

            Newsletter newsletter = feedReader.GetNewest();

            writeToLog(newsletter.Title);
            writeToLog(newsletter.Url);
            writeToLog(newsletter.PublishDate.ToString());
            //writeToLog(newsletter.Text);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["NewsletterUrl"].Value = newsletter.Url;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            writeToLog("Active NewsletterUrl reset to newest newsletter url.");
        }

        #endregion

        #region Logging

        private void MainForm_Load(object sender, EventArgs e)
        {
            writeToLog(@"Mercy Hill Newsletter Parser
Track latest updates and changes at https://github.com/AnthonyNeace/Mercy-Hill-Newsletter");


        }

        #endregion

        #region Export Images

        private void toPowerPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog filePicker = new OpenFileDialog();

            filePicker.Multiselect = true;

            DialogResult result = filePicker.ShowDialog();

            if (result == DialogResult.OK) // Test result.
            {
                List<string> images = filePicker.FileNames.ToList();
                //                List<string> test = new List<string>(
                PowerpointExporter test = new PowerpointExporter();

                test.ExportToPowerpoint(images);
            }
        }

        #endregion

    }
}
