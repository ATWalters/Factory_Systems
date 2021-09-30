/*
    This file is a class for test objects
*/
using System;
using System.Data.SQLite;
using System.Collections.Generic;

namespace Factory_Systems{
    class Test{

        //The number of measurements for a test to be considered valid        private static int CONSIDERED_VALID = 1000;
        private int ID;
        private string planeID;
        private List<Measurement> data = new List<Measurement>();

        private double heightRange;

        private Measurement minHeight;

        private Measurement maxHeight;

        private double meanHeight;

        private double avgRoughness;

        private double rootMeanSqRoughness;

        private bool isValidTest;

        private int numMeasurements = 0;

        public Test(int ID, string planeID){
            this.ID = ID;
            this.planeID = planeID;
        }

        public bool isValid(){
            return this.isValidTest;
        }

        public int getID(){
            return this.ID;
        }

        public string getPlane(){
            return this.planeID;
        }

        public List<Measurement> getData(){
            return this.data;
        }

        public Measurement getMaxHeight(){
            return this.maxHeight;
        }

        public Measurement getMinHeight(){
            return this.minHeight;
        }

        public double getHeightRange(){
            return this.heightRange;
        }

        public double getMeanHeight(){
            return this.meanHeight;
        }

        public double getAvgRoughness(){
            return this.avgRoughness;
        }

        public double getRootMeanSqRoughness(){
            return this.rootMeanSqRoughness;
        }

        public void addDataPoint(int ID, double X, double Y, double height){
            this.data.Add(new Measurement(ID, X, Y, height));
            this.numMeasurements++;
        }

        public void checkValid(){
            if(this.numMeasurements == CONSIDERED_VALID){
                this.isValidTest = true;
            }else{
                this.isValidTest = false;
            }
        }

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

        public double findMeanHeight(SQLiteConnection con){

            using var meanCmd = new SQLiteCommand("SELECT AVG(height) FROM Measurements WHERE test_uid = " + this.getID() + ";", con);

            //Reader to read the result of the max height query
            SQLiteDataReader meanReader = meanCmd.ExecuteReader();
            meanReader.Read();
            //Set the maxHeight for this test
            this.meanHeight = meanReader.GetDouble(0);

            return this.meanHeight;
            
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

        public double findHeightRange(){
            //Return the max height - min height, this will give the range of heights
            this.heightRange = this.maxHeight.getHeight() - this.minHeight.getHeight();
            return this.heightRange;
        }

        public void calcAvgRoughness(){

            //Calculate 𝜇
        }

        public void calcRootMeanSqRoughness(){
            //Calculate 𝜇
        }

        public void calcRootMeanSqRoughness(){

        }

        private double calcMu(){
            //Calculate 𝜇
            double runningTotal = 0;
            foreach(var h in this.data){
                runningTotal += h.getHeight();
            }

        }

    }
}