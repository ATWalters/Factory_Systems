/*
    This file is a class for test objects
*/
using System;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Factory_Systems{
    class Test{

        //The number of measurements for a test to be considered valid 
        private static int CONSIDERED_VALID = 1000;
        //Value for M
        private static int M = 10;
        //value for N
        private static int N = 10;
        //The ID of the test
        private int ID;
        //The Plane ID of the test
        private string planeID;
        //A list that holds all the measurements for the test
        private List<Measurement> data = new List<Measurement>();
        //The height range of the test
        private double heightRange;
        //The minimum height of the test
        private Measurement minHeight;
        //The maximum height of the test
        private Measurement maxHeight;
        //The mean height of the test
        private double meanHeight;
        //The average roughness of the test
        private double avgRoughness;
        //The root mean square roughness of the test
        private double rootMeanSqRoughness;
        //If the test is a valid test or not
        private bool isValidTest;
        //The number of measurements in the test
        private int numMeasurements = 0;
        //The value of Mu for the test
        private double Mu;

        //Two parameter constructor that takes in the test_uid as well as the PlaneID
        public Test(int ID, string planeID){
            this.ID = ID;
            this.planeID = planeID;
        }

        //Returns if the test is a valid test or not
        public bool isValid(){
            return this.isValidTest;
        }

        //Getter for the ID of the test
        public int getID(){
            return this.ID;
        }

        //Getter for the PlaneID of the test
        public string getPlane(){
            return this.planeID;
        }

        //Getter for the measurements in the test
        public List<Measurement> getData(){
            return this.data;
        }

        //Getter for the maximum height
        public Measurement getMaxHeight(){
            return this.maxHeight;
        }

        //Getter for the minimum height
        public Measurement getMinHeight(){
            return this.minHeight;
        }

        //Getter for the height range
        public double getHeightRange(){
            return this.heightRange;
        }

        //Getter for the mean height
        public double getMeanHeight(){
            return this.meanHeight;
        }

        //Getter for the average roughness
        public double getAvgRoughness(){
            return this.avgRoughness;
        }

        //Getter for the root mean square roughness
        public double getRootMeanSqRoughness(){
            return this.rootMeanSqRoughness;
        }

        //Getter for Mu
        public double getMu(){
            return this.Mu;
        }

        //Adds a Measurement to the data list for the test
        public void addDataPoint(int ID, double X, double Y, double height){
            this.data.Add(new Measurement(ID, X, Y, height));
            this.numMeasurements++;
        }

        //Checks to see if the test has enough measurements to be considered a valid test
        public void checkValid(){
            //If a test has exactly CONSIDERED_VALID measurements it is a valid test, any more or less and it is invalid
            if(this.numMeasurements == CONSIDERED_VALID){
                this.isValidTest = true;
            }else{
                this.isValidTest = false;
            }
        }

        //Gets the maximum and minimum height of the test from the database
        public void findMaxMinHeight(SQLiteConnection con){
            
            //Setting up two SQLiteCommands to run a query to get max height and get min height
            using var maxCmd = new SQLiteCommand("SELECT * FROM Measurements WHERE height = (SELECT MAX(height) FROM Measurements WHERE test_uid = " + this.getID() + ");", con);
            using var minCmd = new SQLiteCommand("SELECT * FROM Measurements WHERE height = (SELECT MIN(height) FROM Measurements WHERE test_uid = " + this.getID() + ");", con);

            //Reader to read the result of the max height query
            SQLiteDataReader maxReader = maxCmd.ExecuteReader();
            maxReader.Read();
            //Set the maxHeight for this test
            this.maxHeight = new Measurement(maxReader.GetInt32(0), maxReader.GetDouble(2), maxReader.GetDouble(3), maxReader.GetDouble(4));

            //Reader to read teh result of the min height query
            SQLiteDataReader minReader = minCmd.ExecuteReader();
            minReader.Read();
            //Set the minHeight for this test
            this.minHeight = new Measurement(minReader.GetInt32(0), minReader.GetDouble(2), minReader.GetDouble(3), minReader.GetDouble(4));


            /*************************************************************************************

            Another way I had thought about going to accomplish this before deciding it be better
            to use SQLite queries as they should be faster as well as just more readable for others


            //Start max and min at the first index that way we know we are starting
            // with values that are indeed a part of the data
            this.maxHeight = this.data[0];
            this.minHeight = this.data[0];

            //Loop through the entire data list
            for(int i = 0; i < this.data.Count; i++){
                //Check if max needs to be changed
                if(this.maxHeight.getHeight() < this.data[i].getHeight()){
                    this.maxHeight = this.data[i];
                }

                //check if min needs to be changed
                if(this.minHeight.getHeight() > this.data[i].getHeight()){
                    this.minHeight = this.data[i];
                }
            }
            *************************************************************************************/

        }

        //Gets the mean height from the database for the test
        public void findMeanHeight(SQLiteConnection con){

            using var meanCmd = new SQLiteCommand("SELECT AVG(height) FROM Measurements WHERE test_uid = " + this.getID() + ";", con);

            //Reader to read the result of the max height query
            SQLiteDataReader meanReader = meanCmd.ExecuteReader();
            meanReader.Read();
            //Set the maxHeight for this test
            this.meanHeight = meanReader.GetDouble(0);

            /*************************************************************************************

            Another way I had thought about going to accomplish this before deciding it be better
            to use SQLite queries as they should be faster as well as just more readable for others

            //Start a running total variable
            double runningTotal = 0;
            for(int i = 0; i < this.data.Count; i++){
                //Add the height of each Measurement to the running total
                runningTotal += this.data[i].getHeight();
            }
            //Divide the running total by the number of items in this.data and return
            this.meanHeight = runningTotal / this.data.Count;
            return this.meanHeight;

            *************************************************************************************/
        }


        //Calculates the height range for the test
        public void findHeightRange(){
            //Return the max height - min height, this will give the range of heights
            this.heightRange = this.maxHeight.getHeight() - this.minHeight.getHeight();
        }


        //Calculates the average roughness for the test
        public void calcAvgRoughness(){
            //Call to calcMu()
            calcMu();

            //A running total of the summation
            double runningTotal = 0;
            //loop through the entire list of measurements for this test
            for(int i = 0; i < this.data.Count; i++){
                //Increase runningTotal by the necessary value
                runningTotal += Math.Abs(this.getData()[i].getHeight() - this.getMu());
            }

            //Set avgRoughness by dividing by MN since that is the same as multiplying by 1/MN
            this.avgRoughness = runningTotal / (M * N);

        }

        //Calculates the root mean square roughness for the test
        public void calcRootMeanSqRoughness(){
            //Call to calcMu()
            calcMu();
            //The power to raise Z(X,Y) - Mu to
            int power = 2;

            //A running total of the summation
            double runningTotal = 0;
            //Loop through the entire list of measurements for this test
            for(int i = 0; i < this.data.Count; i++){
                //Increase runningTotal by the necessary value
                runningTotal += Math.Pow(this.getData()[i].getHeight() - this.getMu(), power);
            }

            //Divide by MN since that is the same as multiplying by 1/MN
            runningTotal = runningTotal / (M * N);

            //Raise the value to the power of 0.5
            double temp = Math.Pow(runningTotal, 0.5);

            //Set rootMeanSqRoughness
            this.rootMeanSqRoughness = temp;
        }

        //Calculates Mu to use in the root mean square roughness and average roughness calculations
        private void calcMu(){
            //Calculate 𝜇

            //A running total of the summation
            double runningTotal = 0;

            //loop through the entire list of measurements for this test
            for(int i = 0; i < this.data.Count; i++){
                //Increase runningTotal by the necessary value
                runningTotal += this.getData()[i].getHeight();
            }

            //Divide by MN since that is the same as multiplying by 1/MN
            this.Mu = runningTotal / (M * N);
        }
    }
}