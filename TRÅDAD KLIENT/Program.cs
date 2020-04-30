using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TRÅDAD_KLIENT;

namespace tbClientThreads
{
    class Program
    {
        private static readonly Socket ClientSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int Port = 65002;
       static List<User> Users = new list<User>();
        
        
        static void Main()
        {
            Console.Title = "Klient";
            ConnectToServer();
            RequestLoop();
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Users.Add(new User("simon", 0));
            Users.Add(new User("jacob", 0));
        }

        static void GetUser() 
        { 
         
       
        
        
        
        }
        private static void ConnectToServer()
        {
            int attempts = 0;
            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempt " + attempts);
                    ClientSocket.Connect(IPAddress.Parse("127.0.0.1"), Port);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            }
            Console.Clear();
            Console.WriteLine("Connected");
        }

        public static void ReadServer()
        {
            ReceiveResponse();
        }

        private static void RequestLoop()
        {
            Thread readServerThread = new Thread(new ThreadStart(ReadServer));

            readServerThread.Start();

            Console.WriteLine("Skriv disconnect för att koppla från servern");
            string requestSent = string.Empty;
            try
            {
                while (requestSent.ToLower() != "disconnect")
                {
                    requestSent = Console.ReadLine();
                    ClientSocket.Send(Encoding.UTF8.GetBytes(requestSent), SocketFlags.None);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error! - Lost server.");
                Console.ReadLine();
            }
        }
        private static void ReceiveResponse()
        {
            var buffer = new byte[2048];
            try
            {
                int received = ClientSocket.Receive(buffer, SocketFlags.None);
                if (received == 0)
                    return;
                string test2 = Encoding.UTF8.GetString(buffer, 0, received);

              
                
                
                
                
                
                ReceiveResponse();
            }
            catch (System.Net.Sockets.SocketException)
            {
                Console.WriteLine("Frånkopplar från server...");
                return;
            }

           
            




      
        }
   
    private static void checkIfRight() 
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            string test2 = Console.ReadLine();

            if (test2 == "hej")
            {
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, received));
                Console.WriteLine("du skrev hej");

                checkIfRight();



            }



        }
    
    
    }

}
