using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Collections;
using System.IO.Ports;
using System.IO;
using System.Media;
using System.Diagnostics;


namespace Multiple_cameras
{
    public partial class speech : Form
    {





        private int Index = 0;


       // public SerialPort port;
        public Queue<string> speech_his;

        public speech()
        {
            InitializeComponent();
            inti();
            MessageBox.Show("yezzz");
            speech_his = new Queue<string>();
            //SpeechRecognizer sr = new SpeechRecognizer();
            SpeechRecognitionEngine sr = new SpeechRecognitionEngine();
            //sr.LoadGrammar(new Grammar(new GrammarBuilder("test")));
            
                        Choices command = new Choices();
                        command.Add(new string[] {"lights","lights please", "fans please", "door please", "next song", "old songs", "soft songs", "english songs", "coke studio", "previous song", "stop", "play this" });

                        GrammarBuilder gb = new GrammarBuilder();
                        gb.Append(command);

                        Grammar g = new Grammar(gb);

                        sr.LoadGrammar(g);
            
             sr.SetInputToDefaultAudioDevice();
             sr.RecognizeAsync(RecognizeMode.Multiple);


            sr.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);





        }
        
        public void inti()
        {
            MessageBox.Show("inti");
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
            }

        }
        void sr_Speech(object sender, SpeechRecognizedEventArgs e)
        {
            MessageBox.Show(e.Result.Text);
        }

        void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

           //MessageBox.Show(e.Result.Text);
            /*speech_his.Dequeue();
            speech_his.Enqueue(e.Result.Text);
            string[] str = speech_his.ToArray();
            string speech = str[0] + str[1] + str[2];
            //MessageBox.Show(speech);
            listBox1.Items.Add(speech);
            //MessageBox.Show(e.Result.Text);*/
            // string speech = e.Result.Text;
            switch (e.Result.Text.ToString())
            {
                case "lights please":
                    {
                        //MessageBox.Show("e");
                        if (global.port.IsOpen)
                        {
                            global.port.WriteLine("e");
                            //MessageBox.Show("e");
                            Thread.Sleep(150);
                        }
                    }
                    break;
                case "fans please":
                    {
                        if (global.port.IsOpen)
                        {
                            global.port.WriteLine("f");
                          //  MessageBox.Show("f");
                            Thread.Sleep(150);
                        }
                    }
                    break;
                case "door please":
                    {
                        if (global.port.IsOpen)
                        {
                            global.port.WriteLine("d");
                            //MessageBox.Show("d");
                            Thread.Sleep(150);
                        }
                    }
                    break;

                /*if (e.Result.Text == "victoria lights fans please" || e.Result.Text == "victoria fans lights please")
                {
                    if (port.IsOpen)
                    {
                        port.WriteLine("e");
                        Thread.Sleep(400);
                        port.WriteLine("f");
                        Thread.Sleep(150);
                    }
                }*/





            }
        }
    }
}
