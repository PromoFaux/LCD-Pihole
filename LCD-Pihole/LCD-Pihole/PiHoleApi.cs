using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogiFrame;
using Newtonsoft.Json.Linq;
using Timer = System.Threading.Timer;

namespace LCDPihole
{
    public class PiHoleApi
    {
        private readonly LCDApp _app;
        private readonly LCDLabel _lblTotDomains;
        private readonly LCDLabel _lblQueriesToday;
        private readonly LCDLabel _lblBlockedToday;
        private readonly LCDLabel _lblPercToday;

        private Timer _t;

        public PiHoleApi()
        {
            _app = new LCDApp("Pi-hole Stats", false, false, false);
           // _app.WaitForClose();
            _app.PushToForeground();

            var lineVer = new LCDLine
            {
                Start = new Point((LCDApp.DefaultSize.Width / 2) - 1, 0),
                End = new Point((LCDApp.DefaultSize.Width / 2) - 1, LCDApp.DefaultSize.Height - 1)
            };

            var lineHor = new LCDLine
            {
                Start = new Point(0, (LCDApp.DefaultSize.Height / 2) - 1),
                End = new Point(LCDApp.DefaultSize.Width - 1, (LCDApp.DefaultSize.Height / 2) - 1)
            };

            _lblTotDomains = new LCDLabel
            {
                Font = PixelFonts.Small,
                AutoSize = true

            };
            _lblQueriesToday = new LCDLabel
            {
                Font = PixelFonts.Small,
                AutoSize = true

            };
            _lblBlockedToday = new LCDLabel
            {
                Font = PixelFonts.Small,
                AutoSize = true

            };
            _lblPercToday = new LCDLabel
            {
                Font = PixelFonts.Small,
                AutoSize = true
            };

            // Create an app instance.


            // Add the label control to the app.
            _app.Controls.Add(lineVer);
            _app.Controls.Add(lineHor);
            _app.Controls.Add(_lblTotDomains);
            _app.Controls.Add(_lblQueriesToday);
            _app.Controls.Add(_lblBlockedToday);
            _app.Controls.Add(_lblPercToday);

            UpdateVisibility();

            UpdateStats();


            TimerCallback tDelegate = TimerCallback;
            _t = new Timer(tDelegate, null, 0, 5000);
            
        }

        public void WaitForClose()
        {
            _app.WaitForClose();
        }

        private void UpdateVisibility()
        {
            _app.UpdatePriority = UpdatePriority.Normal;
        }

        private void UpdateStats()
        {
            using (var wc = new WebClient())
            {
                var json = wc.DownloadString("http://pi.hole/admin/api.php?summaryRaw");

                var j = JObject.Parse(json);

                var domainsBlocked = j["domains_being_blocked"].ToString();
                var queriesToday = j["dns_queries_today"].ToString();
                var blockedToday = j["ads_blocked_today"].ToString();
                var percToday = Math.Round(Convert.ToDecimal(j["ads_percentage_today"].ToString()), 2).ToString();
               
                //Set Labels and centre top line of each with bottom line.
                _lblTotDomains.Text = $"{CenterIt(domainsBlocked,15)}\nDomains Blocked";
                _lblBlockedToday.Text = $"{CenterIt(blockedToday,13)}\nBlocked Today";
                _lblQueriesToday.Text = $"{CenterIt(queriesToday,13)}\nQueries Today";
                _lblPercToday.Text = $"{CenterIt($"{percToday}%",10)}\nPercentage";

                //center labels horizontally
                _lblTotDomains.Left = (LCDApp.DefaultSize.Width / 4) - (_lblTotDomains.Width / 2);
                _lblBlockedToday.Left = ((LCDApp.DefaultSize.Width / 4) * 3) - _lblBlockedToday.Width / 2;
                _lblQueriesToday.Left = (LCDApp.DefaultSize.Width / 4) - (_lblQueriesToday.Width / 2);
                _lblPercToday.Left = ((LCDApp.DefaultSize.Width / 4) * 3) - _lblPercToday.Width / 2;

                //center labels vertically
                _lblTotDomains.Top = (LCDApp.DefaultSize.Height / 4) - (_lblTotDomains.Height / 2);
                _lblBlockedToday.Top = (LCDApp.DefaultSize.Height / 4) - (_lblBlockedToday.Height / 2);
                _lblQueriesToday.Top = ((LCDApp.DefaultSize.Height / 4) * 3) - _lblQueriesToday.Height / 2;
                _lblPercToday.Top = ((LCDApp.DefaultSize.Height / 4) * 3) - _lblPercToday.Height / 2;

            }
            
        }

        /// <summary>
        /// Pad left and right to given length to center-justify string.
        /// </summary>
        /// <param name="inStr">String you want to center</param>
        /// <param name="length">Length of new string</param>
        /// <returns></returns>
        public string CenterIt(string inStr, int length)
        {
            var spaces = length - inStr.Length;
            var padLeft = spaces / 2 + inStr.Length;
            return inStr.PadLeft(padLeft).PadRight(length);
        }

        private void TimerCallback(object o)
        {

            UpdateStats();

        }



    }
}
