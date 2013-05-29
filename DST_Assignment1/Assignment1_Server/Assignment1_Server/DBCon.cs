using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Assignment1_Server
{
    public class DBCon
    {
        #region VARIABLES

        private OLTESDataSet OLSI_DS =
                     new OLTESDataSet();
        private
            OLTESDataSetTableAdapters.
            DST_UsersTableAdapter OLSI_UA = new
            OLTESDataSetTableAdapters.
            DST_UsersTableAdapter();
        private
          OLTESDataSetTableAdapters.
             Users_DetailsTableAdapter OLSI_DA = new
             OLTESDataSetTableAdapters.
             Users_DetailsTableAdapter();

        public DBCon()
        {
            OLSI_UA.Fill(OLSI_DS.DST_Users);
            OLSI_DA.Fill(OLSI_DS.Users_Details);
        }

        private bool findUserIndex(user record)
        {
            string ID;
            int C1;
            int C2 = 0;
            int NoUsers1;
            int NoUsers2;
            string TStr1;
            string Tstr2;
            bool found;
            try
            {
                ID = record.username;
                NoUsers1 = OLSI_DS.DST_Users.Count;
                found = false;
                C1 = 0;
                while (C1 < NoUsers1)
                {
                    TStr1 = OLSI_DS.DST_Users.
                        Rows[C1]["Username"].
                        ToString();
                    if (TStr1 == ID)
                    {
                        NoUsers2 = OLSI_DS.
                                  Users_Details.Count;
                        while (C2 < NoUsers2 &&
                                 !found)
                        {
                            Tstr2 = OLSI_DS.
                                Users_Details.Rows[C2]
                                ["Username"].
                                ToString();
                            if (Tstr2 == TStr1)
                            {
                                record.index1 =
                                             C1;
                                record.index2 =
                                             C2;
                                found = true;
                            }
                            else
                            {
                                C2++;
                            }
                        }
                        return found;
                    }
                    else
                    {
                        C1++;
                    }
                }
                return found;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "findUserIndex: " +
                    ex.ToString());
                return false;
            }
        }

        public bool getPWByID(user record)
        {
            int C2;
            string PW;
            try
            {
                if (findUserIndex(record))
                {
                    C2 = record.index2;
                    PW = OLSI_DS.Users_Details.
                        Rows[C2]["User_password"].
                        ToString();
                    record.userPW = PW;
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("getPWByID: "
                               + ex.ToString());
                return false;
            }
        }

        public bool changePW(user record)
        {
            string newPW;
            int C2;
            try
            {
                if (findUserIndex(record))
                {
                    C2 = record.index2;
                    newPW = record.userPW;
                    OLSI_DS.Users_Details.Rows[C2].
                                  BeginEdit();
                    OLSI_DS.Users_Details.Rows[C2]
                            ["User_password"] = newPW;
                    OLSI_DS.Users_Details.Rows[C2].
                                  EndEdit();
                    OLSI_DA.Update(
                               OLSI_DS.Users_Details);
                    return (true);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ChangePW: " +
                                  ex.ToString());
                return false;
            }
        }
        public bool addUser(user record)
        {
            OLTESDataSet.DST_UsersRow
                              newUserRow;
            OLTESDataSet.Users_DetailsRow
                              newDetailRow;
            try
            {
                newUserRow = OLSI_DS.DST_Users.
                               NewDST_UsersRow();
                newDetailRow = OLSI_DS.Users_Details.
                               NewUsers_DetailsRow();
                newUserRow["Username"] =
                                record.username;
                newUserRow["Surname"] =
                                record.surname;
                newUserRow["Initial"] =
                                record.initial;
                newUserRow["User_Class"] =
                               record.userClass;
                OLSI_DS.DST_Users.Rows.Add(
                                newUserRow);
                OLSI_UA.Update(OLSI_DS);
                newDetailRow["Username"] =
                                record.username;
                newDetailRow["User_password"] =
                                record.userPW;
                newDetailRow["Session"] = record.Session;
                OLSI_DS.Users_Details.Rows.Add(
                                newDetailRow);
                OLSI_DA.Update(OLSI_DS);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool deleteUser(user record)
        {
            int C1;
            int C2;
            try
            {
                if (findUserIndex(record))
                {
                    C1 = record.index1;
                    OLSI_DS.DST_Users.Rows[C1].
                                  BeginEdit();
                    OLSI_DS.DST_Users.Rows[C1].
                                  Delete();
                    OLSI_DS.DST_Users.Rows[C1].
                                  EndEdit();
                    OLSI_UA.Update(
                               OLSI_DS.DST_Users);
                    C2 = record.index2;
                    OLSI_DS.Users_Details.Rows[C2].
                                  BeginEdit();
                    OLSI_DS.Users_Details.Rows[C2].
                                  Delete();
                    OLSI_DS.Users_Details.Rows[C2].
                                  EndEdit();
                    OLSI_DA.Update(
                             OLSI_DS.Users_Details);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                          "DeleteUser: " +
                          ex.ToString());
                return false;
            }
        }
        #region deleteUserSQL
        public bool deleteUserSQL(user record)
        {
            int C1;
            string thisName;
            string connectionString =
              "Provider=Microsoft.Jet.OLEDB.4.0;DataSource=|DataDirectory|\\OSLI_Database.mdb";
            System.Data.OleDb.OleDbCommand
                cmd = new System.Data.OleDb.
                          OleDbCommand();
            System.Data.OleDb.OleDbCommand
                cmd2 = new System.Data.OleDb.
                          OleDbCommand();
            System.Data.OleDb.OleDbConnection
                connection =
                       new System.Data.OleDb.
                           OleDbConnection(
                           connectionString);
            try
            {
                if (findUserIndex(record))
                {
                    C1 = record.index1;
                    thisName = (string)(OLSI_DS.
                                 DST_Users.Rows[C1]
                                 ["Username"]);
                    cmd.CommandType =
                              System.Data.
                              CommandType.Text;
                    cmd.Connection = connection;
                    connection.Open();
                    cmd.CommandText =
                        "DELETE FROM DST_List.DST_Users WHERE Username = \'" +
                         thisName + "\'";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    connection.Open();
                    cmd.CommandText =
                        "DELETE FROM DST_List.Users_Details WHERE Username = \'" +
                        thisName + "\'";
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    OLSI_UA.Fill(OLSI_DS.DST_Users);
                    OLSI_DA.Fill(
                                OLSI_DS.Users_Details);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "DeleteUserSQL: " +
                     ex.ToString());
                return false;
            }
        }
        #endregion

        #region authenticateUsers
        public bool authenticateUser(
                                user record)
        {

            int C2;
            string password;
            bool ret;

            if (findUserIndex(record))
            {
                C2 = record.index2;
                password = OLSI_DS.Users_Details.
                    Rows[C2]["User_password"].
                    ToString();
                if (record.userPW == password)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            else
            {
                ret = false;
            }
            return ret;
        }
        #endregion

        #region getUserRecords
        public string getUserRecord(user record)
        {
            int C1;
            int C2;
            string recordMSG = "";
            int count;
            int colNumber1;
            int colNumber2;

            colNumber1 = OLSI_DS.DST_Users.
                           Columns.Count;
            colNumber2 = OLSI_DS.Users_Details.
                           Columns.Count;
            findUserIndex(record);
            C1 = record.index1;
            C2 = record.index2;

            //Retrieve from User Table 
            for (count = 0;
                 count <= colNumber1 - 1;
                 count++)
            {
                recordMSG = recordMSG + "&" +
                    OLSI_DS.DST_Users.Rows[C1]
                    [count].ToString();
            }

            //Retrieve from Details Table 
            for (count = 0;
                 count <= colNumber2 - 1;
                 count++)
            {
                recordMSG = recordMSG + "&" +
                    OLSI_DS.Users_Details.Rows[C2]
                    [count].ToString();
            }

            return recordMSG;
        }
        #endregion

        #region getStudendUsername
        public string getStudentUsername(
                                 user record)
        {
            string IdMSG = "";
            int count;
            int rowNumber1;

            rowNumber1 = OLSI_DS.DST_Users.
                            Rows.Count;

            //Retrieves ID list 
            for (count = 0;
                 count <= rowNumber1 - 1;
                 count++)
            {
                if (OLSI_DS.DST_Users.Rows[count]
                    ["User_Class"].ToString()
                     == "Student")
                {
                    IdMSG = IdMSG + "&" +
                        OLSI_DS.DST_Users.Rows
                        [count]["Username"].
                        ToString();
                }
            }

            return IdMSG;
        }
        #endregion
        #region modifyUserDetails
        public bool modifyDetails(user record)
        {
            int C1;
            int C2;

            if (findUserIndex(record))
            {
                C1 = record.index1;
                C2 = record.index2;

                OLSI_DS.DST_Users.Rows[C1].
                                BeginEdit();
                OLSI_DS.DST_Users.Rows[C1]
                        ["Surname"] =
                               record.surname;
                OLSI_DS.DST_Users.Rows[C1]
                        ["Initial"] =
                               record.initial;
                OLSI_DS.DST_Users.Rows[C1].
                                EndEdit();
                OLSI_UA.Update(OLSI_DS.DST_Users);

                OLSI_DS.Users_Details.Rows[C2].
                                BeginEdit();
                OLSI_DS.Users_Details.Rows[C2]
                        ["User_password"] =
                                record.userPW;
                OLSI_DS.Users_Details.Rows[C2]["Session"] =
                                record.Session;
                OLSI_DS.Users_Details.Rows[C2].
                                EndEdit();
                OLSI_DA.Update(OLSI_DS.Users_Details);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string changeSession(user record) //originally change pw
        {
            string session;
            //Console.WriteLine(Convert.ToString(record.Session));
            int C2;
            try
            {
                if (findUserIndex(record))
                {
                    C2 = record.index2;
                    session = Convert.ToString(record.Session);
                    OLSI_DS.Users_Details.Rows[C2].
                                  BeginEdit();
                    OLSI_DS.Users_Details.Rows[C2]
                            ["Session"] = session;
                    OLSI_DS.Users_Details.Rows[C2].
                                  EndEdit();
                    OLSI_DA.Update(
                               OLSI_DS.Users_Details);
                    return ("Tutorial Change successful");
                }
                else
                {
                    return "Tutorial change unsuccessful";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ChangePW: " +
                                  ex.ToString());
                return "Tutorial change unsuccessful";
            }
        }


        #endregion

    }

}

#endregion

