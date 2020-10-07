using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;

namespace Sensor_Reader_Console
{
    class Program
    {
        // Create the serial port with basic settings
        private SerialPort port = new SerialPort("COM8",
          1200, Parity.None, 8, StopBits.One);
        //send command interval time in seconds
        private bool notReceive = false;
        private Timer timer1 = new Timer();
        private int interval = 10000;
        private StreamWriter sw;
        private string answer = "";
        private string fileName = "readerlog";

        [STAThread]
        static void Main(string[] args)
        {
            // Instatiate this class
            new Program();
        }

        private void Delay()
        {
            System.Threading.Thread.Sleep(1000); //wait one second
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] commands = new string[] { "5", ";", "*", "\r" };
            
            notReceive = true;
            Array.ForEach(commands, el =>
            {
                Delay();
                port.Write(el);
            });
            port.DiscardInBuffer();
            notReceive = false;
        }

        private Program()
        {
            Console.WriteLine("Podaj Interwał [s]:");
            interval = int.Parse(Console.ReadLine()) * 1000;
            Console.WriteLine("Podaj nazwę pliku:");
            fileName = Console.ReadLine();
            Console.WriteLine("Incoming Data:");

            //this.sw = new StreamWriter(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/readerlog.txt");
            //this.sw = new StreamWriter("D:/readerlog.txt");
            timer1.Tick += new System.EventHandler(timer1_Tick);

            // Attach a method to be called when there
            // is data waiting in the port's buffer
            port.DataReceived += new
              SerialDataReceivedEventHandler(port_DataReceived);

            // Begin communications
            port.Open();
            timer1.Interval = interval;
            timer1.Start();
            Application.Run();
        }

        private void port_DataReceived(object sender,
          SerialDataReceivedEventArgs e)
        {
            //if(!notReceive)
            //{ 
            //    string dataFromPort = port.ReadExisting();
            //    string dataToWrite = DateTime.Now.ToString() + ";" + dataFromPort;
            //    // Show and write all the incoming data in the port's buffer
            //    answer += "\n" + dataToWrite;

            //    this.sw = new StreamWriter(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/" 
            //        + fileName + ".txt", true);
            //    sw.WriteLine(answer);
            //    sw.Close();

            //    Console.WriteLine(dataToWrite);
            //}

            if (!notReceive)
            {
                string dataFromPort = port.ReadExisting();
                string dataToWrite = "";

                if (dataFromPort == null
                    || dataFromPort == "\r"
                    || dataFromPort != "\u2191")
                {
                    dataToWrite += DateTime.Now.ToString() + "; ";
                    answer += "\n" + dataToWrite;
                    Console.Write("\n" + dataToWrite);
                }
                else
                {
                    answer += dataFromPort;
                    Console.Write(dataFromPort);
                }

                this.sw = new StreamWriter(System.IO.Path.GetDirectoryName(Application.ExecutablePath) +
                    "/" + fileName + ".txt");
                sw.Write(answer);
                sw.Close();
            }
        }
      
    }
}
