using System;
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
        private static readonly List<User> users = new List<User>();
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
            User u = new User(userName, 0, socket, "");
            users.Add(u);
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

            Console.WriteLine("Text " + text + " skickad från (remote klient): " + crep.Address);
            Console.WriteLine("Text skickad till (server lokal ip): " + clep.Address);


            string[] textsplit = text.Split(' ');

            if (textsplit[0] == "wea238g")
            {
                
                users.Add(new User(textsplit[1], 0, current, ""));

            }
            else if (textsplit[0] == "erf77")
            {
                string namelist = "";
                for (int i = 0; i < users.Count; i++)
                {
                    namelist += users[i].name + " " + users[i].rating + " ";
                }
                current.Send(Encoding.UTF8.GetBytes("erf77 " + namelist));
            }
            else if (textsplit[0] == "swt5")
            {
                foreach (User u in users)
                {
                    if (u.name == textsplit[1])
                    {
                        u.socket.Send(Encoding.UTF8.GetBytes("kl90 " + textsplit[2]));
                    }
                    
                }
            }
            else if (textsplit[0] == "acc47")
            {
                foreach (User u in users)
                {
                    
                    if (u.socket == current)
                    {
                        u.chattpartner = textsplit[1];
                        u.socket.Send(Encoding.UTF8.GetBytes("yp82 " + textsplit[1]));

                        for (int i = 0; i < users.Count; i++)
                        {
                            if (users[i].name == textsplit[1])
                            {
                                
                                users[i].chattpartner = u.name;
                                users[i].socket.Send(Encoding.UTF8.GetBytes("yp82 " + u.name));
                            }
                            
                        }
                    }
                }
                for (int i = 0; i < users.Count; i++)
                {
                    for (int j = 0; j < users.Count; j++)
                    {
                        if (users[i].chattpartner == users[j].name && users[j].chattpartner != users[i].name)
                        {
                            users[i].socket.Send(Encoding.UTF8.GetBytes("yp82 none"));
                            users[i].chattpartner = "";
                        }
                    }
                }

                
            }


            else
            {
                foreach (User u in users)
                {
                    if (u.socket == current)
                    {
                        for (int i = 0; i < users.Count; i++)
                        {
                            if(users[i].name == u.chattpartner)
                            {
                                users[i].socket.Send(Encoding.UTF8.GetBytes(text));
                                Console.WriteLine("Skickar: " + text + ") till: " + users[i].name + ")");
                            }
                        }
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
            }
                current.BeginReceive(Buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, current);
      
        
     
        }
    
    
    }
}