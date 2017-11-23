using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace Multiple_cameras
{
    public partial class recieve : Form
    {
        Socket sckCommunication;
        EndPoint epLocal;


        // Incoming data from the client.
        public static string data = null;

        // buffer to receive info
        byte[] bytes = new Byte[1024];
        public recieve()
        {
            InitializeComponent();

            sckCommunication = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sckCommunication.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //my ip and port goes here
            epLocal = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Convert.ToInt32("45"));
            sckCommunication.Bind(epLocal);
            sckCommunication.Listen(100);
            MessageBox.Show("Server Started");
            while (true)
            {
                try
                {
                    // Start listening for connections.

                    Socket handler = sckCommunication.Accept();
                    data = null;

                    // An incoming connection needs to be processed.
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        MessageBox.Show(data);
                        if (data.Equals("Yes"))
                        {
                            //open the door
                            if (global.port.IsOpen)
                            {
                                global.port.WriteLine("d");
                                MessageBox.Show("door opened");
                                Thread.Sleep(150);
                            }
                        }

                    }

                }

                catch (Exception exi)
                {
                    MessageBox.Show("Error");
                }
            }
        }
    }
}
