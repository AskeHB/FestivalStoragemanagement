using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnivalSoragemanagement.UI
{
    class RfidReader
    {
        private static RfidReader instance = null;
        private SerialPort RfidPort = null;

        private RfidReader()
        {
            SetPort();
        }

        public static RfidReader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RfidReader();
                }
                return instance;
            }
        }
        public void SetPort()
        {
            string portname = "";

            foreach (string port in SerialPort.GetPortNames())
            {
                SerialPort p = new SerialPort(port);
                try
                {
                    p.Open();
                    p.ReadExisting();

                    p.Write("C");
                    string data = p.ReadLine();
                    if (data == "RFID\r")
                    {
                        portname = port;
                        p.Close();
                        break;
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    p.Close();
                }
            }
            if (!string.IsNullOrEmpty(portname))
            {
                RfidPort = new SerialPort(portname, 9600);
                RfidPort.DataReceived += RfidPort_DataReceived;
                RfidPort.Open();
            }

        }

        private void RfidPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
