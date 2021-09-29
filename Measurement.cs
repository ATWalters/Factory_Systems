/*
    This file is a class for measurement objects
*/
using System;

namespace Factory_Systems{
    class Measurement{
        private int ID;
        private double X;
        private double Y;
        private double height;

        public Measurement(int ID, double X, double Y, double height){
            this.ID = ID;
            this.X = X;
            this.Y = Y;
            this.height = height;
        }

        public int getID(){
            return this.ID;
        }

        public double getX(){
            return this.X;
        }

        public double getY(){
            return this.Y;
        }

        public double getHeight(){
            return this.height;
        }
    }
}