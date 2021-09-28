using System;
using System.Data.SQLite;

namespace Factory_Systems{
    class Driver{
        static void Main(string[] args){
            //Getting the filepath to where the database file is stored
            Console.WriteLine("Please enter the full location of the database including file name: ");
            var location = @"Data Source=" + Console.ReadLine();
            //Location of where I have the database file stored currently
            //D:\Life\SurfaceRoughnessDB.db3

            //Sets up a connection to the database and opens it
            using var con = new SQLiteConnection(location);
            con.Open();

            //string stm = "SELECT sTime FROM Tests WHERE test_uid=2";
            //using var cmd = new SQLiteCommand(stm, con);
            //string version = cmd.ExecuteScalar().ToString();
            //Console.WriteLine(version);

        }
    }
}
