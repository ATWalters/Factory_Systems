/*
    This file is a class for measurement objects
*/
using System;

namespace Factory_Systems{
    class Measurement{
        //measurement_uid for the measurment
        private int ID;
        //The X coordinate for the measurement
        private double X;
        //The Y coordinate for the measurement
        private double Y;
        //The height of the measurement
        private double height;

        //4 parameter constructor that takes in the ID, X coord, Y coord and height of the measurement
        public Measurement(int ID, double X, double Y, double height){
            this.ID = ID;
            this.X = X;
            this.Y = Y;
            this.height = height;
        }

        //Getter for the ID of the measurement
        public int getID(){
            return this.ID;
        }

        //Getter for the X coord of the measurement
        public double getX(){
            return this.X;
        }

        //Getter for the Y coord of the measurement
        public double getY(){
            return this.Y;
        }

        //Getter for the height of the measurment
        public double getHeight(){
            return this.height;
        }

        //Overrode ToString() that prints the X coord, Y coord and Height of the measurment
        public override string ToString()
        {
            return "X:" + this.getX() + " Y:" + this.getY() + " Height:" + this.getHeight();
        }
    }
}