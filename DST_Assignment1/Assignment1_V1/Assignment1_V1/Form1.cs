using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Assignment1_V1
{
    public partial class Form1 : Form
    {
        TcpClient connection;
        Int16 serPort;
        string serName;
        IPAddress serIP;

        public const string Footer = "$$";
        public NetworkStream stream;
        public byte[] outBuff;
        public string outMSG;

        public const int IOBSize = 1024;
        public byte[] inBuff = new byte[IOBSize];
        public string inMSG;

        

        public Form1()
        {
            InitializeComponent();
            
        }

        public void cmd_Login_Click(object sender, EventArgs e)
        {
            //string staff_IDRegex = @"[su]\d{6}$"; //add if statement for 7char //old way ^s\\d{6}
            //string student_IDRegex = @"[su]\d{6}$";
            //string passwordRegex = @"[!@#$%^&*()_+=\[{\]};:<>|./?,\\'""-0123456789]";


            ////check for staff / tutor
            //Match staffMatch = Regex.Match(txt_ID.Text, staff_IDRegex);
            //if (staffMatch.Success)
            //{
            //    MessageBox.Show("Hi staff");
            //}

            ////check for student
            //Match studentMatch = Regex.Match(txt_ID.Text, student_IDRegex);
            //if (studentMatch.Success)
            //{
            //    MessageBox.Show("Hi Student");
            //}

            //Match passwordMatch = Regex.Match(txt_Password.Text, passwordRegex);
            //if(passwordMatch.Success)
            //{
            //    MessageBox.Show("NICEPASS");
            //}

            /**
             *  Part A Question 2
             *  Checks the formats of the username. If the check is not satisfactory give an appropriate warning
             *  If check is correct attempt a connection to server
             **/

            string inputCheck = checkFormat();

            switch (inputCheck)
            {
                case "staff":
                    connectToServer(inputCheck);
                    break;
                case "student":
                    connectToServer(inputCheck);
                    break;
                case "false":
                    MessageBox.Show("The format for username must begin with 'u' or 's' \n and password must contain 1 non alphabetic character");
                    break;
            }


            //connectToServer();
            

            //try
            //{
            //    serPort = 1234;
            //    //serPort = Convert.ToInt16(1234); //server Port
            //    serName = Dns.GetHostName();
            //    serIP = Dns.GetHostAddresses(serName)[0];

            //    connection = new TcpClient(serName, serPort);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Cannot Connect to Server, Please Retry");
            //}

            
        }

        public string checkFormat()
        {

            string IDRegex = @"[su]\d{6}$";
            string staff_IDRegex = @"[s]\d{6}$"; //add if statement for 7char /old way ^s\\d{6}
            string student_IDRegex = @"[u]\d{6}$";
            string passwordRegex = @"[!@#$%^&*()_+=\[{\]};:<>|./?,\\'""-0123456789]";

            if ((txt_ID.Text == "") || (txt_Password.Text == ""))
            {
                MessageBox.Show("Please fill all fields");
                return "false";
            }
            else
            {
                ////check for staff / tutor
                //Match staffMatch = Regex.Match(txt_ID.Text, staff_IDRegex);              
                ////check for student
                //Match studentMatch = Regex.Match(txt_ID.Text, student_IDRegex);               

                //check for student or staff
                Match idMatch = Regex.Match(txt_ID.Text, IDRegex);
                Match staffMatch = Regex.Match(txt_ID.Text, staff_IDRegex);
                Match studentMatch = Regex.Match(txt_ID.Text, student_IDRegex);
                //check password
                Match passwordMatch = Regex.Match(txt_Password.Text, passwordRegex);

                if (staffMatch.Success && passwordMatch.Success)
                {
                    return "staff";
                }
                else if(studentMatch.Success && passwordMatch.Success)
                {
                    return "student";
                }
                else
                {
                    return "false";
                }
            }
        }

        public void connectToServer(string loginType)
        {
            try
            {
                

                serPort = 1234;
                serName = Dns.GetHostName();
                serIP = Dns.GetHostAddresses(serName)[0];

                connection = new TcpClient(serName, serPort);

                stream = connection.GetStream();

                sendLogin();
                initializeReader();


                if (loginType == "staff")
                {
                    txt_Search.Visible = true;
                    cmd_Search.Visible = true;
                    lst_Students.Visible = true;
                    cmd_Remove.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection error, Please try again");
            }
                         
        }

        public void requestTutorials()
        {
            int j;

            try
            {
                outMSG = ("getTutorials;" + txt_ID.Text + ";" + txt_Password.Text);
                j = outMSG.IndexOf(Footer);

                //if (i >= 0)
                //{
                //    outMSG = outMSG.Substring(0, i);
                //}

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Message error");
            }
        }

        public void initializeReader()
        {
            Thread reader = new Thread(new ThreadStart(GetMessage));

            reader.IsBackground = true;
            reader.Start();
        }

        public void sendLogin()
        {
            int i;

            try
            {
                outMSG = ("authenticate;" + txt_ID.Text + ";" + txt_Password.Text);
                i = outMSG.IndexOf(Footer);

                
                //outMSG = outMSG.Substring(0, i);
                

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Message error");
            }
                
        }

        public void getStudentSessionNumber()
        {
            int i;

            try
            {
                outMSG = ("getStudentSession;" + txt_ID.Text + ";" + txt_Password.Text);
                i = outMSG.IndexOf(Footer);

                //if (i >= 0)
                //{
                //    outMSG = outMSG.Substring(0, i);
                //}

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("There are currently no connections");
            }
        }


        public void getAllTutorials()
        {
            int i;

            try
            {
                outMSG = ("getAllTutorials;" + txt_ID.Text + ";" + txt_Password.Text);
                i = outMSG.IndexOf(Footer);

                //if (i >= 0)
                //{
                //    outMSG = outMSG.Substring(0, i);
                //}

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("There are currently no connections");
            }

        }

        private void cmd_Logout_Click(object sender, EventArgs e)
        {


            try
            {
                txt_ID.ResetText();
                txt_Password.ResetText();
                txt_Search.Visible = false;
                cmd_Search.Visible = false;
                lst_Students.Visible = false;
                cmd_Remove.Visible = false;
                //outMSG = ("$$");
                //outBuff = Encoding.ASCII.GetBytes(outMSG);
                //stream.Write(outBuff, 0, outMSG.Length);
                //MessageBox.Show("Logged out");
                MessageBox.Show("Logged out");
                sendLogoutMessage();
                connection.Close();

            }
            catch
            {
                MessageBox.Show("No connection to server");
            }
            //connection.Close();
            //cmd_Logout.Visible = false;
            //cmd_Login.Visible = true;
            //MessageBox.Show("Logged out of server");
        }

        public void sendLogoutMessage()
        {
            int k;

            try
            {
                outMSG = ("$$");
                k = outMSG.IndexOf(Footer);

                //if (i >= 0)
                //{
                //    outMSG = outMSG.Substring(0, i);
                //}

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Message error");
            }
        }

        public void GetMessage()
        //This is to run in background
        //by new thread
        {
            int nc = 0;

            try
            {
                while (!connection.Connected) { }
                //state = 3;
                stream = connection.GetStream();
                //this.Invoke(new MethodInvoker(DisplayState));
                while (connection.Connected)
                {
                    if (connection.Available > 0)
                    {
                        nc = stream.Read(inBuff, 0, IOBSize); //break point
                        inMSG = Encoding.ASCII.
                            GetString(inBuff,
                                        0, nc);

                        string[] command = inMSG.Split('.');
                        string data = command[1];
                        

                        switch (command[0])
                        {
                            case "User has been authenticated":
                                MessageBox.Show(inMSG);
                                break;
                            case "Username or password is incorrect":
                                MessageBox.Show(inMSG);                               
                                break;
                            case "Thank You!":
                                MessageBox.Show("Logged out");
                                break;
                            case "tutorialList":

                                lst_TutorialUpdate method = new lst_TutorialUpdate(showTutorials);
                                lst_Tutorials.Invoke(method, new object[] {data});

                                break;
                            case "studentSessionRequest":
                                lbl_SessionUpdate m = new lbl_SessionUpdate(showSession);
                                lbl_Session.Invoke(m, new object[] { data });
                                break;
                            case "Tutorial Change successful":
                                MessageBox.Show(inMSG);
                                break;
                            case "Tutorial change unsuccessful":
                                MessageBox.Show(inMSG);
                                break;
                            case "TutorialStudents":
                                //MessageBox.Show(inMSG);
                                lst_StudentUpdate s = new lst_StudentUpdate(showStudent);
                                lst_Students.Invoke(s, new object[] { data });
                                break;
                            case "foundStudent":
                                lst_StudentUpdate g = new lst_StudentUpdate(showStudent);
                                lst_Students.Invoke(g, new object[] { data });
                                break;
                            case "foundStudentName":
                                lbl_HeaderUpdate h = new lbl_HeaderUpdate(showName);
                                lbl_Header.Invoke(h, new object[] { data });
                                break;
                        }
                    }
                    //break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: getMessage   " +
                    ex.Message);
            }
            
        }
        #region delegates gui updates

        delegate void lbl_HeaderUpdate(string n);
        public void showName(string n)
        {
            lbl_Header.Text = "Hi " + n;
        }
            

        delegate void lst_StudentUpdate(string students);
        public void showStudent(string students)
        {
            lst_Students.Items.Clear();
            string[] student = students.Split('!');
            lst_Students.Items.AddRange(student);
        }

        delegate void lbl_SessionUpdate(string session);

        public void showSession(string session)
        {
            lbl_Session.Text = "Current Tutorial = " + session + " tutorial";
        }

        
        delegate void lst_TutorialUpdate(string tutorials);

        public void showTutorials(string tutorials)
        {
            lst_Tutorials.Items.Clear();

            string[] tutorial = tutorials.Split('!');

            lst_Tutorials.Items.AddRange(tutorial);

        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            txt_Search.Visible = false;
            cmd_Search.Visible = false;
            lst_Students.Visible = false;
            cmd_Remove.Visible = false;
        }

        private void cmd_changeTutorial_Click_1(object sender, EventArgs e)
        {
            int k;

            try
            {
                outMSG = ("changeTutorial;" + txt_ID.Text + ";" + txt_TuteSession.Text);
                k = outMSG.IndexOf(Footer);

                //if (i >= 0)
                //{
                //    outMSG = outMSG.Substring(0, i);
                //}

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Message error");
            }
        }

        private void cmd_UpdateTutorial_Click(object sender, EventArgs e)
        {
            try
            {
                getAllTutorials();
            }
            catch
            {
                MessageBox.Show("No connection to server");
            }
        }

        private void lst_Tutorials_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selection = lst_Tutorials.SelectedIndex;
            //int k;

            txt_TuteSession.Text = Convert.ToString(selection + 1);

            string inputCheck = checkFormat();

            if (inputCheck == "staff")
            {
                requestStudentsInTutorial(Convert.ToString(lst_Tutorials.SelectedIndex + 1));
            }

        }

        public void requestStudentsInTutorial(string tutorialNumber)
        {
            int i;
            try
            {
                outMSG = ("requestStudentsInTutorial;" + txt_ID.Text + ";" + tutorialNumber);
                i = outMSG.IndexOf(Footer);

                //if (i >= 0)
                //{
                //    outMSG = outMSG.Substring(0, i);
                //}

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("There are currently no connections");
            }
        }


        private void lbl_Session_Click(object sender, EventArgs e)
        {
            getStudentSessionNumber();
            getStudentName(txt_ID.Text);
        }

        public void getStudentName(string username)
        {
            int i;
            try
            {
                outMSG = ("requestStudentsName;" + username);
                i = outMSG.IndexOf(Footer);

                //if (i >= 0)
                //{
                //    outMSG = outMSG.Substring(0, i);
                //}

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("There are currently no connections");
            }
        }

        private void lst_Students_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmd_Remove_Click(object sender, EventArgs e)
        {

        }

        private void cmd_Remove_Click_1(object sender, EventArgs e)
        {
        }


        public void removeStudent(string student)
        {
            int i;

            try
            {
                outMSG = ("removeStudent;" + student);
                i = outMSG.IndexOf(Footer);

                //if (i >= 0)
                //{
                //    outMSG = outMSG.Substring(0, i);
                //}

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("There are currently no connections");
            }

        }

        private void cmd_Search_Click(object sender, EventArgs e)
        {
            string searchName = txt_Search.Text;
            searchStudent(searchName);
        }

        public void searchStudent(string name)
        {
            int i;
            
            try
            {
                outMSG = ("findStudent;" + name);
                i = outMSG.IndexOf(Footer);

                //if (i >= 0)
                //{
                //    outMSG = outMSG.Substring(0, i);
                //}

                outBuff = Encoding.ASCII.GetBytes(outMSG);
                stream.Write(outBuff, 0, outMSG.Length);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("There are currently no connections");
            }
        }

    }
}
