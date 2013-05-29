using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment1_Server
{
    public class user
    {
        public string username, surname,
                initial, userClass;

        public string userPW;
        public int index1, index2;
        
        public int Session;

        public user()
        {
            Session = 0;
            userPW = "dst";
        }

        public user(string username, string userPW, int Session)
        {
            this.username = username;
            this.userPW = userPW;
            this.Session = Session; 

        }

        public user(string username, string userPW, string initial, string userClass)
        {
            this.username = username;
            this.userPW = userPW;
            this.initial = initial;
            this.userClass = userClass;
        }
    }
}
