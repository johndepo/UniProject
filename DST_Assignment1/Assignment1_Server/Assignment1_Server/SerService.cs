using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Assignment1_Server
{
    class SerService
    {
        private string childName = "CSP";
        private const string EOB = "$$";
        private const int buffSize = 1024;
        private TcpClient serConnection;
        private NetworkStream stream;
        private byte[] outBuff;
        private byte[] inBuff = new byte[buffSize];


        string notAuthenticated = "User does not exist";
        string authenticateMessage = "YOU ARE AUTHENTICATE AS";


        /**
         * DATABASE Declarations
         * 
         **/

        //user record = new user();
        //string userName;
        //string userPW;

        public string ChildName
        {
            set
            {
                childName = value;
            }
        }

        public TcpClient SerConnection
        {
            set
            {
                serConnection = value;
            }


        }

        public void Service()
        {
            int nc = 0;
            bool cliStop = false;
            string MSG;
            string thankMSG = "Logged out";
            string clientName = childName;
            TcpClient clientConnection = serConnection;
            Console.Write("***Client {0,6} ", clientName);
            Console.WriteLine(" started ...");

            try
            {
                stream = clientConnection.GetStream();
                //MSG = childName + ": ";
                //MSG = MSG + "Echo service - " + "Send message or " + "just $$ to quit";
                //outBuff = Encoding.ASCII.GetBytes(MSG);
                //stream.Write(outBuff, 0, MSG.Length);
                nc = stream.Read(inBuff, 0, buffSize);

                while ((nc > 0) && (!cliStop))
                {
                    MSG = Encoding.ASCII.GetString(inBuff, 0, nc);

                    /**
                     *  STRING SEPERATORS
                     **/


                    string[] inMessage = MSG.Split(';');

                    
                 

                    if (MSG != EOB)
                    {
                        Console.WriteLine("Message From =>", clientName);
                        Console.WriteLine("            " + MSG);

                        string placeHolder = functionSwitch(MSG);
                        //getTutorials();

                        outBuff = Encoding.ASCII.GetBytes(placeHolder);
                        stream.Write(outBuff, 0, placeHolder.Length);

         

                        nc = stream.Read(inBuff, 0, buffSize);


                    }
                    else
                    {
                        //outBuff = Encoding.ASCII.GetBytes(thankMSG);

                        //stream.Write(outBuff, 0, thankMSG.Length);
                        //stream.Flush();

                        //Console.Write("***Client wants");
                        //Console.WriteLine(" to stop");
                        
                        //cliStop = true;

                        Console.Write("***Client wants");
                        Console.WriteLine(" to stop");
                        cliStop = true;
                        outBuff = Encoding.ASCII.GetBytes(thankMSG);
                        stream.Write(outBuff, 0, thankMSG.Length);//break point
                        stream.Flush();
                    }
                }
                stream.Close();
                clientConnection.Close();
                Console.Write("***Client {0,6} ", clientName);
                Console.WriteLine("stopped!");
            }
            catch (Exception ex)
            {
                Console.Write("Err: Client {0,6} left ", clientName);
                Console.WriteLine(" or " + ex.Message);
            }
        }

        public string functionSwitch(string message)
        {
            string tutorial;
            string function;
            string id;
            string password;
            string[] inMessage = message.Split(';');


            function = inMessage[0];
            


            switch (function)
            {
                case "authenticate":
                    //Console.WriteLine("ATTEMPTING TO AUTHENTICATE SWITCH IS WORKING");
                    //Console.WriteLine("SendingMEssageBacktoClient");

                    //functionMessage = validate(id, password);

                    //outBuff = Encoding.ASCII.GetBytes(functionMessage);

                    //stream.Write(outBuff, 0, functionMessage.Length);
                    //stream.Flush();
                    id = inMessage[1];
                    password = inMessage[2];

                    DBCon authenticate_DB = new DBCon();
                    user u = new user();

                    u.username = id;
                    u.userPW = password;
                    
                    

                    if (authenticate_DB.authenticateUser(u) == true)
                    {
                        return "User has been authenticated.";
                    }
                    else
                    {
                        return "Username or password is incorrect.";
                    }

                case "getAllTutorials":
                    string allTutes = "tutorialList." + getTutorials();
                    return allTutes;
                    //Console.WriteLine(getTutes.getStudentUsername(t));

                case "getStudentSession":
                    id = inMessage[1];
                    password = inMessage[2];
                    string sessionNumber = "studentSessionRequest." + getSessionNumber(id);
                    return sessionNumber;
                case "changeTutorial":
                    string confirmation;
                    id = inMessage[1];
                    tutorial = inMessage[2];
                    DBCon changeTut = new DBCon();
                    user changeTutorialStudent = new user();

                    changeTutorialStudent.username = id;
                    changeTutorialStudent.Session = int.Parse(tutorial);
                    //changeTutorialStudent.userPW = "LOL";
                    confirmation = changeTut.changeSession(changeTutorialStudent);
                    confirmation += ".";
                    

                    //DatabaseHandler dt = new DatabaseHandler();
                    //dt.updateSession(changeTutorialStudent);
                    //Console.WriteLine("MAYBE");
                    return confirmation;
                case "requestStudentsInTutorial":
                    string list = "TutorialStudents.";
                    id = inMessage[1];
                    tutorial = inMessage[2];

                    list += requestStudentsInTutorial(tutorial);

                    return list;
                case "removeStudent":
                    string confirm = "Student removed from tutorial";
                    Console.WriteLine(inMessage);
                    id = inMessage[1];
                    tutorial = inMessage[2];
                    DBCon dbc = new DBCon();
                    user removedStudent = new user();

                    removedStudent.username = id;
                    removedStudent.Session = 0;

                    confirmation = dbc.changeSession(removedStudent);
                    confirmation += ".";

                    return confirm;
                case "findStudent":
                    string studentFound = "foundStudent.";
                    studentFound += getUsers_Details(inMessage[1]);
                    return studentFound;
                case "requestStudentsName":
                    string studentName = "foundStudentName.";
                    string userName = inMessage[1];
                    studentName += getName(userName);

                        return studentName;
                default:
                    return "HUH";
            }

        }

        //public string validate(string uName, string password)
        //{
        //    string valMessage = "User Authenticated";
        //    string valDecline = "User does not exist";

        //    DBCon myDB = new DBCon();
        //    user record = new user();

        //    record.username = uName;
        //    record.userPW = password;

        //    if (myDB.authenticateUser(record) == true)
        //    {
        //        Console.WriteLine("Congrats it works lol");
        //        return valMessage;
        //    }
        //    else
        //    {
        //        Console.WriteLine("NOPE DOESNT WORK BRAH");
        //        return valDecline;
        //    }

        //    #region badcode
        //    //string userName;
        //    //string userPW;
        //    //string ans;
        //    //bool stop = false;
        //    //int task;
        //    //bool found;
        //    //user record = new user();


        //    //while (!stop)
        //    //{
        //    //    //Console.Write("Enter userName: ");
        //    //    //userName = Console.ReadLine();
        //    //    record.username = userName;
        //    //    record.userPW = password;
        //    //    if (myDB.getPWByID(record))
        //    //    //if (password == record.userPW)
        //    //    {
        //    //        password = record.userPW;
        //    //        Console.WriteLine("Password of "
        //    //          + userName + " is " + password);
        //    //        found = true;

        //    //        stop = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        Console.WriteLine("Password is incorrect");
        //    //        stop = true;
        //    //    }
        //    //    //else
        //    //    //{
        //    //    //    Console.WriteLine("hehe" + record.userPW);
        //    //    //    stop = true;
        //    //    //}


        //    //    //else
        //    //    //{
        //    //    //    Console.WriteLine("User " +
        //    //    //        userName + " not found!");
        //    //    //    found = false;
        //    //    //}
        //    //    //if (found)
        //    //    //{
        //    //    //    Console.Write(
        //    //    //        "Select task 0 (exit), 1 (modify), 2 (delete) 3 (next): ");
        //    //    //    ans = Console.ReadLine();
        //    //    //    task = Convert.ToInt16(ans);
        //    //    //    switch (task)
        //    //    //    {
        //    //    //        case 0: stop = true;
        //    //    //            break;
        //    //    //        case 1:
        //    //    //            Console.Write(
        //    //    //                "New password: ");
        //    //    //            record.userPW =
        //    //    //                Console.ReadLine();
        //    //    //            if (myDB.
        //    //    //                  changePW(record))
        //    //    //            {
        //    //    //                Console.WriteLine(
        //    //    //                    "Record changed successfully");
        //    //    //            }
        //    //    //            else
        //    //    //            {
        //    //    //                Console.WriteLine(
        //    //    //                    "Record changed unsuccessfully");
        //    //    //            }
        //    //    //            userPW = record.userPW;
        //    //    //            Console.WriteLine(
        //    //    //              "Password of user " +
        //    //    //                userName + " is " +
        //    //    //                userPW);
        //    //    //            break;
        //    //    //        case 2:
        //    //    //            Console.Write(
        //    //    //             "Delete user? (Y/N) ");
        //    //    //            ans = Console.
        //    //    //                        ReadLine();
        //    //    //            if (ans == "Y" ||
        //    //    //                ans == "y")
        //    //    //            {
        //    //    //                if (myDB.deleteUser(
        //    //    //                  record))
        //    //    //                {
        //    //    //                    Console.WriteLine(
        //    //    //                     "Record deleted successfully");
        //    //    //                }
        //    //    //                else
        //    //    //                {
        //    //    //                    Console.WriteLine(
        //    //    //                      "Record deleted unsuccessfully");
        //    //    //                }
        //    //    //            }
        //    //    //            break;
        //    //    //        default: continue;
        //    //    //    }
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    Console.Write(
        //    //    //        "add user? (Y/N) ");
        //    //    //    ans = Console.ReadLine();
        //    //    //    if (ans == "Y" || ans == "y")
        //    //    //    {
        //    //    //        record.surname = userName;
        //    //    //        record.userPW = "IamNew_" +
        //    //    //                        userName;
        //    //    //        if (myDB.addUser(record))
        //    //    //        {
        //    //    //            Console.WriteLine(
        //    //    //                "Record added successfully");
        //    //    //            userPW = record.userPW;
        //    //    //            Console.WriteLine(
        //    //    //                "Password of user " +
        //    //    //                userName + " is " +
        //    //    //                userPW);

        //    //    //        }
        //    //    //        else
        //    //    //        {
        //    //    //            Console.WriteLine(
        //    //    //                "Record added unsuccessfully");
        //    //    //        }
        //    //    //    }
        //    //    //}

        //    //}
        //    #endregion




        //}

        //public static List<tute> getTutorials() //List<tute>
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    string[][] tuteArray = dh.getAllTutorials();
        //    List<tute> tuteList = new List<tute>();

        //    foreach (string[] ar in tuteArray)
        //    {
        //        tuteList.Add(new tute(Int16.Parse(ar[0]), ar[1], ar[2], ar[3]));
        //    }

        //    string tList = "";

        //    for (int i = 0; i < tuteList.Count; i++)
        //    {
        //        //string[] tList = { Convert.ToString(tuteList[i].Session), tuteList[i].Tutor, tuteList[i].Time, tuteList[i].Room};

                
        //        tList += Convert.ToString(tuteList[i].Session) + ";" + tuteList[i].Tutor + ";" + tuteList[i].Time +";" + tuteList[i].Room + "!!!";

        //        Console.WriteLine(tList);

        //    }

            

        //    return tuteList;
        //}

        public string requestStudentsInTutorial(string tutorial)
        {

            DatabaseHandler dh = new DatabaseHandler();
            string[][] reqArray = dh.getStudentSession();
            List<user> reqList = new List<user>();

            string requestedStudents = "";

            foreach (string[] ar in reqArray)
            {
                reqList.Add(new user(ar[0], ar[1], Int16.Parse(ar[2])));

                if (tutorial == ar[2])
                {
                    //requestedStudents += ar[0] + ";" + Convert.ToString(ar[2]) + ";";
                    requestedStudents += getPersonDetails(ar[0]);
                }
            }
            return requestedStudents;

            
        }

        public string getTutorials() //List<tute>
        {
            DatabaseHandler dh = new DatabaseHandler();
            string[][] tuteArray = dh.getAllTutorials();
            List<tute> tuteList = new List<tute>();

            foreach (string[] ar in tuteArray)
            {
                tuteList.Add(new tute(Int16.Parse(ar[0]), ar[1], ar[2], ar[3]));
            }

            string tList = "";

            for (int i = 0; i < tuteList.Count; i++)
            {
                //string[] tList = { Convert.ToString(tuteList[i].Session), tuteList[i].Tutor, tuteList[i].Time, tuteList[i].Room};


                tList += Convert.ToString(tuteList[i].Session) + ";" + tuteList[i].Tutor + ";" + tuteList[i].Time + ";" + tuteList[i].Room + "!";
                
                //Console.WriteLine(tList);

            }



            return tList;
        }

        public string getName(string user_name)
        {
            DatabaseHandler dh = new DatabaseHandler();
            string[][] personArray = dh.getDST_Users();
            List<user> personList = new List<user>();

            string person = "";

            foreach (string[] ar in personArray)
            {
                personList.Add(new user(ar[0], ar[1], ar[2], ar[3]));

                if (user_name == ar[0])
                {
                    person = ar[1];
                }
            }
            return person;
        }

        public string getPersonDetails(string user_name)
        {
            DatabaseHandler dh = new DatabaseHandler();
            string[][] personArray = dh.getDST_Users();
            List<user> personList = new List<user>();

            string person = "";

            foreach (string[] ar in personArray)
            {
                personList.Add(new user(ar[0], ar[1], ar[2], ar[3]));

                if (user_name == ar[0])
                {
                    person = ar[0] +";"+ ar[1] +";"+ ar[2] +";"+ ar[3] + "!";
                }
            }
            return person;
        }

        public string getSessionNumber(string user_name)
        {
            DatabaseHandler dh = new DatabaseHandler();
            string[][] sessionArray = dh.getStudentSession();
            List<user> sessionList = new List<user>();

            string sessionNo = "";

            foreach (string[] ar in sessionArray)
            {
                sessionList.Add(new user(ar[0], ar[1], Int16.Parse(ar[2])));

                if (user_name == ar[0])
                {
                    sessionNo = Convert.ToString(ar[2]);
                }
            }
            return sessionNo;
            
        }

        public string getUsers_Details(string user_name)
        {
            DatabaseHandler dh = new DatabaseHandler();
            string[][] sessionArray = dh.getStudentSession();
            List<user> sessionList = new List<user>();

            string sessionNo = "";

            foreach (string[] ar in sessionArray)
            {
                sessionList.Add(new user(ar[0], ar[1], Int16.Parse(ar[2])));

                if (user_name == ar[0])
                {
                    sessionNo = ar[0] + ";" + ar[2] + ";";
                }
            }
            return sessionNo;
        }

        public string getSess() //List<tute>
        {
            DatabaseHandler dh = new DatabaseHandler();
            string[][] tuteArray = dh.getAllTutorials();
            List<tute> tuteList = new List<tute>();

            foreach (string[] ar in tuteArray)
            {
                tuteList.Add(new tute(Int16.Parse(ar[0]), ar[1], ar[2], ar[3]));
            }

            string tList = "";

            for (int i = 0; i < tuteList.Count; i++)
            {
                //string[] tList = { Convert.ToString(tuteList[i].Session), tuteList[i].Tutor, tuteList[i].Time, tuteList[i].Room};


                tList += Convert.ToString(tuteList[i].Session) + ";" + tuteList[i].Tutor + ";" + tuteList[i].Time + ";" + tuteList[i].Room + "!";

                //Console.WriteLine(tList);

            }



            return tList;
        }

    }
}

#region MAYNEED LATER

//string function;
                        //string id;
                        //string password;

                        //function = inMessage[0];
                        //id = inMessage[1];
                        //password = inMessage[2];


                        //switch (function)
                        //{
                        //    case "authenticate":

                        //        DBCon myDB = new DBCon();
                        //        user record = new user();

                        //        record.username = id;
                        //        record.userPW = password;

                        //        if (myDB.authenticateUser(record) == true)
                        //        {
                        //            Console.WriteLine("Congrats it works lol");
                        //            outBuff = Encoding.ASCII.GetBytes(authenticateMessage);

                        //            stream.Write(outBuff, 0, authenticateMessage.Length);
                        //            stream.Flush();
                        //        }
                        //        else
                        //        {
                        //            Console.WriteLine("NOPE DOESNT WORK BRAH");
                        //            outBuff = Encoding.ASCII.GetBytes(notAuthenticated);

                        //            stream.Write(outBuff, 0, notAuthenticated.Length);
                        //            stream.Flush();
                        //        }
                        //        break;
                        //    //default:
                        //    //    Console.WriteLine("UH OH");
                        //}
#endregion