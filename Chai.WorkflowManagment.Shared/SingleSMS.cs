
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace Chai.WorkflowManagment.Shared
{
    public class SingleSMS
    {
        public AutoResetEvent receiveNow;

        public SerialPort OpenPort(SerialPort serialPort)
        {
            SerialPort serialP = null;
            try
            {
                //serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                serialPort.Open();
                serialP= serialPort;
                serialPort.Close();
            }
            catch
            {
               
            }
            return serialP;

        }

        static AutoResetEvent readNow = new AutoResetEvent(false);

        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                {
                    receiveNow.Set();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool sendMsg(SerialPort port, string PhoneNo, string Message)
        {
            receiveNow = new AutoResetEvent(false);

            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived); 

            bool isSend = false;

            try
            {
               
                string recievedData = ExecCommand(port, "AT", 300, "No phone connected");
               
              
                recievedData = ExecCommand(port, "AT+CMGF=1", 300, "Failed to set message format.");
                String command = "AT+CMGS=\"" + PhoneNo + "\"";
                recievedData = ExecCommand(port, command, 300, "Failed to accept phoneNo");
                command = Message + char.ConvertFromUtf32(26) + "\r";
                recievedData = ExecCommand(port, command, 30000, "Failed to send message"); //3 seconds
                if (recievedData.EndsWith("\r\nOK\r\n"))
                {
                    isSend = true;
                }
                else if (recievedData.Contains("ERROR"))
                {
                    isSend = false;
                }
                return isSend;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        public string ExecCommand(SerialPort port, string command, int responseTimeout, string errorMessage)
        {
            try
            {

                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                receiveNow.Reset();
                port.Write(command + "\r");

                string input = ReadResponse(port, responseTimeout);
                //if ((input.Length == 0) || ((!input.EndsWith("\r\n> ")) && (!input.EndsWith("\r\nOK\r\n"))))
                    //throw new ApplicationException("No success message was received.");
                return input;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Receive data from port
      

        public string ReadResponse(SerialPort port, int timeout)
        {
           
            string buffer = string.Empty;
            try
            {
                do
                {
                    if (receiveNow.WaitOne(timeout, false))
                    {
                        string t = port.ReadExisting();
                        buffer += t;
                    }
                    else
                    {
                        if (buffer.Length > 0)
                            throw new ApplicationException("Response received is incomplete.");
                        else
                            throw new ApplicationException("No data received from phone.");
                    }
                }
                while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\n> ") && !buffer.EndsWith("\r\nERROR\r\n"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer;
        }


    }
}
