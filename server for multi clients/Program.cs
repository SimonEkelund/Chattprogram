﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace tbServerMutlipleClients
{
    class Program
    {
        private static Socket _serverSocket;
        private static readonly List<Socket> ClientSockets = new List<Socket>(); // lista med user
        private static readonly List<User> Users = new List<User>();
        private const int BufferSize = 2048;
        private const int Port = 65002;
        private static readonly byte[] Buffer = new byte[BufferSize];
        private static bool _closing;
        private static int received;

        static void Main()
        {
            Console.Title = "Server, stöd för flera klienter";
            SetupServer();
            //Vänta här!
            Console.ReadLine();
            _closing = true;
            CloseAllSockets();
            Thread.Sleep(2000);
        }
        private static void SetupServer()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("Server startad, väntar på klienter...");
        }
        private static void CloseAllSockets()
        {
            foreach (Socket socket in ClientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            _serverSocket.Close();
        }
        private static void AcceptCallback(IAsyncResult ar)
        {
            if (_closing)
                return;
            Socket socket = _serverSocket.EndAccept(ar);
            Console.WriteLine("Klient ansluten...");

            //socket.Send(Encoding.UTF8.GetBytes("välj ett namn")); 
            socket.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, socket);

            ClientSockets.Add(socket);
     
            _serverSocket.BeginAccept(AcceptCallback, null);
        }
        private static void GetUserName(IAsyncResult ar) 
        {
           
            Socket socket = (Socket)ar.AsyncState;
            int received;
            try
            {
                received = socket.EndReceive(ar);
            }
            catch (SocketException)
            {
                Console.WriteLine("Klient frånkopplad...");
                socket.Close();
                return;
            }

            string userName = Encoding.UTF8.GetString(Buffer, 0, received);
            User u = new User(userName, 0, socket);
            Users.Add(u);
            ClientSockets.Add(socket);
            socket.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, socket);

        }
       


        private static void ReceiveCallback(IAsyncResult ar) 
        {
            if (_closing)
                return;
            Socket current = (Socket)ar.AsyncState;
            int received;
            try
            {
                received = current.EndReceive(ar);
            }
            catch (SocketException)
            {
                Console.WriteLine("Klient frånkopplad...");
                current.Close();
                ClientSockets.Remove(current);
                return;
            }
          
            string text = Encoding.UTF8.GetString(Buffer, 0, received);

            IPEndPoint crep = current.RemoteEndPoint as IPEndPoint;
            IPEndPoint clep = current.LocalEndPoint as IPEndPoint;

            Console.WriteLine("Text skickad från (remote klient): " + crep.Address);
            Console.WriteLine("Text skickad till (server lokal ip): " + clep.Address);

            string[] textsplit = text.Split(' ');
         
            
            if (textsplit[0] == "wea238g")
            {
                
                Users.Add(new User(textsplit[1], 0, current));
                

            }

            

            if (textsplit[0] == "erf77")
            {
                string namelist = "";
                for (int i = 0; i < Users.Count; i++)
                {
                    namelist += Users[i].name + " ";
                }
                current.Send(Encoding.UTF8.GetBytes("erf77 " + namelist));
            }

            if (Users.Count == 2)
            {
                Console.WriteLine("hey");
            }

            else
            {


                Console.WriteLine("Mottagen text: '" + text + "' -> skickas till övriga klienter");

                foreach (Socket s in ClientSockets)
                {
                    if (s != current)
                    {
                        s.Send(Encoding.UTF8.GetBytes(text));
                        //IPEndPoint rep = s.RemoteEndPoint as IPEndPoint;
                        //IPEndPoint lep = s.LocalEndPoint as IPEndPoint;

                        //Console.WriteLine("Remote: " + rep.Address);
                        //Console.WriteLine("Local: " + lep.Address);
                    }
                }

                switch (text.ToLower())
                {
                    case "disconnect":
                        current.Shutdown(SocketShutdown.Both);
                        current.Close();
                        ClientSockets.Remove(current);
                        Console.WriteLine("Klient frånkopplad...");
                        return;
                }

                string chattText = Encoding.UTF8.GetString(Buffer, 0, received);
                foreach (User u in Users)
                {
                    if (u.socket == current)
                    {
                        Console.WriteLine(u.GetName() + "// " + chattText);

                    }
                }


            }
                current.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, current);
      
        
     
        }
    
    
    }
}