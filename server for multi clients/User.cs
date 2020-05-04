using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace tbServerMutlipleClients
{
    class User
    {


        public string name;
        public int rating;
        public Socket socket;
        public User()
        {
             name = "unkown";
             rating = 0;
             socket = null;
        }

        public User(string name, int rating, Socket socket)
        {
            this.name = name;
            this.rating = rating;
            this.socket = socket;



        }

        internal void Add()
        {
            throw new NotImplementedException();


        }
        public string GetName()
        {


            return name;


        }

    }
}
