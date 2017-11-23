using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Cvb;
using HandGestureRecognition.SkinDetector;

namespace i_2_final
{
    public partial class gesture : Form
    {

        IColorSkinDetector skinDetector;
        private static BackgroundSubtractor fgDetector;
        private static Emgu.CV.Cvb.CvBlobDetector blobDetector;
        Image<Bgr, Byte> currentFrame;
        Image<Bgr, Byte> currentFrameCopy;
        Capture grabber;
        int frameWidth;
        int frameHeight;
        Hsv hsv_min;
        Hsv hsv_max;
        Ycc YCrCb_min;
        Ycc YCrCb_max;
        Image<Gray, Byte> croped;
        Image<Gray, Byte> croped1;
        Image<Gray, Byte> croped2;
        string path;
        int[] na = new int[11];
        int fol = 0;
        private gesture_recog fow_prop;
        int gesture_number;
        int index;

        public gesture()
        {
            InitializeComponent();
            fow_prop = new gesture_recog();
            fow_prop.Show();
            //CvInvoke.UseOpenCL = false;
            try
            {
                grabber = global.capture;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            grabber.QueryFrame();
            frameWidth = grabber.Width;
            frameHeight = grabber.Height;
            //   detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);
            hsv_min = new Hsv(0, 45, 0);
            hsv_max = new Hsv(20, 254, 254);
            // YCrCb_min = new Ycc(0, 131, 80);
            //YCrCb_max = new Ycc(255, 185, 135);
            YCrCb_min = new Ycc(0, 130, 80);
            YCrCb_max = new Ycc(255, 185, 135);
            index = 0;
            for (int i = 0; i < 10; i++)
            {
                na[i] = 1;
            }
            fgDetector = new BackgroundSubtractorMOG2();
            blobDetector = new Emgu.CV.Cvb.CvBlobDetector();
            Application.Idle += new EventHandler(FrameGrabber);

        }


        void FrameGrabber(object sender, EventArgs e)
        {
            Mat x = new Mat();
            //currentFrame = CvInvoke.grabber.QueryFrame().ToImage<Bgr,Byte>;
            x = grabber.QueryFrame();
            currentFrame = x.ToImage<Bgr, byte>();
            if (currentFrame != null)
            {
                currentFrameCopy = currentFrame.Copy();

                // Uncomment if using opencv adaptive skin detector
                //Image<Gray,Byte> skin = new Image<Gray,byte>(currentFrameCopy.Width,currentFrameCopy.Height);                
                //detector.Process(currentFrameCopy, skin);                

                skinDetector = new YCrCbSkinDetector();
                //skinDetector = new HsvSkinDetector();
                Image<Gray, Byte> skin = skinDetector.DetectSkin(currentFrameCopy, YCrCb_min, YCrCb_max);

                ExtractBlobAndCrop(skin);


                //imageBoxSkin.Image = skin;
                //imageBox1.Image = skin;
                //imageBoxFrameGrabber.Image = skin;
            }
        }



        private void ExtractBlobAndCrop(Image<Gray, byte> skin)
        {
            using (MemStorage storage = new MemStorage())
            {
                Image<Gray, Byte> smoothedFrame = new Image<Gray, byte>(skin.Size);
                CvInvoke.GaussianBlur(skin, smoothedFrame, new Size(3, 3), 1); //filter out noises


                imageBoxFrameGrabber.Image = skin;


                Mat forgroundMask = new Mat();
                Mat ss = new Mat();
                ss = skin.Mat;
                //grabber.Retrieve(ss);
                fgDetector.Apply(ss, forgroundMask);

                //imageBox1.Image = forgroundMask;

                CvBlobs blobs = new CvBlobs();
                //blobDetector.Detect(forgroundMask.ToImage<Gray, byte>(), blobs);
                blobDetector.Detect(skin, blobs);
                blobs.FilterByArea(30000, 150000);

                CvBlob b = null;
                CvBlob btemp;
                int area = 0;
                foreach (var pair in blobs)
                {
                    btemp = pair.Value;
                    if (area < btemp.Area)
                    {
                        b = pair.Value;
                        area = btemp.Area;
                    }
                }

                //Crop LArgest Blob
                Bitmap skin_bit = skin.ToBitmap();


                //MessageBox.Show("" + area);
                if (area != 0)
                {
                    CvInvoke.Rectangle(currentFrame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                    //Rectangle rec = new Rectangle(b.BoundingBox.X, b.BoundingBox.Y, b.BoundingBox.Width, b.BoundingBox.Height);

                    Bitmap crop_image = new Bitmap((b.BoundingBox.Width > b.BoundingBox.Height ? b.BoundingBox.Width : b.BoundingBox.Height), (b.BoundingBox.Width > b.BoundingBox.Height ? b.BoundingBox.Width : b.BoundingBox.Height));
                    //Bitmap crop_image = skin_bit.Clone(rec, skin_bit.PixelFormat);
                    Graphics g = Graphics.FromImage(crop_image);
                    g.DrawImage(skin_bit, -b.BoundingBox.X, -b.BoundingBox.Y);
                    //g.DrawImage(skin_bit, -50, -50);


                    croped = new Image<Gray, Byte>(crop_image).Resize(350, 350, Inter.Cubic);
                    croped1 = new Image<Gray, Byte>(crop_image).Resize(100, 100, Inter.Cubic);
                    croped2 = new Image<Gray, Byte>(crop_image).Resize(50, 50, Inter.Cubic);


                    int gesture_number = fow_prop.image(croped2);
                    label1.Text = "" + gesture_number;


                    imageBoxSkin.Image = croped;
                    crop_image.Dispose();
                    skin_bit.Dispose();

                }
            }
        }
    }
}