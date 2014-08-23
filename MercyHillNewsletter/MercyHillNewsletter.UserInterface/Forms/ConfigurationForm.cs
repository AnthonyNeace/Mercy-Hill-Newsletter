using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;

namespace MercyHillNewsletter.UserInterface
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            txtNewsletterFeed.Text = appSettings["NewsletterFeed"];
            txtNewsletterUrl.Text = appSettings["NewsletterUrl"];
            txtWriteDirectory.Text = appSettings["WriteDirectory"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["NewsletterFeed"].Value = txtNewsletterFeed.Text;
            config.AppSettings.Settings["NewsletterUrl"].Value = txtNewsletterUrl.Text;
            config.AppSettings.Settings["WriteDirectory"].Value = txtWriteDirectory.Text;

            foreach (KeyValueConfigurationElement appSetting in config.AppSettings.Settings)
            {
                if (string.IsNullOrEmpty(appSetting.Value))
                {
                    MessageBox.Show(string.Format("Update failed because {0} was empty.", appSetting.Key));
                    return;
                }
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Update complete.  Changes will expire at the end of this session.");
        }
    }
}
