//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Diagnostics;
using Emgu.CV.VideoSurveillance;
using Emgu.Util;
using System.IO;

using System.Net.Sockets;
using Emgu.CV.UI;

namespace Multiple_cameras
{
    public partial class Form1 : Form
    {
        private ImageBox foreground;
        private Capture _capture;
        private MotionHistory _motionHistory;
        private BackgroundSubtractor _forgroundDetector;
        Stopwatch time;
        private long ellapsed_time = 2000;
        private long pixel_count = 20000000;
        public Form1(ImageBox image)
        {
            InitializeComponent();
            foreground = image;
            time = new Stopwatch();
            //try to create the capture
            if (_capture == null)
            {
                try
                {
                    _capture = new Capture(0);
                      time.Start();
                }
                catch (NullReferenceException excpt)
                {   //show errors if there is any
                    MessageBox.Show(excpt.Message);
                }
            }

            if (_capture != null) //if camera capture has been successfully created
            {

                _motionHistory = new MotionHistory(
                    0.01, //in second, the duration of motion history you wants to keep
                    0.05, //in second, any motion in interval more than this will not be considered
                    0.05); //in second, any motion in interval less than this will not be considered

                _capture.ImageGrabbed += ProcessFrame;
                _capture.Start();
            }
        }

        private Mat _segMask = new Mat();
        private Mat _forgroundMask = new Mat();
        Mat ff = new Mat();
        private void ProcessFrame(object sender, EventArgs e)
        {
            Mat image = new Mat();

            _capture.Retrieve(image);
            if (_forgroundDetector == null)
            {
                _forgroundDetector = new BackgroundSubtractorMOG2();
            }

            _forgroundDetector.Apply(image, _forgroundMask);

            capturedImageBox.Image = image;

            //update the motion history
            _motionHistory.Update(_forgroundMask);

            foreground.Image = _forgroundMask;


            #region get a copy of the motion mask and enhance its color
            double[] minValues, maxValues;
            Point[] minLoc, maxLoc;
            _motionHistory.Mask.MinMax(out minValues, out maxValues, out minLoc, out maxLoc);
            Mat motionMask = new Mat();
            using (ScalarArray sa = new ScalarArray(255.0 / maxValues[0]))
                CvInvoke.Multiply(_motionHistory.Mask, sa, motionMask, 1, DepthType.Cv8U);
            //Image<Gray, Byte> motionMask = _motionHistory.Mask.Mul(255.0 / maxValues[0]);
            #endregion

            //create the motion image 
          //  Image<Bgr, Byte> motionImage = new Image<Bgr, byte>(motionMask.Size);
            //display the motion pixels in blue (first channel)
            //motionImage[0] = motionMask;
           // CvInvoke.InsertChannel(motionMask, motionImage, 0);

            //Threshold to define a motion area, reduce the value to detect smaller motion
            // double minArea = 100;

            //storage.Clear(); //clear the storage
            Rectangle[] rects;
            using (VectorOfRect boundingRect = new VectorOfRect())
            {
                _motionHistory.GetMotionComponents(_segMask, boundingRect);
                rects = boundingRect.ToArray();
            }
          
            //iterate through each of the motion component
            foreach (Rectangle comp in rects)
            {
                
                time.Start();
                // find the angle and motion pixel count of the specific area
                double angle, motionPixelCount;
                _motionHistory.MotionInfo(_forgroundMask, comp, out angle, out motionPixelCount);


                if (Main.security.Text == "SECURITY MODE ON")
                {
                    long x = time.ElapsedMilliseconds;
                    if (x > ellapsed_time)
                        if (motionPixelCount > pixel_count)
                        {
                            //MessageBox.Show("My message here");
                            Console.Beep(5000, 1000);
                            if (Main.connected == true)
                                chat.send(Encoding.ASCII.GetBytes("Someone is in the room"));
                            break;
                            time.Stop();
                        }
                }
            }



        }

        protected override void Dispose(bool disposing)
        {

            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //_capture.Stop();
        }
    }
}

