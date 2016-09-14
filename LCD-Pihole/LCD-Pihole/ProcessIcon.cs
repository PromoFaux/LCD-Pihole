//Notification Tray adapted from http://www.codeproject.com/Articles/290013/Formless-System-Tray-Application

using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using LCDPihole;
using LogiFrame;
using Timer = System.Threading.Timer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LCDPlay
{
    /// <summary>
    /// 
    /// </summary>
    class ProcessIcon : IDisposable
    {

        private Timer t;


        /// <summary>
        /// The NotifyIcon object.
        /// </summary>
        NotifyIcon ni;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessIcon"/> class.
        /// </summary>
        public ProcessIcon()
        {
            // Instantiate the NotifyIcon object.
            ni = new NotifyIcon();
        }

        /// <summary>
        /// Displays the icon in the system tray.
        /// </summary>
        public void Display()
        {
            // Put the icon in the system tray and allow it react to mouse clicks.			

            ni.Icon = Resource.favicon_pIH_icon;
            ni.Text = "Pi-hole stats for Logitech LCDs!";
            ni.Visible = true;

            // Attach a context menu.
            ni.ContextMenuStrip = new ContextMenus().Create();


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






        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            // When the application closes, this will remove the icon from the system tray immediately.
            ni.Dispose();
        }


    }
}