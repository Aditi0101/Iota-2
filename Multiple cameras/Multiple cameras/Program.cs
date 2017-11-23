using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
namespace Multiple_cameras
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
           

         //   Thread speech = new Thread((ThreadStart)delegate { Application.Run(new speech()); });
         //   speech.Start();
           Thread main = new Thread((ThreadStart)delegate { Application.Run(new Main()); });
             main.Start();

           // Application.Run(new Main());

        }
    }
}
