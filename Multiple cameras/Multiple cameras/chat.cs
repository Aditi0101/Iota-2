using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.IO;
namespace Multiple_cameras
{
    public partial class chat : Form
    {
        public static Socket sckCommunication;
        public static EndPoint epLocal, epRemote;
        public static bool connected = false;
        public static string ip = null;
        public static PictureBox pic = new PictureBox();
        public chat()
        {
            InitializeComponent();
            pic = p;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {


            send(Encoding.ASCII.GetBytes(text.Text));

        }
        public static void send(byte[] text)
        {

            //Send msgs to server
            if (connected == false)
            {

                sckCommunication = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sckCommunication.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //Connect to server
                //dusre k yahin
                epRemote = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Convert.ToInt32("12"));
                sckCommunication.Connect(epRemote);
                 MessageBox.Show("Connected to Server");
                connected = true;
            }


            byte[] buffer = text;
            sckCommunication.Send(buffer, buffer.Length, SocketFlags.None);       
            //MessageBox.Show("msg sent");

           
                    

        }
        

        

    }
}
