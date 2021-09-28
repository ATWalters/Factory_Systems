using System;
using System.Data.SQLite;

namespace Factory_Systems{
    class Driver{
        static void Main(string[] args){
            /*
            Lines that just make sure I properly installed the SQLite package
            string cs = "Data Source=:memory:";
            string stm = "SELECT SQLITE_VERSION()";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(stm, con);
            string version = cmd.ExecuteScalar().ToString();

            Console.WriteLine($"SQLite version: {version}");
            */

            //Getting the filepath to where the database file is stored
            Console.WriteLine("Please enter the full location of the database: ");
            //string location = Console.ReadLine();

        }
    }
}
