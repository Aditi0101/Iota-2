using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using pppt = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.IO;
using System.Threading;
namespace i_2_final
{
    public partial class ppt : Form
    {

        pppt.SlideShowSettings sst1;
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);




        [DllImport("user32.dll", SetLastError = true)]

        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]

        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        pppt.Presentation presentation;

        Microsoft.Office.Interop.PowerPoint.SlideShowView oSlideShowView;

        bool flag = false;


        public ppt()
        {
            InitializeComponent();
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "PPT FILES (.pptx)|*.pptx|PPT Files (.ppt)|*.ppt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOK = openFileDialog1.ShowDialog();

            

            // Process input if the user clicked OK.
            if (userClickedOK == DialogResult.OK)
            {
                String file = openFileDialog1.FileName;

                pppt.Application application;

                try
                {
                    // For Display in Panel

                    IntPtr screenClasshWnd = (IntPtr)0;

                    IntPtr x = (IntPtr)0;

                    application = new pppt.Application();

                    presentation = application.Presentations.Open(@file, MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoFalse);

                    panel1.Controls.Add(application as Control);

                    sst1 = presentation.SlideShowSettings;

                    sst1.LoopUntilStopped = Microsoft.Office.Core.MsoTriState.msoCTrue;

                    pppt.Slides objSlides = presentation.Slides;

                    sst1.LoopUntilStopped = MsoTriState.msoTrue;

                    sst1.StartingSlide = 1;

                    sst1.EndingSlide = objSlides.Count;

                    panel1.Dock = DockStyle.Fill;

                    sst1.ShowType = pppt.PpSlideShowType.ppShowTypeKiosk;

                    pppt.SlideShowWindow sw = sst1.Run();

                    oSlideShowView = presentation.SlideShowWindow.View;

                    IntPtr pptptr = (IntPtr)sw.HWND;

                    //Gesteur Listener
                    Thread ges = new Thread(new ThreadStart(ges_listen));
                    ges.Start();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }



        public void ges_listen()
        {
            
            while (true)
            {
                if (global.gesture)
                {
                    if (global.gesture_number == 3)
                    {
                        presentation.SlideShowWindow.View.Previous();
                        Thread.Sleep(2000);
                        global.gesture_number = 0;
                    }
                    else if (global.gesture_number == 4)
                    {
                        presentation.SlideShowWindow.View.Next();
                        Thread.Sleep(2000);
                        global.gesture_number = 0;
                    }
                    else if (global.gesture_number == 9)
                    {
                        //if(oSlideShowView != null)
                        {
                        //oSlideShowView.Exit();

                       // this.Dispose();
                        }
                        presentation.Close();
                        //this.Hide();
                        global.gesture_number = 0;
                        Thread.CurrentThread.Suspend();
                        //Thread.Sleep(5000);
                        
                    }
                }
                Thread.Sleep(1000);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            presentation.SlideShowWindow.View.Previous();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            presentation.SlideShowWindow.View.Next();

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //capture up arrow key
            if (keyData == Keys.Up)
            {
                MessageBox.Show("You pressed Up arrow key");
                return true;
            }
            //capture down arrow key
            if (keyData == Keys.Down)
            {
                MessageBox.Show("You pressed Down arrow key");
                return true;
            }
            //capture left arrow key
            if (keyData == Keys.Left)
            {
                MessageBox.Show("You pressed Left arrow key");
                return true;
            }
            //capture right arrow key
            if (keyData == Keys.Right)
            {
                MessageBox.Show("You pressed Right arrow key");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void presskey(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show(e.KeyChar.ToString());
        }

    }

}

