using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Cvb;


namespace i_2_final
{
    public partial class mouse : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //[DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, int dwExtraInfo);
        private const uint MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const uint MOUSEEVENTF_VWHEEL = 0x0800;
        private const uint MOUSEEVENTF_HWHEEL = 0x1000;
        Capture capture;
        private bool _captureInProgress;
        private static BackgroundSubtractor fgDetector;
        private static Emgu.CV.Cvb.CvBlobDetector blobDetector;
        private int minarea = 400;
        private int maxarea = 5000;
        private int redThres = 65;
        private int blueThres = 55;
        private int greenThres = 15;
        private int mouseflag = 0;
        private int ccount = 0;
        private int scroll_x = 0;
        private int scroll_y = 0;
        private int scroll_mul_h = 5;
        private int scroll_mul_v = 5;
        private int safevalue = 5;
        private double cursor_mul_x = 1.8;//3
        private int cursor_add_x = -500;//-150
        private double cursor_mul_y = 2;//3
        private int cursor_add_y = -300;//-150
        private int green_history_x = Screen.PrimaryScreen.Bounds.Width / 2;
        private int green_history_y = Screen.PrimaryScreen.Bounds.Height / 2;
        Queue<int> cursor_history_x;
        Queue<int> cursor_history_y;
        private int queue_cursor_length = 10;


        public mouse()
        {
            InitializeComponent();
            //CvInvoke.UseOpenCL = false;
            while (global.capture == null)
            { }
            try
            {
                capture = global.capture;    //new Capture();
               // capture = new Capture();
                //if (capture != null) capture.FlipHorizontal = !capture.FlipHorizontal;
                capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

           // MessageBox.Show("" + Screen.PrimaryScreen.Bounds + "         __        " + capture.Width + " " + capture.Height);

            fgDetector = new Emgu.CV.VideoSurveillance.BackgroundSubtractorMOG2();
            blobDetector = new Emgu.CV.Cvb.CvBlobDetector();
            cursor_history_x = new Queue<int>();
            cursor_history_y = new Queue<int>();

            //initilize queue with initial 
            for (int i = 0; i < queue_cursor_length; i++)
            {
                cursor_history_x.Enqueue(Screen.PrimaryScreen.Bounds.Width / 2);
                cursor_history_y.Enqueue(Screen.PrimaryScreen.Bounds.Height / 2);
            }

        }

        private void ProcessFrame(object sender, EventArgs arg)
        {

            Mat frame = new Mat();
            capture.Retrieve(frame, 0);
            Image<Hsv, Byte> currenthsvFrame = (frame.ToImage<Bgr, Byte>()).Convert<Hsv, Byte>();
            Image<Gray, Byte> color_one = new Image<Gray, Byte>(frame.Width, frame.Height);
            Image<Gray, Byte> color_two = new Image<Gray, Byte>(frame.Width, frame.Height);
            Image<Gray, Byte> color_three = new Image<Gray, Byte>(frame.Width, frame.Height);
            Image<Gray, Byte> color_four = new Image<Gray, Byte>(frame.Width, frame.Height);


            /*
             * Color one is Red
             * Color two is Blue
             * Color three is Green
             * Color Four is Yellow
             * Green is in Right Index Finger
             * Blue is in Left Index Finger
             * Red in Right Thumb
             * Yelloe in Left Thumb
            */


           /* Hsv hsv_min_color_one = new Hsv(0, 135, 110);
            Hsv hsv_max_color_one = new Hsv(6, 255, 255);
            Hsv hsv_min_color_two = new Hsv(112, 53, 10);
            Hsv hsv_max_color_two = new Hsv(119, 255, 255);
            Hsv hsv_min_color_three = new Hsv(68, 59, 80);
            Hsv hsv_max_color_three = new Hsv(85, 255, 255);
            Hsv hsv_min_color_four = new Hsv(20, 165, 165);
            Hsv hsv_max_color_four = new Hsv(36, 255, 255);*/
            Hsv hsv_min_color_one = new Hsv(0, 135, 50);
            //Hsv hsv_max_color_one = new Hsv(6, 255, 255);
            Hsv hsv_max_color_one = new Hsv(8, 255, 255);
            Hsv hsv_min_color_two = new Hsv(112, 53, 10);
            Hsv hsv_max_color_two = new Hsv(119, 255, 255);
            /*
            Hsv hsv_min_color_three = new Hsv(68, 59, 80);
            Hsv hsv_max_color_three = new Hsv(85, 255, 255);
            Hsv hsv_min_color_four = new Hsv(20, 165, 165);
            Hsv hsv_max_color_four = new Hsv(36, 255, 255);
            */
            Hsv hsv_min_color_three = new Hsv(65, 70, 0);
            Hsv hsv_max_color_three = new Hsv(109, 255, 255);
            Hsv hsv_min_color_four = new Hsv(18, 155, 155);
            Hsv hsv_max_color_four = new Hsv(35, 255, 255);



            color_one = currenthsvFrame.InRange(hsv_min_color_one, hsv_max_color_one);
            color_two = currenthsvFrame.InRange(hsv_min_color_two, hsv_max_color_two);
            color_three = currenthsvFrame.InRange(hsv_min_color_three, hsv_max_color_three);
            color_four = currenthsvFrame.InRange(hsv_min_color_four, hsv_max_color_four);



            //Blob detection
            #region Blob Detection

            //Color one detection
            Image<Bgr, Byte> smoothedFrame_cone = new Image<Bgr, byte>(currenthsvFrame.Size);
            CvInvoke.GaussianBlur(color_one, smoothedFrame_cone, new Size(3, 3), 1); //filter out noises

            Mat forgroundMask_cone = new Mat();
            fgDetector.Apply(smoothedFrame_cone, forgroundMask_cone);

            CvBlobs blobs_color_one = new CvBlobs();
            blobDetector.Detect(forgroundMask_cone.ToImage<Gray, byte>(), blobs_color_one);
            blobs_color_one.FilterByArea(minarea, maxarea);


            //Color two Blob Detection
            Image<Bgr, Byte> smoothedFrame_ctwo = new Image<Bgr, byte>(currenthsvFrame.Size);
            CvInvoke.GaussianBlur(color_two, smoothedFrame_ctwo, new Size(3, 3), 1); //filter out noises

            Mat forgroundMask_ctwo = new Mat();
            fgDetector.Apply(smoothedFrame_ctwo, forgroundMask_ctwo);

            CvBlobs blobs_color_two = new CvBlobs();
            blobDetector.Detect(forgroundMask_ctwo.ToImage<Gray, byte>(), blobs_color_two);
            blobs_color_two.FilterByArea(minarea, maxarea);


            //Color three blob detection
            Image<Bgr, Byte> smoothedFrame_cthree = new Image<Bgr, byte>(currenthsvFrame.Size);
            CvInvoke.GaussianBlur(color_three, smoothedFrame_cthree, new Size(3, 3), 1); //filter out noises

            Mat forgroundMask_cthree = new Mat();
            fgDetector.Apply(smoothedFrame_cthree, forgroundMask_cthree);

            CvBlobs blobs_color_three = new CvBlobs();
            blobDetector.Detect(forgroundMask_cthree.ToImage<Gray, byte>(), blobs_color_three);
            blobs_color_three.FilterByArea(minarea, maxarea);


            //Color four detection
            Image<Bgr, Byte> smoothedFrame_cfour = new Image<Bgr, byte>(currenthsvFrame.Size);
            CvInvoke.GaussianBlur(color_four, smoothedFrame_cfour, new Size(3, 3), 1); //filter out noises

            Mat forgroundMask_cfour = new Mat();
            fgDetector.Apply(smoothedFrame_cfour, forgroundMask_cfour);

            CvBlobs blobs_color_four = new CvBlobs();
            blobDetector.Detect(forgroundMask_cfour.ToImage<Gray, byte>(), blobs_color_four);
            blobs_color_four.FilterByArea(minarea, maxarea);

            #endregion

            //Makers Interpretition
            float[] cent_color_one = new float[2];
            float[] cent_color_two = new float[2];
            float[] cent_color_three = new float[2];
            float[] cent_color_four = new float[2];

            cent_color_one[0] = 0;
            cent_color_one[1] = 0;
            cent_color_two[0] = 0;
            cent_color_two[1] = 0;
            cent_color_three[0] = green_history_x;
            cent_color_three[1] = green_history_y;
            cent_color_four[0] = 0;
            cent_color_four[1] = 0;

            //Corsor control with Green Marker

            if (blobs_color_three.Count == 1 || mouseflag != 0)
            {
                foreach (var pair in blobs_color_three)
                {
                    CvBlob b = pair.Value;
                    CvInvoke.Rectangle(frame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                    cursor_history_x.Enqueue((int)b.Centroid.X);
                    cursor_history_y.Enqueue((int)b.Centroid.Y);

                    cursor_history_x.Dequeue();  
                    cursor_history_y.Dequeue();

                    cent_color_three[0] = (int)b.Centroid.X;
                    cent_color_three[1] = (int)b.Centroid.Y;

                    /*int temp_sum = 0;
                    int[] temp = cursor_history_x.ToArray();
                    for (int i = 0; i < queue_cursor_length; i++)
                        temp_sum += temp[i];
                    cent_color_three[0] = temp_sum / queue_cursor_length;

                    temp_sum = 0;
                    temp = cursor_history_y.ToArray();
                    for (int i = 0; i < queue_cursor_length; i++)
                        temp_sum += temp[i];
                    cent_color_three[1] = temp_sum / queue_cursor_length;

                    green_history_x = (int)cent_color_three[0];
                    green_history_y = (int)cent_color_three[1];*/
                }

                //Cursor Movement Controlled
                //Primary Screem
                // if (Screen.AllScreens.Length == 1)
                {
                    //Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width - (int)(cursor_mul * (int)cent_color_three[0] * Screen.PrimaryScreen.Bounds.Width / capture.Width), (int)(cursor_mul * (int)cent_color_three[1]) * Screen.PrimaryScreen.Bounds.Height / capture.Height);
                    Cursor.Position = new Point((int)((cursor_mul_x * (int)cent_color_three[0]) * (Screen.PrimaryScreen.Bounds.Width) / capture.Width) + cursor_add_x, (((int)cursor_mul_y * (int)cent_color_three[1]) * Screen.PrimaryScreen.Bounds.Height / capture.Height) + cursor_add_y);
                    //mouse_event(MOUSEEVENTF_MOVE, ( (-(int)cent_color_three[0] + green_history_x)), ( (-(int)cent_color_three[1] + green_history_y)),0,0);
                    //mouse_event(MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
                }
                //Secondary Screen
                //Cursor.Position = new Point((int)(cursor_mul * (int)cent_color_three[0] * Screen.AllScreens[1].Bounds.Width / capture.Width), (int)(cursor_mul * (int)cent_color_three[1]) * Screen.AllScreens[1].Bounds.Height / capture.Height);
                //Number of Screen = 2 and both a same time
                /*      if (Screen.AllScreens.Length == 2)
                     {

                         Cursor.Position = new Point((int)(cursor_mul * (int)cent_color_three[0] * (Screen.AllScreens[1].Bounds.Width + Screen.AllScreens[0].Bounds.Width) / capture.Width),
                                                 (int)(cursor_mul * (int)cent_color_three[1]) * (Screen.AllScreens[1].Bounds.Height + Screen.AllScreens[0].Bounds.Height) / capture.Height);
                     }
                     //Number of screen =3 and all at same time
                     if (Screen.AllScreens.Length == 3)
                     {

                         Cursor.Position = new Point((int)(cursor_mul * (int)cent_color_three[0] * (Screen.AllScreens[1].Bounds.Width + Screen.AllScreens[0].Bounds.Width + Screen.AllScreens[2].Bounds.Width) / capture.Width),
                                                 (int)(cursor_mul * (int)cent_color_three[1]) * (Screen.AllScreens[1].Bounds.Height + Screen.AllScreens[0].Bounds.Height + Screen.AllScreens[0].Bounds.Height) / capture.Height);
                     }
                        */

                
                //Check for Clicks
                if (blobs_color_one.Count == 1)
                {
                    foreach (var pair in blobs_color_one)
                    {
                        CvBlob b = pair.Value;
                        CvInvoke.Rectangle(frame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                        cent_color_one[0] = b.Centroid.X;
                        cent_color_one[1] = b.Centroid.Y;
                    }
                    if (blobs_color_three.Count == 0)
                    {
                        if (ccount == 1)
                        {
                            //double click
                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, (uint)cent_color_three[0], (uint)cent_color_three[1], 0, 0);
                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, (uint)cent_color_three[0], (uint)cent_color_three[1], 0, 0);
                            Thread.Sleep(150);
                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, (uint)cent_color_three[0], (uint)cent_color_three[1], 0, 0);
                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, (uint)cent_color_three[0], (uint)cent_color_three[1], 0, 0);
                        }
                        else
                        {
                            ccount--;
                        }
                    }

                    else if ((cent_color_one[0] - cent_color_three[0]) * (cent_color_one[0] - cent_color_three[0]) + (cent_color_one[1] - cent_color_three[1]) * (cent_color_one[1] - cent_color_three[1]) <= 5000)
                    {
                        ccount = safevalue;
                        mouseflag = 1;
                        //single click
                        mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)cent_color_three[0], (uint)cent_color_three[1], 0, 0);
                        mouse_event(MOUSEEVENTF_LEFTUP, (uint)cent_color_three[0], (uint)cent_color_three[1], 0, 0);
                        mouse_event(MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
                    }
                }
                else
                {
                    ccount = 0;

                }

            }


            if (blobs_color_two.Count == 1)
            {
                foreach (var pair in blobs_color_two)
                {
                    CvBlob b = pair.Value;
                    CvInvoke.Rectangle(frame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                    cent_color_two[0] = b.Centroid.X;
                    cent_color_two[1] = b.Centroid.Y;
                }

                if (blobs_color_three.Count == 1 && ((cent_color_three[0] - cent_color_two[0]) * (cent_color_three[0] - cent_color_two[0]) + (cent_color_three[1] - cent_color_two[1]) * (cent_color_three[1] - cent_color_two[1]) <= 5000))
                {
                    //right click
                    mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTDOWN, (uint)cent_color_three[0], (uint)cent_color_three[1], 0, 0);
                    mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTUP, (uint)cent_color_three[0], (uint)cent_color_three[1], 0, 0);
                }

                else //if(blobs_g.Count == 0)
                {
                    //MessageBox.Show("d");
                    //Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width - (int)(cursor_mul * green_history_x * Screen.PrimaryScreen.Bounds.Width / capture.Width), (int)(cursor_mul * green_history_y) * Screen.PrimaryScreen.Bounds.Height / capture.Height);
                    //mouse_event(MOUSEEVENTF_VWHEEL, 0, 0, (scroll_y - (int)cent_color_two[1]) * scroll_mul_v, 0);
                    mouse_event(MOUSEEVENTF_HWHEEL, 0, 0, (uint)((scroll_x - (int)cent_color_two[0]) * scroll_mul_h), 0);
                    mouse_event(MOUSEEVENTF_VWHEEL,(uint) Cursor.Position.X, (uint)Cursor.Position.Y, 50, 0);
                    //mouse_event(MOUSEEVENTF_HWHEEL, 0, 0, 50, 0);
                    scroll_y = (int)cent_color_two[1];
                    scroll_x = (int)cent_color_two[0];

                }

            }


            captureImageBox.Image = frame;
            grayscaleImageBox.Image = color_one;
            smoothedGrayscaleImageBox.Image = color_two;
            cannyImageBox.Image = color_three;
            Color4ImageBox.Image = color_four;





        }



        private void captureButton_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture
                    captureButton.Text = "Start Capture";
                    capture.Pause();
                }
                else
                {
                    //start the capture
                    captureButton.Text = "Stop";
                    capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }


        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }

        private void horizontal_Click(object sender, EventArgs e)
        {
            if (capture != null) capture.FlipHorizontal = !capture.FlipHorizontal;
        }

        private void vertical_Click(object sender, EventArgs e)
        {
            if (capture != null) capture.FlipVertical = !capture.FlipVertical;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
