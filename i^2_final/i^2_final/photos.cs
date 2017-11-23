using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace i_2_final
{
    public partial class photos : Form
    {
        protected string[] pFileNames;
        protected int pCurrentImage = 0;
        protected string[] fileArray;
        protected string path;
        Image img1;
        List<Image> images = new List<Image>();
      
        public photos()
        {
            InitializeComponent();
            //MessageBox.Show(trackBar1.Value.ToString());
            Thread lis = new Thread(new ThreadStart(ges_listen));
            lis.Start();
        }

       
        protected void ShowCurrentImage()
        {
            if (pCurrentImage >= 0 && pCurrentImage <= images.Count - 1)
            {
                pictureBox1.Image = images[pCurrentImage];
                //MessageBox.Show(images[pCurrentImage]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (images.Count > 0)
            {
                pCurrentImage = pCurrentImage == 0 ? images.Count - 1 : --pCurrentImage;
                ShowCurrentImage();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (images.Count > 0)
            {
                pCurrentImage = pCurrentImage == images.Count - 1 ? 0 : ++pCurrentImage;
                ShowCurrentImage();
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
                        if (images.Count > 0)
                        {
                            pCurrentImage = pCurrentImage == 0 ? images.Count - 1 : --pCurrentImage;
                            ShowCurrentImage();
                        }
                        global.gesture_number = 0;
                    }
                    else if (global.gesture_number == 4)
                    {
                        if (images.Count > 0)
                        {
                            pCurrentImage = pCurrentImage == images.Count - 1 ? 0 : ++pCurrentImage;
                            ShowCurrentImage();
                        }
                        Thread.Sleep(2000);
                        global.gesture_number = 0;
                    }
                    else if (global.gesture_number == 9)
                    {
                        
                        //this.Hide();
                        global.gesture_number = 0;
                        Thread.Sleep(5000);

                    }
                }
                Thread.Sleep(1000);
            }

        }

        

       
       

       /* Image Zoom(Image img, Size size) {
            Bitmap bmp = new Bitmap(img, img.Width + (img.Width + size.Width / 20), img.Height + (img.Height + size.Height / 20));
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {
           /* OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "JPEG|*.jpg|Bitmaps|*.bmp";
            openFileDialog.InitialDirectory = @"Libraries\Pictures";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pFileNames = openFileDialog.FileNames;
                path = System.IO.Path.GetDirectoryName(pFileNames[0]);
                fileArray = Directory.GetFiles(path, "*.jpg");
                pCurrentImage = 0;
                ShowCurrentImage();
            }*/

            DirectoryInfo di = new DirectoryInfo(@"C:\Users\Shubhankar\Pictures\Camera Roll\"); // give path
            FileInfo[] finfos = di.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
            foreach (FileInfo fi in finfos)
               images.Add(Image.FromFile(fi.FullName));
            ShowCurrentImage();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

       
        
        }
    }
