using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Cvb;

namespace i_2_final
{
    public partial class home_screen : Form
    {
        public web web = null;
        public ppt ppt_ = null;
        public home_screen()
        {
            InitializeComponent();
            global.capture = new Capture();
            web = new web();

            //Start Threads
            Thread battery = new Thread(new ThreadStart(battery_status));
            battery.Start();
            Thread dt = new Thread(new ThreadStart(datetime_m));
            dt.Start();
            Thread re = new Thread(new ThreadStart(reciever_m));
            //re.Start();

            //Thread mouse = new Thread(new ThreadStart(mouse_start));
            //mouse.Start();
            
            /*
            Thread gesture = new Thread(new ThreadStart(gesture_start));
            gesture.Start();
            */
        }


        #region DateTime
        // Battery status
        public void datetime_m()
        {
            while (true)
            {
                string temp = "" + DateTime.Now;
                ThreadHelperClass.SetText(this, datetime, temp);
                Thread.Sleep(1000);
            }
        }
        #endregion

        #region Battery Status
        // Battery status
        public void battery_status()
        {
            while (true)
            {
                PowerStatus p = SystemInformation.PowerStatus;
                int a = (int)(p.BatteryLifePercent * 100);
                //MessageBox.Show("" + a);
                ThreadHelperClass.SetText(this, batter_status , "Battery Status: " + a +"%");
                Thread.Sleep(1000);
                if (global.security_fag == 1)
                    ThreadHelperClass.SetText(this, security, "Connected");
                else
                    ThreadHelperClass.SetText(this, security, "Disconnected");
                Thread.Sleep(1000);
            }
        }
        #endregion

        public void mouse_start()
        {
            mouse m = new mouse();
            m.Show();
        }

        public void gesture_start()
        {
            gesture g = new gesture();
            g.Show();
        }

        public void reciever_m()
        {
            reciever r = new reciever();
            r.Show();
        }

        #region Control Access
        // To access controls from threads
        public static class ThreadHelperClass
        {
            delegate void SetTextCallback(Form f, Control ctrl, string text);
  
            public static void SetText(Form form, Control ctrl, string text)
            {
                // InvokeRequired required compares the thread ID of the 
                // calling thread to the thread ID of the creating thread. 
                // If these threads are different, it returns true. 
                if (ctrl.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    form.Invoke(d, new object[] { form, ctrl, text });
                }
                else
                {
                    ctrl.Text = text;
                }
            }
        }
        #endregion

        private void desktop_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                foreach (Form frm in Application.OpenForms)
                {
                    //frm.WindowState = FormWindowState.Minimized;
                }
            }
        }

        private void alarm_Click(object sender, EventArgs e)
        {
            alarm al = new alarm();
            al.Show();
        }

        private void icon1_Click(object sender, EventArgs e)
        {
            camera c = new camera();
            c.Show();
        }

        private void icon4_Click(object sender, EventArgs e)
        {
            paint p = new paint();
            p.Show();
        }

        private void settings_Click(object sender, EventArgs e)
        {
            settings s = new settings();
            s.Show();
        }

        private void icon7_Click(object sender, EventArgs e)
        {
            ppt_ = new ppt();
            ppt_.Show();
        }

        private void fb_Click(object sender, EventArgs e)
        {
            web.Show();
            web.open("https://www.facebook.com/", "facebook");
        }

        private void gmail_Click(object sender, EventArgs e)
        {
            web.Show();
            web.open("https://mail.google.com/", "Gmail");
        }

        private void linkden_Click(object sender, EventArgs e)
        {
            web.Show();
            web.open("https://www.linkedin.com/", "linkedIn");
        }

        private void google_search_Click(object sender, EventArgs e)
        {
            web.Show();
            web.open("https://www.google.co.in/?gfe_rd=cr&ei=i1xuV5XRM8iM8QfHxLmoDw", "google");
        }

        private void icon2_Click(object sender, EventArgs e)
        {
            music_player mp = new music_player();
            mp.Show();
        }

        private void icon8_Click(object sender, EventArgs e)
        {
            photos ph = new photos();
            ph.Show();
        }

        private void icon5_Click(object sender, EventArgs e)
        {
            pdf pd = new pdf();
            pd.Show();
        }
    
    }
}
