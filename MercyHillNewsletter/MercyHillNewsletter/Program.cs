using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MercyHillNewsletter.UserInterface;

namespace MercyHillNewsletterParser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // TODO: Quit using WinForms so you can make this a scheduled weekly job...

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
