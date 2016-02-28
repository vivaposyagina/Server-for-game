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

    class ClientSocket
    {
        public delegate void ClientSocketActionEventHandlerForClientCommand();
        //public delegate void ClientSocketActionEventHandlerForServer(ClientCommand client);
        protected TcpClient tcp;
        byte[] buffer;
        NetworkStream stream;
        int length = 0;        
        protected status currentStatus = status.off;
        public status CurrentStatus
        { 
            get {return currentStatus;}
            set { currentStatus = value; }
        }
        public event ClientSocketActionEventHandlerForClientCommand EventHandlersListForClientCommand;
        //public event ClientSocketActionEventHandlerForServer ClientSocketActionEventHandlerForServerForDisconnected;

        public void StartToWaitMessage()
        {
            Thread th = new Thread(MessageReceiver);
            th.Start();
        }
        public ClientSocket(TcpClient client)
        {
            tcp = client;            
            buffer = new byte[1000];
            stream = tcp.GetStream();
            StartToWaitMessage();
        }
        public int Length
        {
            get
            {
                return length;
            }
        }
        public byte[] Buffer
        {
            get
            {
                return buffer;
            }
        }
        public void ClearBuffer()
        {
            Array.Clear(this.buffer, 0, this.buffer.Length);            
            length = 0;
        }
        void MessageReceiver()
        {
            byte[] localBuffer = new byte[1000];
            while (true)
            {
                try
                { 
                    int lenghtNewMessage = stream.Read(localBuffer, 0, 1000);
                    if (lenghtNewMessage == 0)
                    {
                        // закрыть сокет, написать предупреждение
                        stream.Close();
                        return;
                    }
                    else
                    {
                        lock (buffer)
                        {
                            Array.Copy(localBuffer, 0, buffer, 0, lenghtNewMessage);
                            length = lenghtNewMessage;            
                           
                        }
                    }
                }
                catch (Exception)
                {
                    currentStatus = status.error;
                    //throw new SocketException();
                }
              
                if (EventHandlersListForClientCommand != null)
                { 
                    EventHandlersListForClientCommand();                    
                }
            }
        }

        public string ReceiveMessage()
        {
            lock(buffer)
            {
                string str = ConvertToString(buffer);
                ClearBuffer();
                return str;
            }
        }

        public string DataFromBuffer()
        {            
            return ConvertToString(buffer);
        }

              
        private byte[] ConvertToArrayOfByte(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return bytes;
        }
        private string ConvertToString(byte[] bytes)
        {
            string str = System.Text.Encoding.UTF8.GetString(bytes, 0, length);
            return str;
        }

        public bool Send(string data)
        {
            if (data.Length != 0)
            {
                try
                {
                    byte[] bytes = ConvertToArrayOfByte(data);
                     
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
                catch (Exception)
                {
                    currentStatus = status.error;                                    
                    return false;
                }
               
            }
            //Console.WriteLine("Уровень 3 " + DateTime.Now);
            //Console.WriteLine(DateTime.Now);
            return true;
        }
        public void Disconnect()
        {
            this.tcp.Close();
            currentStatus = status.off;
        }
    }
}
