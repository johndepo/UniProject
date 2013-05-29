using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Assignment1_Server
{
    class SerListener
    {
        static void Main(string[] args)
        {
            IPAddress hostIP;
            String hostName;
            Int16 serverPort = 1234;
            TcpListener serListener;
            TcpClient serConnection;
            Thread childThread;
            DateTime dayTime;
            string childName;
            SerService serviceObj;
            string stop = "N";

            //inser serverport

            hostName = Dns.GetHostName();
            hostIP = Dns.GetHostAddresses(hostName)[0];

            while (stop != "Y")
            {
                try
                {
                    serListener = new TcpListener(hostIP, serverPort);
                    serListener.Start();
                    Console.WriteLine("Echo server by host " + hostName + " at serverPort " + Convert.ToString(serverPort));

                    while (true)
                    {
                        serConnection = serListener.AcceptTcpClient();
                        serviceObj = new SerService();
                        childThread = new Thread(new ThreadStart(serviceObj.Service));
                        dayTime = DateTime.Now;
                        childName = "C-" + dayTime.Minute.ToString() + dayTime.Second.ToString() + dayTime.Millisecond.ToString();
                        serviceObj.ChildName = childName;
                        serviceObj.SerConnection = serConnection;
                        childThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error server Main: " + ex.Message);
                    Console.Write("Stop (Y): ");

                    stop = Console.ReadLine();
                    stop = stop.ToUpper();
                }
            }
        }
    }
}
