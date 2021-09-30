/*
    This file is the main driver of the program
*/
using System;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Factory_Systems{
    class Driver{

        public static List<Test> readData(int numTests, SQLiteConnection con){
            List<Test> result = new List<Test>();
            for(int i = 1; i <= numTests; i++){
                using var cmd = new SQLiteCommand("SELECT PlaneID FROM Tests WHERE test_uid =" + i, con);
                //Get the PlaneID for the current Test being processed
                //command.CommandText = "SELECT PlaneID FROM Tests WHERE test_uid =" + i;
                SQLiteDataReader reader = cmd.ExecuteReader();
                //Add the current test to the tests List for use later
                reader.Read();
                result.Add(new Test(i, reader.GetString(0)));

                //Get required data from Measurements table for the current test
                using var cmd1 = new SQLiteCommand("SELECT measurement_uid, x, y, height FROM Measurements WHERE test_uid =" + i, con);
                SQLiteDataReader reader1 = cmd1.ExecuteReader();
                //Loop through the reader adding the data to the result List
                while(reader1.Read()){
                    result[i - 1].addDataPoint(reader1.GetInt32(0), reader1.GetDouble(1), reader1.GetDouble(2), reader1.GetDouble(3));
                }
            }
            return result;
        }


        static void Main(string[] args){
            List<Test> tests = new List<Test>();

            //Getting the filepath to where the database file is stored
            Console.WriteLine("Please enter the full location of the database including file name: ");
            string location = @"Data Source=" + Console.ReadLine();
            //Location of where I have the database file stored currently
            //D:\Life\SurfaceRoughnessDB.db3

            //Sets up a connection to the database and opens it
            using var con = new SQLiteConnection(location);
            con.Open();

            //SQL query to get the number of tests in the database
            string stmt = "SELECT count(*) FROM Tests";
            using var command = new SQLiteCommand(stmt, con);
            int numTests = Int32.Parse(command.ExecuteScalar().ToString());

            tests = readData(numTests, con);

            Console.WriteLine(tests[0].getData()[0].getHeight());
            Console.WriteLine("Mean height of test 1: " + tests[0].findMeanHeight(con));
        }
    }
}