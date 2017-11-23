using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Media;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Cvb;


namespace i_2_final
{
    public partial class camera : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //[DllImport("user32.dll")]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
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
        private Capture capture = null;
        private bool _captureInProgress;
        private static BackgroundSubtractor fgDetector;
        private static Emgu.CV.Cvb.CvBlobDetector blobDetector;
        private int minarea = 400;
        private int maxarea = 3000;


        public camera()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;
            try
            {
                capture = global.capture;
                capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            fgDetector = new Emgu.CV.VideoSurveillance.BackgroundSubtractorMOG2();
            blobDetector = new Emgu.CV.Cvb.CvBlobDetector();
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {

            Mat frame = new Mat();
            capture.Retrieve(frame, 0);
            Mat frame_crop = frame;
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


            Hsv hsv_min_color_one = new Hsv(0, 135, 110);
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
            Hsv hsv_min_color_three = new Hsv(83, 109, 105);
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

            //Makers Interpretition
            float[] cent_color_one = new float[2];
            float[] cent_color_two = new float[2];
            float[] cent_color_three = new float[2];
            float[] cent_color_four = new float[2];
            //Centroids of Markers
            foreach (var pair in blobs_color_one)
            {
                CvBlob b = pair.Value;
                CvInvoke.Rectangle(frame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                cent_color_one[0] = b.Centroid.X;
                cent_color_one[1] = b.Centroid.Y;
            }

            foreach (var pair in blobs_color_two)
            {
                CvBlob b = pair.Value;
                CvInvoke.Rectangle(frame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                cent_color_two[0] = b.Centroid.X;
                cent_color_two[1] = b.Centroid.Y;
            }

            foreach (var pair in blobs_color_three)
            {
                CvBlob b = pair.Value;
                CvInvoke.Rectangle(frame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                cent_color_three[0] = b.Centroid.X;
                cent_color_three[1] = b.Centroid.Y;
            }

            foreach (var pair in blobs_color_four)
            {
                CvBlob b = pair.Value;
                CvInvoke.Rectangle(frame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                cent_color_four[0] = b.Centroid.X;
                cent_color_four[1] = b.Centroid.Y;
            }
            #endregion


            #region Calculation
            int click_flag = 0;
            int[] x_cor = new int[4];
            int[] y_cor = new int[4];

            if (blobs_color_one.Count != 0 && blobs_color_two.Count != 0 && blobs_color_three.Count != 0 && blobs_color_four.Count != 0)
            {
                foreach (var pair in blobs_color_one)
                {
                    CvBlob b = pair.Value;
                    foreach (var pairr in blobs_color_two)
                    {
                        CvBlob c = pairr.Value;
                        if ((b.Centroid.X - c.Centroid.X) * (b.Centroid.X - c.Centroid.X) + (b.Centroid.Y - c.Centroid.Y) * (b.Centroid.Y - c.Centroid.Y) <= 5000)
                        {
                            click_flag = 1;
                            x_cor[0] = ((int)b.Centroid.X);
                            x_cor[1] = ((int)c.Centroid.X);
                            y_cor[0] = ((int)b.Centroid.Y);
                            y_cor[1] = ((int)c.Centroid.Y);
                            break;
                        }
                    }
                    if (click_flag == 1)
                        break;
                }
                if (click_flag == 1)
                {
                    click_flag = 0;
                    foreach (var pair in blobs_color_three)
                    {
                        CvBlob b = pair.Value;
                        foreach (var pairr in blobs_color_four)
                        {
                            CvBlob c = pairr.Value;
                            if ((b.Centroid.X - c.Centroid.X) * (b.Centroid.X - c.Centroid.X) + (b.Centroid.Y - c.Centroid.Y) * (b.Centroid.Y - c.Centroid.Y) <= 10000)
                            {
                                click_flag = 1;
                                x_cor[2] = ((int)b.Centroid.X);
                                x_cor[3] = ((int)c.Centroid.X);
                                y_cor[2] = ((int)b.Centroid.Y);
                                y_cor[3] = ((int)c.Centroid.Y);
                                break;
                            }
                        }
                        if (click_flag == 1)
                            break;
                    }
                }
            }

            if (click_flag == 1)
            {
                //MessageBox.Show("clicked");
                SoundPlayer simpleSound = new SoundPlayer(@"click_sound.wav");
                simpleSound.Play();

                Array.Sort(x_cor);
                Array.Sort(y_cor);
                Bitmap ori_image = frame_crop.ToImage<Bgr, Byte>().ToBitmap();
                Bitmap crop_image = new Bitmap(x_cor[2] - x_cor[1], y_cor[2] - y_cor[1]);
                Graphics g = Graphics.FromImage(crop_image);
                g.DrawImage(ori_image, -x_cor[1], -y_cor[1]);
                //string name = string.Format("SAP_{0:ddMMyyyy_hh_mm_ss}.jpg",DateTime.Now);
                frame.Save(@"C:\Users\Shubhankar\Pictures\Camera Roll\" + string.Format("SAP_{0:ddMMyyyy_hh_mm_ss}_original.jpg", DateTime.Now));
                crop_image.Save(@"C:\Users\Shubhankar\Pictures\Camera Roll\" + string.Format("SAP_{0:ddMMyyyy_hh_mm_ss}.jpg", DateTime.Now));
                Thread.Sleep(500);
            }
            #endregion

            #region Click Gesture


            #endregion

            captureImageBox.Image = frame;
            grayscaleImageBox.Image = color_one;
            smoothedGrayscaleImageBox.Image = color_two;
            cannyImageBox.Image = color_three;
            Color4ImageBox.Image = color_four;

        }




        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
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

        private void horizontal_Click(object sender, EventArgs e)
        {
            if (capture != null) capture.FlipHorizontal = !capture.FlipHorizontal;
        }

        private void vertical_Click(object sender, EventArgs e)
        {
            if (capture != null) capture.FlipVertical = !capture.FlipVertical;
        }

      
    }
}
