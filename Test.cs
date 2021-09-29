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

        public void addDataPoint(int ID, double X, double Y, double height){
            this.data.Add(new Measurement(ID, X, Y, height));
        }

        public override string ToString(){
            return "test_uid: " + this.getID() + "\n PlaneID: " + this.getPlane() + "\n";
        }
    }
}