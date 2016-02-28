using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace Server29._10
{
    class ServerSocket
    {
        public delegate void TcpClientActionEventHandler();
        protected bool isServerRunning;
        Queue<TcpClient> clients = new Queue<TcpClient>();
        protected TcpListener listener;
        int port = 7777;
        IPEndPoint point;        
        public event TcpClientActionEventHandler EventHandlerListForServer;
        Thread TCPClientAccepter;

        public void StartListener()
        {
            try
            {
                isServerRunning = true;
                point = new IPEndPoint(IPAddress.Any, port);
                listener = new TcpListener(point);
                listener.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            TCPClientAccepter = new Thread(delegate()
            {
                while (isServerRunning)
                {
                    try
                    {
                        TcpClient tcp = listener.AcceptTcpClient();
                        tcp.NoDelay = true;                   
                        clients.Enqueue(tcp);                        
                        if (EventHandlerListForServer != null) EventHandlerListForServer();
                    }
                    catch (Exception ex)
                    {
                        if (isServerRunning)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
                );
            TCPClientAccepter.Start();
        }
        public void StopListener()
        {
            isServerRunning = false;
            listener.Stop();
            //TCPClientAccepter.Abort();
        }
        public TcpClient AcceptTcpClient()
        {
            return clients.Dequeue();
        }
    }
}
