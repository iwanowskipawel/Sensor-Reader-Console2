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
        private SerialPort port = new SerialPort("COM8", 1200, Parity.None, 8, StopBits.One);
        //send command interval time in seconds
        private bool notReceive = false;
        private Timer timer1 = new Timer();
        private int interval = 10000;

        [STAThread]
        static void Main(string[] args)
        {
            // Instatiate this class
            new Program();
        }
        private void Delay()
        {
            double d = 1.1;
            for (int i = 0; i < 100000000; i++)
            {
                d = Math.Sqrt(d);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            notReceive = true;
            Delay();
            port.Write("5");
            Delay();
            port.Write(";");
            Delay();
            port.Write("*");
            Delay();
            port.Write("\r");
            port.DiscardInBuffer();
            notReceive = false;
        }

        private Program()
        {
            Console.WriteLine("Podaj Interwał [s]:");
            interval = int.Parse(Console.ReadLine())*1000;
            Console.WriteLine("Incoming Data:");
            timer1.Tick += new System.EventHandler(timer1_Tick);
            
            // Attach a method to be called when there
            // is data waiting in the port's buffer
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

            // Begin communications
            port.Open();
            timer1.Interval = interval;
            timer1.Start();
            Application.Run();
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if(!notReceive)
            { 
                string dataFromPort = port.ReadExisting();
                // Show all the incoming data in the port's buffer
                //Console.WriteLine(dataFromPort);
                Console.WriteLine(DateTime.Now.ToString()+ ";" + dataFromPort);// ToLongTimeString());
            }
        }

    }
}
