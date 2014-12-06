using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MercyHillNewsletter.Logging.Logger;
using MercyHillNewsletter.Logging;
using System.Collections.Specialized;
using System.Configuration;
using MercyHillNewsletter.Parsing;
using MercyHillNewsletter.Parsing.RSS;

namespace MercyHillNewsletter.ProcessNewest.Job
{
    public class NewsletterManager
    {
        private NameValueCollection AppSettings { get; set; }
        private NewsletterParser _parser { get; set; }

        private static FileLogger _logger { get; set; }
        private static LogWriter _logWriter { get; set; }

        public NewsletterManager()
        {
            AppSettings = ConfigurationManager.AppSettings;

            string logDirectory = AppSettings["WriteDirectory"];
            string imageDirectory = string.Format(@"{0}{1}\", AppSettings["ImageDirectory"], DateTime.UtcNow.ToString("yyyyMMdd"));

            System.IO.Directory.CreateDirectory(logDirectory);
            System.IO.Directory.CreateDirectory(imageDirectory);

            _logger = new FileLogger(AppSettings["WriteDirectory"]);
            _logWriter = new LogWriter(_logger);
            _parser = new NewsletterParser(_logWriter, imageDirectory);            
        }

        private void refreshAppSettings()
        {
            AppSettings = ConfigurationManager.AppSettings;
        }

        private void writeToLog(string log)
        {
            _logWriter.WriteMessageSynchronously(log);
        }

        public void GetNewest()
        {
            refreshAppSettings();

            FeedReader feedReader = new FeedReader(AppSettings["NewsletterFeed"]);

            Newsletter newsletter = feedReader.GetNewest();

            writeToLog(newsletter.Title);
            writeToLog(newsletter.Url);
            writeToLog(newsletter.PublishDate.ToString());

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["NewsletterUrl"].Value = newsletter.Url;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            writeToLog("Active NewsletterUrl reset to newest newsletter url.");
        }

        public void SliceElements()
        {
            _parser.TakeScreenshotsOfHtmlElements(new Uri(AppSettings["NewsletterUrl"]));
        }

    }
}
