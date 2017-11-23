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
using System.Net.Sockets;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Cvb;
using System.Net;
using System.IO;
using System.IO.Ports;
using System.Drawing.Imaging;


namespace Multiple_cameras
{
    public partial class Main : Form
    {
        
        public static Capture capture1, capture2;
        private int i = 0; 
        Thread speech, motion;
        public static Button security;
        public static ListBox list;
        public static Socket sckCommunication;
        public static EndPoint epLocal, epRemote;
        public static bool connected = false;
        public static string ip = null;

        public Main()
        {
            InitializeComponent();
            inti();
            security = button1;
            try
            {
                capture1 = new Capture(0);       //for room
                capture2 = new Capture(1);      //at the gate
               

                capture1.Start();
                capture2.Start();
               

            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            Application.Idle += ProcessFrame;
            
            //Thread for speech module
     //       speech =new Thread( new ThreadStart(speechload));
     //       speech.Start();

            //Thread for motion module
             motion = new Thread(new ThreadStart(motionload));
            motion.Start();

            //Thread for recieving messages from user
           Thread recieve = new Thread(new ThreadStart(recieveload));
           recieve.Start();

           Thread recieve_door = new Thread(new ThreadStart(recievedoor));
           recieve_door.Start();
       
        }

        public void inti()
        {
            //MessageBox.Show("Serial Communication Started");
            global.port = new SerialPort();
            global.port.PortName = "COM3";
            global.port.BaudRate = 9600;
            global.port.DataBits = 8;
            global.port.StopBits = System.IO.Ports.StopBits.One;
            global.port.Parity = System.IO.Ports.Parity.None;
            try
            {
                global.port.Open();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }//

        }

        private void motionload()
        {
            Form1 f = new Form1(foregroundImageBox);
            Thread.Sleep(1000);
            
        }
       
        private void recieveload()
        {
            recieve r = new recieve();
           
        }

        private void recievedoor()
        {
            while (true)
            {
                if (global.port.IsOpen)
                {
                    if (global.port.ReadChar() == 's')
                    {
                        Mat x = capture1.QueryFrame();
                        Image<Bgr, Byte> ImageFrame = x.ToImage<Bgr, Byte>();
                        pictureBox1.Image = ImageFrame.ToBitmap();
                        Bitmap bmp = new Bitmap(ImageFrame.ToBitmap());
                        Image img = pictureBox1.Image;
                        byte[] ByteImage = ConvertImageToByteArray(img);
                        chat.send(ByteImage);
                    }
                }
                Thread.Sleep(1000);
            }

        }
       

        private void ProcessFrame(object sender, EventArgs arg)
        {

            Mat frame = capture1.QueryFrame();
            Mat frame2 = capture2.QueryFrame();
          //  Mat frame3 = capture3.QueryFrame();
            
            imageBox1.Image = frame;
            imageBox2.Image = frame2;
           // imageBox3.Image = frame;


        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //capture1.Stop();
            //capture2.Stop();
           
        }

      
        

        private void button1_Click(object sender, EventArgs e)
        {
            //motion.Start();
            if (i == 1)
                security.Text = "SECURITY MODE ON";
            else
                security.Text = "SECURITY MODE OFF";
            i = (i + 1) % 2;
        }

        

        private void button2_Click_1(object sender, EventArgs e)
        {
            Mat x = capture1.QueryFrame();
            Image<Bgr, Byte> ImageFrame = x.ToImage<Bgr, Byte>();
            pictureBox1.Image = ImageFrame.ToBitmap();
            Bitmap bmp = new Bitmap(ImageFrame.ToBitmap());
            Image img = pictureBox1.Image;
            byte[] ByteImage = ConvertImageToByteArray(img);
            chat.send(ByteImage);
            
        }
        public static byte[] ConvertImageToByteArray(Image _image)
        {
            byte[] ImageByte;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    _image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ImageByte = ms.ToArray();
                }
            }
            catch (Exception) 
            {
                throw;
            }
            return ImageByte;
        }


    }
}
