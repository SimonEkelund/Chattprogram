﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chattprogram
{
   public class Användare
    {
        public string name;

        public float rating;
        public Socket socket;
        public Användare()
        {
            string name = "unkown";
            float rating = 0;
            Socket socket = null;

        public int rating;
        public Socket socket;
        public Användare() 
        {
             name = "unkown";
             rating = 0;
             socket = 0;

        }
        
        public Användare(string name, float rating, Socket socket)
        {
            this.name = name;
            this.rating = rating;

            this.socket = socket;

           
        

        }






    }
}
