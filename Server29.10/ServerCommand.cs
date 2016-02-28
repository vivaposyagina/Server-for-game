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
    class ServerCommand : ServerSocket
    {
        public ClientCommand AcceptClientCommand()
        {
            TcpClient newTcpClient;
            newTcpClient = AcceptTcpClient();
            if (newTcpClient != null)
            {
                ClientCommand newClientCommand = new ClientCommand(newTcpClient);
                return newClientCommand;
            }
            else
            {
                return null;
            }           
        }
    }
}
