using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LogiFrame;
using Newtonsoft.Json.Linq;

namespace LCDPihole
{
    class PiHoleApi
    {

        public PiHoleApi()
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("http://pi.hole/admin/api.php?summaryRaw");

                JObject j = JObject.Parse(json);

                var label = new LCDLabel
                {
                    Font = PixelFonts.Small, // The PixelFonts class contains various good fonts for LCD screens.
                    Text = json,
                    AutoSize = true,
                };
                var tabPage = new LCDTabPage
                {
                    Icon = new LCDLabel
                    {
                        AutoSize = true,
                        Text = "A",
                        Font = PixelFonts.Title
                    }
                };
                tabPage.Controls.Add(label);


                var tabPage2 = new LCDTabPage
                {
                    Icon = new LCDLabel
                    {
                        AutoSize = true,
                        Text = "B",
                        Font = PixelFonts.Title
                    }
                };


                var tabControl = new LCDTabControl();
                tabControl.TabPages.Add(tabPage);
                tabControl.TabPages.Add(tabPage2);

                tabControl.SelectedTab = tabPage;



                // Create an app instance.
                var app = new LCDApp("Sample App", false, false, false);

                // Add the label control to the app.
                app.Controls.Add(tabControl);
                // Make the app the foreground app on the LCD screen.
                app.PushToForeground();

                // A blocking call. Waits for the LCDApp instance to be disposed. (optional)
                app.WaitForClose();
            }


        }
    }
}
