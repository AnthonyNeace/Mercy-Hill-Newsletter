using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MercyHillNewsletter.ProcessNewest.Job
{
    class Program
    {
        static void Main(string[] args)
        {
            NewsletterManager manager = new NewsletterManager();

            manager.GetNewest();

            manager.SliceElements();
        }
    }
}
