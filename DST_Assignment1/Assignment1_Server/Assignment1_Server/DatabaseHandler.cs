using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace Assignment1_Server
{


    class DatabaseHandler
    {
        #region Setup
        private OleDbConnection connection;

        public DatabaseHandler()
        {
            connection = createConnection();
        }

        private OleDbConnection createConnection()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=OLTES.mdb;Persist Security Info=True";
            //string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\\..\\..\\OLTES.mdb;Persist Security Info=True";
            return new OleDbConnection(connectionString);
        }

        private void commandQuery(string sql)
        {
            connection.Open();
            OleDbCommand newCommand = new OleDbCommand(sql, connection);
            
            newCommand.ExecuteNonQuery();
            connection.Close();
        }

        private string[][] selectQuery(string sql)
        {
            OleDbCommand command;
            OleDbDataReader reader;

            List<string[]> result = new List<string[]>();

            connection.Open();
            command = new OleDbCommand(sql, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                string[] row = new string[reader.FieldCount];

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader[i].ToString();


                }
                result.Add(row);
            }
            reader.Close();
            connection.Close();

            return result.ToArray();
        }
        #endregion

        #region Methods

        public string[][] getStudentSession()
        {
            return selectQuery("SELECT * FROM Users_Details");
        }

        public string[][] getDST_Users()
        {
            return selectQuery("SELECT * FROM DST_Users");
        }

        public string[][] getAllTutorials()
        {
            return selectQuery("SELECT * FROM Tute");
        }

        public string[] getStudentSession(string username)
        {
            string[][] result = selectQuery("SELECT Session FROM Users_Details WHERE Username = '" + username + "'");

            try
            {
                return result[0];
            }
            catch
            {
                throw new ArgumentException();
            }
        }

        public void updateSession(user record)
        {
            commandQuery("UPDATE Users_Details SET Session = " + record.Session + " WHERE Username = '" + record.username + "'");
        }

        //public string[][] getSessionStudents(int session)
        //{
        //    return selectQuery("SELECT * FROM Users_Details WHERE Session =" + session);
        //}

        #endregion
    }

}
