using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chattprogram
{
   public class Användare
    {
        public string name;
        public int rating;
        public int socket;
        public Användare() 
        {
            string Name = "unkown";
            int Rating = 0;
            int Socket = 0;
        }

        public Användare(string name, int rating, int socket) 
        {
            this.name = name;
            this.rating = rating;
            this.socket = socket; 
        
        }






    }
}
