using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace i_2_final
{
    public partial class web : Form
    {
        public static Thread ges;
        public web()
        {
            InitializeComponent();
            //Gesteur Listener
            ges = new Thread(new ThreadStart(ges_listen));
            ges.Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            Form log = new Form();
            this.Close();

        }
        public void open(String str, String s)
        {
            TabPage tabpage = new TabPage();
            tabpage.Text = s;
            tabControl1.Controls.Add(tabpage);
            WebBrowser webbrowser = new WebBrowser();
            webbrowser.Parent = tabpage;
            webbrowser.Dock = DockStyle.Fill;
            webbrowser.Navigate(str);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
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

        public void ges_listen()
        {
            while (true)
            {
                if (global.gesture)
                {
                   /* if (global.gesture_number == 3)
                    {
                        Thread.Sleep(2000);
                        global.gesture_number = 0;
                    }
                    else if (global.gesture_number == 4)
                    {
                        Thread.Sleep(2000);
                        global.gesture_number = 0;
                    }*/
                    if (global.gesture_number == 9)
                    {
                      
                        //tabControl1.Invoke.(new MethodInvoker(delegate{tabControl1.TabPages.Remove(tabControl1.SelectedTab);}));
                        this.Hide();
                        global.gesture_number = 0;
                        Thread.Sleep(2000);

                    }
                }
                Thread.Sleep(1000);
            }

        }

    }


}

