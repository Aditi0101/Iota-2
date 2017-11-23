using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using HandGestureRecognition.SkinDetector;

namespace i_2_final
{
    public partial class gesture_recog : Form
    {
        double[,] Theta2, Theta1, Theta3, v, image_i;
        int nRows1 = 2501;
        int nColumns1 = 2500;
        int nRows2 = 2501;
        int nColumns2 = 1200;
        int que_length = 2;
        Queue<int> gesture_history;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        int nRows3 = 1201;
        int nColumns3 = 10;
       

        public gesture_recog()
        {
            InitializeComponent();
            //read .csv file to 2-D array
            Console.WriteLine("test");
            StreamReader aFile1 = new StreamReader(@"E:\BTP\gesture\1.csv");

            gesture_history = new Queue<int>();
            for (int i = 0; i < que_length; i++)
            {
                gesture_history.Enqueue(0);
            }

            Theta1 = new double[nRows1, nColumns1];
            Bitmap bp = new Bitmap(nRows1, nColumns1);
            int colIndex = 0;
            int rowIndex = 0;
            string aLine;
            while ((aLine = aFile1.ReadLine()) != null)
            {
                //MessageBox.Show(aLine);
                colIndex = 0;
                string[] parts = aLine.Split(',');
                //MessageBox.Show(parts[0]);
                foreach (string part in parts)
                {
                    Theta1[rowIndex, colIndex] = Double.Parse(part);
                    ++colIndex;
                    //bp.SetPixel(rowIndex, colIndex, ); 
                }

                ++rowIndex;
            }
            //MessageBox.Show("" + Theta1[0, 0] + " " + Theta1[2500, 2499] + " " + Theta1[500, 260] + " " + Theta1[50, 90] + " " + Theta1[201, 102] + " " + Theta1[1, 2] + " " + Theta1[223, 500]);
            aFile1.Close();

            StreamReader aFile2 = new StreamReader(@"E:\BTP\gesture\2.csv");
            
            Theta2 = new double[nRows2, nColumns2];
            colIndex = 0;
            rowIndex = 0;
            while ((aLine = aFile2.ReadLine()) != null)
            {
                colIndex = 0;
                string[] parts = aLine.Split(',');
                foreach (string part in parts)
                {
                    Theta2[rowIndex, colIndex] = Double.Parse(part);
                    ++colIndex;
                }
                ++rowIndex;
            }
           // MessageBox.Show("" + Theta2[0, 0] + " " + Theta2[2500, 1199] + " " + Theta2[500, 260] + " " + Theta2[50, 90] + " " + Theta2[201, 102] + " " + Theta2[1, 2] + " " + Theta2[223, 500]);
            aFile2.Close();

            StreamReader aFile3 = new StreamReader(@"E:\BTP\gesture\3.csv");
            
            Theta3 = new double[nRows3, nColumns3];
            colIndex = 0;
            rowIndex = 0;
            while ((aLine = aFile3.ReadLine()) != null)
            {
                colIndex = 0;
                string[] parts = aLine.Split(',');
                foreach (string part in parts)
                {
                    Theta3[rowIndex, colIndex] = Double.Parse(part);
                    ++colIndex;
                }
                ++rowIndex;
            }
            //MessageBox.Show("" + myArray[0, 0] + " " + myArray[0, 1] + " " + myArray[0, 2] + " " + myArray[1,0] + " " + myArray[1,1] + " " + myArray[1,2] + " " + myArray[3,0]);
            aFile3.Close();
           // image(imgSource);


        }

        public int image(Image<Gray, Byte> img)
        {
            Bitmap bmp = img.ToBitmap();
           
            image_i = new double[1, nRows1];
            double[,] ii = new double[bmp.Size.Height, bmp.Size.Width];
            
            int oi = 1;
            for (int k = 0; k < bmp.Size.Height; k++)
            {
                for (int l = 0; l < bmp.Size.Width; l++)
                {
                    
                    image_i[0, oi] = bmp.GetPixel(k, l).R /255;
                    //MessageBox.Show("" + image_i[0, i]);
                    oi++;
                }
            }
            //MessageBox.Show("" + oi);
            //MessageBox.Show("" + image_i[0, 0] + " " + image_i[0,2500] + " " + image_i[0,250] + " " + image_i[0, 90] + " " + image_i[0, 102] + " " + image_i[0, 2] + " " + image_i[0, 500]);
            image_i[0, 0] = 1;
            

            double[,] b = Theta1;
            double[,] c = Theta2;
            double[,] d = Theta3;
            v = new double[1, 2501];
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2500; j++)
                {
                    v[i, j+1] = 0;
                    for (int k = 0; k < 2501; k++)
                    {
                        v[i, j+1] += image_i[i, k] * b[k, j];
                    }
                }

            }
            v[0, 0] = 1;
            //MessageBox.Show("" + v[0, 0] + " _" + v[0, 1] + " _" + v[0, 2] + " _" + v[0, 3] + " _" + v[0, 4] + " _" + v[0, 5] + " _" + v[0, 6] + " _" + v[0, 7] + " _" + v[0, 8] + " _" + v[0, 9]);
            for (int i = 0; i < 1; i++)
            {
                for (int j=0; j < 2501; j++)
                {
                    //sigmoid= 1.0 ./ (1.0 + exp(-z));
                    v[i, j] = (1/(1 + (Math.Exp(-v[i, j]))));
                }
            }
            v[0, 0] = 1;
            //MessageBox.Show("" + v[0, 0] + " _" + v[0, 1] + " _" + v[0, 2] + " _" + v[0, 3] + " _" + v[0, 4] + " _" + v[0, 5] + " _" + v[0, 6] + " _" + v[0, 7] + " _" + v[0, 8] + " _" + v[0, 9]);

            double[,] v1 = new double[1, 1201];
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 1200; j++)
                {
                    v1[i, j+1] = 0;
                    for (int k = 0; k < 2501; k++)
                    {
                        v1[i, j+1] += v[i, k] * c[k, j];
                    }
                }

            }
            v1[0, 0] = 1;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 1201; j++)
                {
                    //sigmoid= 1.0 ./ (1.0 + exp(-z));
                    v1[i, j] = (1/(1 + (Math.Exp(-v1[i, j]))));
                }
            }
            v1[0, 0] = 1;
            //MessageBox.Show("" + v1[0, 0] + " _" + v1[0, 1] + " _" + v1[0, 2] + " _" + v1[0, 3] + " _" + v1[0, 4] + " _" + v1[0, 5] + " _" + v1[0, 6] + " _" + v1[0, 7] + " _" + v1[0, 8] + " _" + v1[0, 9]);

            double[,] v2 = new double[1, 10];
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    v2[i, j] = 0;
                    for (int k = 0; k < 1201; k++)
                    {
                        v2[i, j] += v1[i, k] * d[k, j];
                    }
                }

            }
            for (int j = 0; j < 10; j++)
            {
                //sigmoid= 1.0 ./ (1.0 + exp(-z));
                v2[0, j] = (1 / (1 + (Math.Exp(-v2[0, j]))));
                v2[0, j] = Math.Round(v2[0, j], 4);
            }

            double max = v2[0, 0];
            int max_index = 1;
            for (int j = 0; j < 10; j++)
            {
                if (v2[0, j] > max)
                {
                    max = v2[0, j];
                    max_index = j + 1;
                }
            }

            //MessageBox.Show(""+v2[0,0] + " \n" +v2[0,1] + " \n" +v2[0,2] + " \n" +v2[0,3] + " \n" +v2[0,4] + " \n" +v2[0,5] + " \n" +v2[0,6] + " \n" +v2[0,7] + " \n" +v2[0,8] + " \n" +v2[0,9]);
            //listBox1.Items.Add("" + v2[0, 0] + " \n" + v2[0, 1] + " \n" + v2[0, 2] + " \n" + v2[0, 3] + " \n" + v2[0, 4] + " \n" + v2[0, 5] + " \n" + v2[0, 6] + " \n" + v2[0, 7] + " \n" + v2[0, 8] + " \n" + v2[0, 9] + "\n\n");
            if (max > 0.95)
            {
                //listBox1.Items.Add(max_index + "\n");
                listBox1.Items.Add("" + v2[0, 0] + " \n" + v2[0, 1] + " \n" + v2[0, 2] + " \n" + v2[0, 3] + " \n" + v2[0, 4] + " \n" + v2[0, 5] + " \n" + v2[0, 6] + " \n" + v2[0, 7] + " \n" + v2[0, 8] + " \n" + v2[0, 9] + "\n\n");
                label1.Text = "" + max_index;
                listBox1.TopIndex = listBox1.Items.Count - 1;
                gesture_history.Dequeue();
                gesture_history.Enqueue(max_index);
            }
            else
            {
                //gesture_history.Dequeue();
                //gesture_history.Enqueue(0);
            }

            int [] num = gesture_history.ToArray();
            int iii = 1;
            while (iii != que_length)
            {
                if (num[0] != num[iii])
                {
                    global.gesture_number = 0;
                    break;
                   
                }
                iii += 1;
            }
            if (iii == que_length)
            {
                global.gesture_number = num[0];
                global.gesture = true;
            }
            else
            {
                global.gesture = false;
            }

            return max_index;
        }
    }
}

