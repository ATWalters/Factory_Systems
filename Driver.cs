/*
    This file is the main driver of the program
*/
using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Factory_Systems{
    class Driver{

        //Method that will query the database and build a list of Test objects that contain
        // the required information
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

        //Method that will perform all of the necessary calculations for a test if it is considered
        // to be a valid test. This method should only be called after readData() has been called.
        public static void performCalculations(List<Test> data, SQLiteConnection con){
            //Loop through each test in data
            foreach(var d in data){
                //Call checkValid method to setup required variables in Test.cs
                d.checkValid();
                //If the test is valid then perform all the other calculations, else just move onto the next test in 'data'
                if(d.isValid()){
                    d.findMaxMinHeight(con);
                    d.findMeanHeight(con);
                    d.findHeightRange();
                    d.calcAvgRoughness();
                    d.calcRootMeanSqRoughness();
                }
            }
        }

        //Method that will generate the summary csv file for the database that was given.
        //This method should only be called after performCalculations() has been called
        public static void genCSV(List<Test> data){
            //StringBuilder to store the data to enter into the csv file
            StringBuilder csvData = new StringBuilder();
            //Setting up the first line of the csv file
            csvData.AppendLine("test_uid,PlaneID,Valid Test,Min height and location,Max height and location,Mean height,Height range,Average roughness,Root mean squre roughness");
            //Loop through data list and append to StringBuilder object
            for(int i = 0; i < data.Count; i++){
                //If the test is a valid test append all calculations else only append the test_uid, PlanID and if it is a valid test or not.
                if(data[i].isValid()){
                    csvData.AppendLine(data[i].getID() + "," + data[i].getPlane() + "," + data[i].isValid() + "," + data[i].getMinHeight().ToString() + "," + data[i].getMaxHeight().ToString() + "," + data[i].getMeanHeight() + "," + data[i].getHeightRange() + "," + data[i].getAvgRoughness() + "," + data[i].getRootMeanSqRoughness());
                }else{
                    csvData.AppendLine(data[i].getID() + "," + data[i].getPlane() + "," + data[i].isValid());
                }
            }

            //Store the csv in the same directory as this Driver.cs file
            string csvPath = "awalters.csv";
            //Write all of the StringBuilder object to the csv file
            File.WriteAllText(csvPath, csvData.ToString());
        }


        //Main method, the driver of the program
        static void Main(string[] args){
            List<Test> tests = new List<Test>();

            //Getting the filepath to where the database file is stored
            Console.WriteLine("Please enter the full location of the database including file name: ");
            string location = @"Data Source=" + Console.ReadLine();

            //Sets up a connection to the database and opens it
            using var con = new SQLiteConnection(location);
            con.Open();

            //SQL query to get the number of tests in the database
            string stmt = "SELECT count(*) FROM Tests";
            using var command = new SQLiteCommand(stmt, con);
            int numTests = Int32.Parse(command.ExecuteScalar().ToString());

            //Calling readData() to build the list of data called 'tests'
            tests = readData(numTests, con);

            //Calling performCalculations() against the list of data
            performCalculations(tests, con);

            //Calling genCSV() to build the summary csv file
            genCSV(tests);
        }
    }
}