using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment1_Server
{
    class tute
    {
        public string Tutor;
        public string Time;
        public string Room;
        public Int16 Session;

        public tute()
        {
            Session = 0;
            Tutor = "";
            Time = "";
            Room = "";
        }

        public tute(Int16 Session, string Tutor, string Time, string Room)
        {
            this.Tutor = Tutor;
            this.Time = Time;
            this.Room = Room;
            this.Session = Session;
        }
    }
}
