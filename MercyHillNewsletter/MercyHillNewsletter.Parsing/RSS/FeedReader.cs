using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ServiceModel.Syndication;

namespace MercyHillNewsletter.Parsing.RSS
{
    public class FeedReader
    {

        XmlReader _reader;
        SyndicationFeed _feed;

        public FeedReader()
        {

        }

        public FeedReader(string feedUrl)
        {
            loadFeed(feedUrl);
        }

        /// <summary>
        /// Pass in a feedUrl to load a feed for other methods to use.
        /// </summary>
        private void loadFeed(string feedUrl)
        {
            _reader = XmlReader.Create(feedUrl);
            _feed = SyndicationFeed.Load(_reader);
        }

        public Newsletter GetNewest()
        {
            // Return the newest item from the feed
            SyndicationItem item = _feed.Items.OrderByDescending(x => x.PublishDate).FirstOrDefault();

            if (item == null)
            {
                return null;
            }

            return new Newsletter(item);
        }
    }
}
