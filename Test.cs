/*
    This file is a class for test objects
*/
using System;
using System.Collections.Generic;

namespace Factory_Systems{
    class Test{
        private int ID;
        private string planeID;
        private List<Measurement> data = new List<Measurement>();

        private double heightRange;

        private Measurement minHeight;

        private Measurement maxHeight;

        private double meanHeight;


        public Test(int ID, string planeID){
            this.ID = ID;
            this.planeID = planeID;
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

        public double getMaxHeight(){
            return this.maxHeight;
        }

        public double getMinHeight(){
            return this.minHeight;
        }

        public double getHeightRange(){
            return this.heightRange;
        }

        public double getMeanHeight(){
            return this.meanHeight;
        }

        public void addDataPoint(int ID, double X, double Y, double height){
            this.data.Add(new Measurement(ID, X, Y, height));
        }

        public override string ToString(){
            return "test_uid: " + this.getID() + "\n PlaneID: " + this.getPlane() + "\n";
        }

        public void findMaxMinHeight(){

            /*************************************************************************************
            This problem could also be approached by doing some SQL statements like the ones below
            SELECT MIN(height) FROM Measurements WHERE test_uid = X; for min and 
            SELECT MAX(height) FROM Measurements WHERE test_uid = X; for max where X is the test
            for which you are wanting to find the max and min height for. I wanted to put this here
            to show another way in which I had thought about approaching the problem before
            deciding on going with the implementation below that doesn't use any SQL.
            *************************************************************************************/

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
        }

        public double findMeanHeight(){

            /*************************************************************************************
            This problem could also be approached by doing some SQL statements like the one below
            SELECT AVG(height) FROM Measurements WHERE test_uid = X; where X is the test for
            which you are wanting to find the mean height for. I wanted to put this here to show
            another way in which I had thought about approaching the problem before deciding
            on going with the implementation below that doesn't use any SQL.
            *************************************************************************************/

            //Start a running total variable
            int runningTotal = 0;
            for(int i = 0; i < this.data.Count; i++){
                //Add the height of each Measurement to the running total
                runningTotal += this.data[i].getHeight();
            }
            //Divide the running total by the number of items in this.data and return
            this.meanHeight = runningTotal / this.data.Count;
            return this.meanHeight;

        }

        public double findHeightRange(){
            //Return the max height - min height, this will give the range of heights
            this.heightRange = this.maxHeight.getHeight() - this.minHeight.getHeight();
            return this.heightRange;
        }

    }
}