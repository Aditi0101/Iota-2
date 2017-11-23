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

namespace i_2_final
{
    public partial class reciever : Form
    {
        Socket sckCommunication;
        EndPoint epLocal, epRemote;
        Socket sckCommunication_s;
        EndPoint epLocal_s, epRemote_s;
        bool connect = false;
        bool connect_s = false;
        // Incoming data from the client.
        public static string data = null;

        // buffer to receive info
        byte[] bytes = new Byte[1024];
        public reciever()
        {
            InitializeComponent();
            if (!connect)
            {
                sckCommunication = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sckCommunication.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //apnni
                epLocal = new IPEndPoint(IPAddress.Parse("192.168.65.52"), Convert.ToInt32("12"));
                sckCommunication.Bind(epLocal);
                MessageBox.Show("connected");
            }

            ///
                

            while (true)
            {
                sckCommunication.Listen(100);

                try
                {
                    // Start listening for connections.

                    Socket handler = sckCommunication.Accept();
                    data = null;

                    // An incoming connection needs to be processed.

                    /*   bytes = new byte[1024];
                         int bytesRec = handler.Receive(bytes);
                         data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                      */


                    while (true)
                    {
                        byte[] buffer = new byte[10000000];
                        handler.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                        // FileStream fs = File.Create("1.jpg");

                        //  fs.Write(buffer, 0, buffer.Length);
                        Image newImage;
                        using (MemoryStream mem = new MemoryStream(buffer, 0, buffer.Length))
                        {
                            newImage = System.Drawing.Image.FromStream(mem);
                        }
                        pictureBox1.Image = newImage;
                        this.Show();
                        //MessageBox.Show("Done");
                        DialogResult dialogResult = MessageBox.Show("Sure", "Allow this person to enter??", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (!connect_s)
                            {
                                sckCommunication_s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                sckCommunication_s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                //Connect to server
                                //dusre k yahin
                                epRemote_s = new IPEndPoint(IPAddress.Parse("192.168.65.211"), Convert.ToInt32("45"));
                                sckCommunication_s.Connect(epRemote_s);
                                //MessageBox.Show("Connected to Server");
                            }
                            string sy = "Yes";


                            byte[] buffer1 = Encoding.ASCII.GetBytes(sy);
                            sckCommunication_s.Send(buffer1, buffer1.Length, SocketFlags.None);
                            /////////////////////////////////////////////////
                            this.Hide();
                            break;
                            //MessageBox.Show("hohoho");
                        }
                        else
                        {
                            if (connect_s)
                            {
                                sckCommunication_s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                                sckCommunication_s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                                //Connect to server
                                //dusre k yahin
                                epRemote_s = new IPEndPoint(IPAddress.Parse("191.168.65.211"), Convert.ToInt32("45"));
                                sckCommunication_s.Connect(epRemote_s);
                              //  MessageBox.Show("Connected to Server");
                            }
                            string sy = "No";


                            byte[] buffer1 = Encoding.ASCII.GetBytes(sy);
                            sckCommunication_s.Send(buffer1, buffer1.Length, SocketFlags.None);

                        }
                        
                    }


                }

                catch (Exception ex)
                {
                    MessageBox.Show("Someone breached your security. Please take action.");
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
                   
        }
    }
}

