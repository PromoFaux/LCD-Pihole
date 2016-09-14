using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogiFrame;
using LogiFrame.Drawing;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;


namespace LCDPlay
{
    class Program
    {
        static void Main(string[] args)
        {

            using (ProcessIcon pi = new ProcessIcon())
            {
                pi.Display();

                // Make sure the application runs!
                Application.Run();


            }

           

            // Show the system tray icon.
   

        }
    }
}
