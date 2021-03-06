﻿using System;
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
        private SerialPort port = new SerialPort("COM4",
          1200, Parity.None, 8, StopBits.One);
        //send command interval time in seconds
        private int interval = 60; 

        [STAThread]
        static void Main(string[] args)
        {
            // Instatiate this class
            new Program();
        }

        private Program()
        {
            Console.WriteLine("Incoming Data:");

            // Attach a method to be called when there
            // is data waiting in the port's buffer
            port.DataReceived += new
              SerialDataReceivedEventHandler(port_DataReceived);

            // Begin communications
            port.Open();

            Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(this.interval * 1000);
                port.Write("5;*"); //command to send to the sensor
            });
            

            // Enter an application loop to keep this thread alive
            Application.Run();
        }

        private void port_DataReceived(object sender,
          SerialDataReceivedEventArgs e)
        {

            string dataFromPort = port.ReadExisting();
            // Show all the incoming data in the port's buffer
            Console.WriteLine(dataFromPort);

            StreamWriter writer = new StreamWriter("sensor.txt");
            writer.WriteLine(dataFromPort);
        }
    }
}
