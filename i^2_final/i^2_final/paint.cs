using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;

namespace i_2_final
{
    public partial class paint : Form
    {
        public Bitmap drawing;
        bool startPaint = false;
        Graphics g;
        //nullable int for storing Null value
        int? initX;
        int? initY;
        int prevX;
        int prevY;
        int fiX, fiY;
        int a = 3, k = 1, x = 10, h = 10;
        string fname = "";
        bool drawRectangle = false;
        bool drawCircle = false;
        Pen p = new Pen(Color.Black, 1);

        public paint()
        {
            InitializeComponent();
            g = pnl_Draw.CreateGraphics();
            drawing = new Bitmap(panel1.Width, panel1.Height);
        }
        

        //Event Fired when the mouse pointer is over Panel and a mouse button is pressed
        private void pnl_Draw_MouseDown(object sender, MouseEventArgs e)
        {
            startPaint = true;
            prevX = e.X;
            prevY = e.Y;

        }
        //Fired when the mouse pointer is over the pnl_Draw and a mouse button is released.
        private void pnl_Draw_MouseUp(object sender, MouseEventArgs e)
        {
            startPaint = false;
            Pen px = new Pen(btn_PenColor.BackColor, k);
            fiX = e.X;
            fiY = e.Y;
            /*MessageBox.Show(fiX.ToString()); 
            MessageBox.Show( fiY.ToString()); 
            MessageBox.Show(prevX.ToString());
            MessageBox.Show(prevY.ToString());*/
            switch (a)
            {
                case 1:
                    if (drawRectangle)
                    {

                        int o, p;
                        o = (prevX - fiX);
                        p = (prevY - fiY);
                        Graphics g = pnl_Draw.CreateGraphics();
                        int s = o, r = p;
                        if (s < 0)
                            s = -s;
                        if (r < 0)
                            r = -r;
                        if (o > 0 && p < 0)
                        {

                            g.DrawRectangle(px, prevX - o, prevY, s, r);
                        }
                        if (o > 0 && p > 0)
                        {

                            g.DrawRectangle(px, prevX - o, prevY - p, s, r);
                        }
                        if (o < 0 && p > 0)
                        {

                            g.DrawRectangle(px, prevX, prevY - p, s, r);
                        }
                        if (o < 0 && p < 0)
                        {

                            g.DrawRectangle(px, prevX, prevY, s, r);
                        }



                    }
                    break;

                case 2:
                    if (drawCircle)
                    {

                        int o, p;
                        o = (prevX - fiX);
                        p = (prevY - fiY);
                        Graphics g = pnl_Draw.CreateGraphics();
                        int s = o, r = p;
                        if (s < 0)
                            s = -s;
                        if (r < 0)
                            r = -r;
                        if (o > 0 && p < 0)
                        {

                            g.DrawEllipse(px, prevX - o, prevY, s, r);
                        }
                        if (o > 0 && p > 0)
                        {

                            g.DrawEllipse(px, prevX - o, prevY - p, s, r);
                        }
                        if (o < 0 && p > 0)
                        {

                            g.DrawEllipse(px, prevX, prevY - p, s, r);
                        }
                        if (o < 0 && p < 0)
                        {

                            g.DrawEllipse(px, prevX, prevY, s, r);
                        }
                    }
                    break;
            }
        }
        //Event fired when the mouse pointer is moved over the Panel(pnl_Draw).

        private void pnl_Draw_MouseMove(object sender, MouseEventArgs e)
        {

            if (startPaint && a == 3)
            {

                //Setting the Pen BackColor and line Width  

                Pen p = new Pen(btn_PenColor.BackColor, k);


                //Drawing the line.  
                g.DrawLine(p, new Point(initX ?? e.X, initY ?? e.Y), new Point(e.X, e.Y));
                initX = e.X;
                initY = e.Y;
            }
            if (startPaint && a == 6)
            {

                //Setting the Pen BackColor and line Width  

                Pen p = new Pen(Color.White, x);


                //Drawing the line.  
                g.DrawLine(p, new Point(initX ?? e.X, initY ?? e.Y), new Point(e.X, e.Y));
                initX = e.X;
                initY = e.Y;
                btn_PenColor.BackColor = Color.Black;
            }

            if (startPaint && a == 4)
            {

                Graphics g = pnl_Draw.CreateGraphics();
                g.FillEllipse(new SolidBrush(bsh_color.BackColor), e.X, e.Y, h, h);

                g.Dispose();
            }


        }


        //Button for Setting pen Color
        private void button1_Click(object sender, EventArgs e)
        {
            a = 3;
            //Open Color Dialog and Set BackColor of btn_PenColor if user click on OK
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                btn_PenColor.BackColor = c.Color;
            }
        }


        //New 
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Clearing the graphics from the Panel(pnl_Draw)
            g.Clear(pnl_Draw.BackColor);
            //Setting the BackColor of pnl_draw and btn_CanvasColor to White on Clicking New under File Menu
            pnl_Draw.BackColor = Color.White;

        }

        //Setting the Canvas Color
        private void btn_CanvasColor_Click_1(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                pnl_Draw.BackColor = c.Color;

            }
        }


        private void btn_Rectangle_Click(object sender, EventArgs e) // button for rectangle 
        {
            a = 1;
            drawRectangle = true;
        }

        private void btn_Circle_Click(object sender, EventArgs e)  // button click for circle
        {
            a = 2;
            drawCircle = true;
        }
        //Exit under File Menu
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button4_Click(object sender, EventArgs e) //eraser
        {

            btn_PenColor.BackColor = Color.White;
            a = 6;

        }

        private void button3_Click(object sender, EventArgs e)   // clear all 
        {
            g.Clear(Color.White);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {


            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        this.button2.Image.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        this.button2.Image.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        this.button2.Image.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();

            }



        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {


            trackBar1.Minimum = 1;
            trackBar1.Maximum = 50;

            k = Int32.Parse(trackBar1.Value.ToString());

        }
        // to open fileDialogBox
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "paint files|*.jpg|all files (*.*)|*.*";
            openFileDialog1.ShowDialog();
            fname = openFileDialog1.FileName;
            System.Diagnostics.Process.Start(fname);

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.google.co.in/webhp?sourceid=chrome-instant&ion=1&espv=2&ie=UTF-8#q=paint");
        }



        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar2.Minimum = 10;
            trackBar2.Maximum = 100;

            x = Int32.Parse(trackBar2.Value.ToString());
        }



        private void bsh_color_Click_1(object sender, EventArgs e)
        {
            a = 4;
            //Open Color Dialog and Set BackColor of btn_PenColor if user click on OK
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK)
            {
                bsh_color.BackColor = c.Color;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            a = 4;

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            trackBar3.Minimum = 10;
            trackBar3.Maximum = 100;

            h = Int32.Parse(trackBar3.Value.ToString());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            a = 3;
        }

        private void pnl_Draw_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
