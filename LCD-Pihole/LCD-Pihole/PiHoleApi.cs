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
                AutoSize = true,
                Top = 0,
                Left = 0,
                Width = (LCDApp.DefaultSize.Width / 2) - 1

            };
            _lblQueriesToday = new LCDLabel
            {
                Font = PixelFonts.Small,
                AutoSize = true,
                Top = LCDApp.DefaultSize.Height / 2,
                Left = 0

            };
            _lblBlockedToday = new LCDLabel
            {
                Font = PixelFonts.Small,
                AutoSize = true,
                Top = 0,
                Left = LCDApp.DefaultSize.Width / 2

            };
            _lblPercToday = new LCDLabel
            {
                Font = PixelFonts.Small,
                AutoSize = true,
                Top = LCDApp.DefaultSize.Height / 2,
                Left = LCDApp.DefaultSize.Width / 2

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

                var DomainsBlocked = j["domains_being_blocked"].ToString();
                var QueriesToday = j["dns_queries_today"].ToString();
                var BlockedToday = j["ads_blocked_today"].ToString();
                var PercToday = Math.Round(Convert.ToDecimal(j["ads_percentage_today"].ToString()), 2).ToString();

                _lblTotDomains.Text = $"{DomainsBlocked}\nDomains Blocked";
                _lblBlockedToday.Text = $"{BlockedToday}\nBlocked Today";
                _lblQueriesToday.Text = $"{QueriesToday}\nQueries Today";
                _lblPercToday.Text = $"{PercToday}%\nPercentage";
            }

            
        }

        private void TimerCallback(object o)
        {

            UpdateStats();

        }



    }
}
