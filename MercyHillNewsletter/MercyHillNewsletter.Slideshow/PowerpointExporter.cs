using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MercyHillNewsletter.Slideshow
{
    public class PowerpointExporter
    {
        public PowerpointExporter()
        {

        }

        public void ExportToPowerpoint(List<string> images)
        {
            Application app = openPowerPoint();

            Presentation pre = openPresentation(app);

            foreach (var image in images)
            {
                AddSlide(pre, image);
            }

            savePresentation(pre);

            //killPowerPoint(app, pre);

            pre = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            // TODO: Investigate occasion problems with PPT not closing.
            // Things seem to be okay so long as I release pre and quit the application here.
            app.Quit();

            // It's dead, Jim.
            // TODO: This should be configurable... this will kill all running instances of PPT.
            //killProcess();
        }

        #region #Private Helper Methods

        private Application openPowerPoint()
        {
            // Create a PowerPoint application object.
            Application appPPT = new Application();

            return appPPT;
        }

        private Presentation openPresentation(Application appPPT)
        {
            // Create a new PowerPoint presentation.
            Presentation pptPreso = appPPT.Presentations.Add();

            return pptPreso;
        }

        private Slide AddSlide(Presentation pptPreso, string image)
        {
            CustomLayout pptLayout = default(CustomLayout);
            if ((pptPreso.SlideMaster.CustomLayouts._Index(7) == null))
            {
                pptLayout = pptPreso.SlideMaster.CustomLayouts._Index(1);
            }
            else
            {
                pptLayout = pptPreso.SlideMaster.CustomLayouts._Index(7);
            }

            // Create newSlide by using pptLayout.
            Slide newSlide =
                pptPreso.Slides.AddSlide((pptPreso.Slides.Count + 1), pptLayout);

            Image img = Image.FromFile(image);

            // Prepare to center the image.
            float sldHeight = pptPreso.PageSetup.SlideHeight;
            float sldWidth = pptPreso.PageSetup.SlideWidth;

            var left = sldWidth * .5f - (float)img.Width * .5f;
            var top = sldHeight * .5f - (float)img.Height * .5f;

            // Finally, add the picture... centered in the frame.
            newSlide.Shapes.AddPicture(image, MsoTriState.msoTrue, MsoTriState.msoTrue, left, top, (float)img.Width, (float)img.Height);

            return newSlide;
        }

        private void savePresentation(Presentation presentation)
        {
            string fileName = string.Format(@"C:\temp\newsletter-{0}.pptx", DateTime.Now.ToString("yyyyMMdd-mmHH"));

            presentation.SaveAs(fileName,
                PpSaveAsFileType.ppSaveAsOpenXMLPresentation,
                MsoTriState.msoTriStateMixed);
            presentation.Close();
        }

        private void killProcess()
        {
            Process[] processes = Process.GetProcessesByName("POWERPNT");
            for (int i = 0; i < processes.Count(); i++)
            {
                if (processes[i].ProcessName.ToLower().Contains("powerpnt"))
                {
                    processes[i].Kill();
                }
            }
        }

        #endregion

    }
}
