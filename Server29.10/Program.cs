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
    public enum status { on, off, error };
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            Console.WriteLine("Для работы с сервером доступны команды:");
            Console.WriteLine("Start - запуск сервера");
            Console.WriteLine("Stop - остановка работы сервера");            
            while (true)
            {
                String command = Console.ReadLine();
                if (command == "Start")
                {
                    if (server.currentStatus == status.on)
                    {
                        Console.WriteLine("Сервер уже запущен");
                    }
                    else
                    {
                        server.InitializationServer();                                             
                        Console.WriteLine("Сервер успешно запущен");
                        
                    }
                }
                else if (command == "Stop")
                {
                    if (server.currentStatus != status.on)
                    {
                        Console.WriteLine("Сервер уже отключен");
                    }
                    else
                    {
                        server.FinalizationWorkingServer();
                        Console.WriteLine("Сервер отключен");
                    }
                }
                else
                {
                    Console.WriteLine("Неизвестная команда");
                }
            }
        }
    }
}
