//Notification Tray adapted from http://www.codeproject.com/Articles/290013/Formless-System-Tray-Application

using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using LCDPihole;
using LogiFrame;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LCDPlay
{
    /// <summary>
    /// 
    /// </summary>
    class ProcessIcon : IDisposable
    {

      


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

            ni.Icon = Resource.RedBerry;
            ni.Text = "Pi-hole stats for Logitech LCDs!";
            ni.Visible = true;
            
            // Attach a context menu.
            

            var p = new PiHoleApi();
            ni.ContextMenuStrip = new ContextMenus().Create();
            //  p.WaitForClose();


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