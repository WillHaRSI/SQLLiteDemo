using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SQLliteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string testDb = "TestDatabase.sqlite";

            if (!File.Exists(testDb)) SQLiteConnection.CreateFile(testDb);

            SQLiteConnection m_dbConnection = new SQLiteConnection($"Data Source={testDb};");
            m_dbConnection.Open();

            string sql = "CREATE TABLE IF NOT EXISTS testStrings ( stringValue TEXT NOT NULL );";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            string action = "";

            while(!action.Equals("ADD") && !action.Equals("DELETE"))
            {
                Console.WriteLine("Please type an action ADD or DELETE: ");
                action = Console.ReadLine();
            }

            if (action.Equals("ADD"))
            {
                Console.WriteLine("Enter a string value to add to the table: ");
                string inputValue = Console.ReadLine();

                sql = $"INSERT INTO testStrings (stringValue) values ('{inputValue}')";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }

            if (action.Equals("DELETE"))
            {
                Console.WriteLine("Enter a string value to remove from the table: ");
                string inputValue = Console.ReadLine();

                sql = $"DELETE FROM testStrings WHERE stringValue = '{inputValue}'";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }

            sql = "SELECT stringValue FROM testStrings";
            SQLiteCommand sqlCommand = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader sqlRead = sqlCommand.ExecuteReader();

            try
            {
                List<string> lstResults = new List<string>();

                while (sqlRead.Read()) lstResults.Add(sqlRead.GetString(0));

                Console.WriteLine($"Values in table ({lstResults.Count}): ");

                foreach(string s in lstResults) Console.WriteLine(s);
            }

            finally
            {
                sqlRead.Close();
                m_dbConnection.Close();
            }
            Console.ReadKey();
        }
    }
}
