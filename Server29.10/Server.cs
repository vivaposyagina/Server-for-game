using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server29._10
{
    class Server
    {
        GameData dataOfThisGame;
        ServerCommand serverCommand;
        Dictionary<int, ClientCommand> clients;
        Dictionary<int, string> listOfPlayersAndTheirNickname;        
        public status currentStatus = status.off;

        public Server()
        {
            serverCommand = new ServerCommand();
            serverCommand.EventHandlerListForServer += new ServerSocket.TcpClientActionEventHandler(AddNewClientCommand);            
            clients = new Dictionary<int, ClientCommand>();
            listOfPlayersAndTheirNickname = new Dictionary<int, string>();            
        }

        public void Process(ClientCommand client)
        {        
            BaseCommand bcmd = client.ReceiveLastCommand();
            switch (bcmd.ID)
            {
                case 2:                    
                    Intro command = bcmd as Intro;
                    Response answer = null;
                    if (dataOfThisGame.phaseOfGame == phase.game || dataOfThisGame.phaseOfGame == phase.result)
                    {
                        answer = new Response("notok", "Game has already start");
                        client.SendNewCommand(answer as BaseCommand);
                        break;
                    }
                    bool flag = true;
                    
                    foreach (KeyValuePair<int, string> player in listOfPlayersAndTheirNickname)
                    {
                        if (player.Value == command.Name)
                        {
                           answer = new Response("notok", "Player with this nickname already exists");
                           client.SendNewCommand(answer as BaseCommand);
                           flag = false;
                           break;
                        }
                    }
                    if (flag)
                    {
                        answer = new Response("ok", "welcome, " + command.Name);
                        listOfPlayersAndTheirNickname.Add(client.id, command.Name);
                        clients[client.id].CurrentStatus = status.on;
                        dataOfThisGame.AddNewPlayer(command.Name);


                        client.SendNewCommand(answer as BaseCommand);
                        client.SendNewCommand(dataOfThisGame.FormCommandOfTimeLeft() as BaseCommand);
                        
                        for (int i = 1; i < ClientCommand.nextID; i++)
                        {
                            if (clients.ContainsKey(i) && clients[i].CurrentStatus == status.on)
                            {
                                clients[i].SendNewCommand(dataOfThisGame.FormCommandOfPlayersList() as BaseCommand);                                
                            }
                        }
                        
                    }
                
                    break;
                case 4:
                    Chat messageCommand = bcmd as Chat;
                    for (int i = 1; i < ClientCommand.nextID; i++)
                    {
                        if (clients.ContainsKey(i) && clients[i].CurrentStatus == status.on)
                        {
                            clients[i].SendNewCommand(messageCommand as BaseCommand);
                        }
                    } 
                    break;
                case 10:
                    PlayerMove movement = bcmd as PlayerMove;
                    for (int i = 1; i < ClientCommand.nextID; i++)
                    {
                        if (clients.ContainsKey(i) && clients[i].CurrentStatus == status.on && client == clients[i])
                        {
                            dataOfThisGame.PlayerMoved(movement.Direction, listOfPlayersAndTheirNickname[i]);
                        }
                    }                  
                    
                    for (int i = 1; i <= ClientCommand.nextID; i++)
                    {
                        if (clients.ContainsKey(i) && clients[i].CurrentStatus == status.on)
                        {
                            if (client == clients[i])
                            {
                                client.SendNewCommand(dataOfThisGame.FormCommandOfPlayerCoords(listOfPlayersAndTheirNickname[i]) as BaseCommand);
                                client.SendNewCommand(dataOfThisGame.FormCommandOfVisibleObjects(listOfPlayersAndTheirNickname[i]) as BaseCommand);
                            }
                            clients[i].SendNewCommand(dataOfThisGame.FormCommandOfVisiblePlayers(listOfPlayersAndTheirNickname[i]) as BaseCommand);
                       }
                       
                    }
                    break;
                case 12:
                    PlayerDisconnect disconnectCommand = bcmd as PlayerDisconnect;
                    
                    for (int j = 1; j <= ClientCommand.nextID; j++)
                    {
                        if (clients.ContainsKey(j) && clients[j] == client)
                        {
                            clients[j].Disconnect();
                            clients.Remove(j);
                            Console.WriteLine("Клиент " + listOfPlayersAndTheirNickname[j] + " отключился");
                            dataOfThisGame.DeletePlayer(listOfPlayersAndTheirNickname[j]);
                            listOfPlayersAndTheirNickname.Remove(j);                            
                        }
                    }
                    client.Disconnect();                    
                    for (int q = 1; q <= ClientCommand.nextID; q++)
                    {
                        if (clients.ContainsKey(q))
                        {
                            clients[q].SendNewCommand(dataOfThisGame.FormCommandOfPlayersList() as BaseCommand);                          
                        }
                    }                      
                    break;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }
        }
        public void CheckClients()
        {
            for (int i = 1; i < ClientCommand.nextID; i++)
            {
                if (clients.ContainsKey(i) && listOfPlayersAndTheirNickname.ContainsKey(i) && clients[i].CurrentStatus == status.error)
                {
                    clients[i].Disconnect();
                    dataOfThisGame.DeletePlayer(listOfPlayersAndTheirNickname[i]);
                    clients.Remove(i);
                    Console.WriteLine("Клиент " + listOfPlayersAndTheirNickname[i] + " перестал отвечать");
                    listOfPlayersAndTheirNickname.Remove(i);
                    for (int j = 1; j <= ClientCommand.nextID; j++)
                    {
                        if (clients.ContainsKey(j))
                        {
                            clients[j].SendNewCommand(dataOfThisGame.FormCommandOfPlayersList() as BaseCommand);
                        }
                    }
                }
            }
        }
        public void WorkerThread()
        {
            Thread workerThread = new Thread(delegate()
            {
                bool WhetherDataIsSentToStartGame = false;
                bool WhetherDataIsSentToFinishGame = false;
                DateTime timeForSendingLeftTime = DateTime.Now.AddSeconds(10);
                while (currentStatus == status.on)
                {
                    Thread.Sleep(30);
                    CheckClients();
                    if (dataOfThisGame.phaseOfGame == phase.waiting || dataOfThisGame.phaseOfGame == phase.game)
                    {
                        if (DateTime.Compare(DateTime.Now, timeForSendingLeftTime) > 0)
                        {
                            SendTimeLeft();
                            timeForSendingLeftTime = timeForSendingLeftTime.AddSeconds(10);
                        }
                    }
                    if (DateTime.Now < dataOfThisGame.TimeOfEndingPhaseGame && DateTime.Now > dataOfThisGame.TimeOfEndingThisWaiting && !WhetherDataIsSentToStartGame)
                    {
                        dataOfThisGame.StartGame();
                        for (int i = 1; i <= ClientCommand.nextID; i++)
                        {
                            if (clients.ContainsKey(i) && clients[i].CurrentStatus == status.on)
                            {
                                clients[i].SendNewCommand(dataOfThisGame.FormCommandOfMapSize() as BaseCommand);
                                //Console.WriteLine(i + "  клиент получил сообщение1 " + DateTime.Now);
                                clients[i].SendNewCommand(dataOfThisGame.FormCommandOfPlayerCoords(listOfPlayersAndTheirNickname[i]) as BaseCommand);
                                //Console.WriteLine(i + "  клиент получил сообщение2 " + DateTime.Now);
                                clients[i].SendNewCommand(dataOfThisGame.FormCommandOfVisibleObjects(listOfPlayersAndTheirNickname[i]) as BaseCommand);
                                //Console.WriteLine(i + "  клиент получил сообщение3 " + DateTime.Now);
                                clients[i].SendNewCommand(dataOfThisGame.FormCommandOfVisiblePlayers(listOfPlayersAndTheirNickname[i]) as BaseCommand);
                                //Console.WriteLine(i + "  клиент получил сообщение4 " + DateTime.Now);
                            }
                        }
                        WhetherDataIsSentToStartGame = true;
                    }

                    if (DateTime.Now > dataOfThisGame.TimeOfEndingPhaseGame && DateTime.Now < dataOfThisGame.TimeOfEndingPhaseResult && !WhetherDataIsSentToFinishGame)
                    {
                        dataOfThisGame.FinishGame();
                        for (int i = 1; i < ClientCommand.nextID; i++)
                        {
                            if (clients.ContainsKey(i) && clients[i].CurrentStatus == status.on)
                            {
                                clients[i].SendNewCommand(dataOfThisGame.FormCommandOfGameOver() as BaseCommand);
                            }
                        }
                        WhetherDataIsSentToFinishGame = true;
                    }
                    if (DateTime.Now > dataOfThisGame.TimeOfEndingPhaseResult)
                    {
                        FinalizationWorkingServer();
                        //InitializationServer();
                    }
                }
            });
                workerThread.Start();
        }
        public void SendTimeLeft()
        {
            TimeLeft tl = dataOfThisGame.FormCommandOfTimeLeft();
            for (int i = 1; i < ClientCommand.nextID; i++)
            {
                if (clients.ContainsKey(i) && clients[i].CurrentStatus == status.on)
                {
                    clients[i].SendNewCommand(tl);
                    //Console.WriteLine(i + " клиент уровень 1 - время");
                }               
            } 
        }
        
     
        public void InitializationServer()
        {
            serverCommand.StartListener();
            dataOfThisGame = new GameData();            
            currentStatus = status.on;
            WorkerThread();
        }
        public void FinalizationWorkingServer()
        {
            serverCommand.StopListener();
            for (int i = 0; i < ClientCommand.nextID; i++)
            {
                if (clients.ContainsKey(i) && clients[i].CurrentStatus == status.on)
                {
                    clients[i].Disconnect();
                }
            }
            currentStatus = status.off;
        }
        public void AddNewClientCommand()
        {
            // сделать проверку
            clients.Add(ClientCommand.nextID, serverCommand.AcceptClientCommand());
            clients.Last().Value.EventHandlersListForServer += new ClientCommand.ClientCommandActionEventHandlerForServer(Process);
        } 
    }
}
