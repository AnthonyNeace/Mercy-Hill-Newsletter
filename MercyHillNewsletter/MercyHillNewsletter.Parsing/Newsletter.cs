using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Syndication;

namespace MercyHillNewsletter.Parsing
{
    public class Newsletter
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTimeOffset PublishDate { get; set; }

        public Newsletter()
        {

        }

        public Newsletter(SyndicationItem item)
        {
            this.Url = item.Id;
            this.Title = item.Title.Text;
            this.Text = item.Summary.Text;
            this.PublishDate = item.PublishDate;
        }
    }
}
