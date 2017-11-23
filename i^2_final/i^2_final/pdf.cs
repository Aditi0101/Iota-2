using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using itextsharp.pdfa;
using System.Diagnostics;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Threading;
namespace i_2_final
{
    public partial class pdf : Form
    {
        Control ct;
        Stopwatch s = new Stopwatch();
        Thread t1 = null, t2 = null, t3 = null;
        public pdf()
        {
            InitializeComponent();
            t2 = new Thread(new ThreadStart(scrolls2));
            t1 = new Thread(new ThreadStart(scrolls1));
            t3 = new Thread(new ThreadStart(scrolls1));

        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            // set file filter of dialog 

            dlg.Filter = "pdf files (*.pdf) |*.pdf;";

            dlg.ShowDialog();

            if (dlg.FileName != null)
            {

                // use the LoadFile(ByVal fileName As String) function for open the pdf in control

                axAcroPDF1.LoadFile(dlg.FileName);

                //axAcroPDF1.setShowToolbar(false);
                // axAcroPDF1.setViewScroll("FitH", 0);
                s.Start();
                scrolls1();
            }
        }
        int i = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            /*
            if (t2.IsAlive)
            {
                t2.Abort();
            }
            if (t3.IsAlive)
            {
                t3.Abort();
            }
            t1.Start();
            */
            scrolls1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (t1.IsAlive)
                t1.Abort();
            if (t3.IsAlive)
                t3.Abort();
            t2.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (t1.IsAlive)
                t1.Abort();
            if (t2.IsAlive)
                t2.Abort();
            t3.Start();
        }

        private void scrolls1()
        {

            while (i < 40)
            {
                if (s.ElapsedMilliseconds > 5000)
                {
                    axAcroPDF1.gotoNextPage();
                    s.Restart();
                    i++;
                }
            }
        }
        private void scrolls2()
        {

            while (i < 40)
            {
                if (s.ElapsedMilliseconds > 10000)
                {
                    if (axAcroPDF1 == null)
                    {
                        axAcroPDF1.gotoNextPage();
                        s.Restart();
                        i++;
                    }

                }
            }
        }


    }

}


