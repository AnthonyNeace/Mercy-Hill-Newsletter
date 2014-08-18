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

namespace MercyHillNewsletter.UserInterface
{
    public partial class MainForm : Form
    {
        private NameValueCollection AppSettings { get; set; }
        private TextBoxLogger _logger { get; set; }
        private DocumentParser _parser { get; set; }

        public MainForm()
        {
            InitializeComponent();

            _logger = new TextBoxLogger(txtLog);
            _parser = new DocumentParser(webBrowser, _logger);

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
            _parser.TakeScreenshotsOfHtmlElements();

            txtLog.Refresh();

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

        private void MainForm_Load(object sender, EventArgs e)
        {
            writeToLog(@"Mercy Hill Newsletter Parser
Track latest updates and changes at https://github.com/AnthonyNeace/Mercy-Hill-Newsletter");
        }

        public void writeToLog(string log)
        {
            _logger.WriteMessage(log);
        }
    }
}
