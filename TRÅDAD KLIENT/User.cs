using System;
using System.Collections.Generic;
using System.Text;

namespace TRÅDAD_KLIENT
{
    class User
    {
       
        
        public string name;
        public int rating;
        
        public User()
        {
             name = "unkown";
             rating = 0;
            
        }

        public User(string name, int rating)
        {
            this.name = name;
            this.rating = rating;




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
