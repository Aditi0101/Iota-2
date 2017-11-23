using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace i_2_final
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread mouse = new Thread((ThreadStart)delegate { Application.Run(new home_screen()); });
            mouse.SetApartmentState(ApartmentState.STA);
            mouse.Start();
            
            Thread ges = new Thread((ThreadStart)delegate { Application.Run(new gesture()); });
            ges.Start();
            //Thread rec = new Thread((ThreadStart)delegate { Application.Run(new mouse()); });
            //rec.Start();
           /* MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show("Do you want to connect to your home server", "", buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Run(new reciever());
                global.security_fag = 1;
            }
            else
                global.security_fag = 0;
            */
        }
    }
}
