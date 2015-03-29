using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace physicsgame
{
    class Program
    {
        private const double G = 9.8;
        // Main Program Entry Point
        static void Main(string[] args)
        {
            //Show welcome message
            Console.WriteLine("Welcome to Physics Game. This application will calculate the maximum height of the shell and the distance it will travel along the ground");
            //Ask to input initial degree
            Console.Write("Angle in degrees:");
            //Read the input and calculate the degree in radian
            double theta = float.Parse(Console.ReadLine()) * .0174532925;
            //Ask to input initial speed
            Console.Write("Speed:");
            //Read the input and store in speed
            float speed = float.Parse(Console.ReadLine());
            //Calculate vox and voy
            double vox = speed * Math.Cos(theta);
            double voy = speed * Math.Sin(theta);
            //Calculate t,h and dx
            double t = voy / G;
            double h = voy * (voy / (2 * G));
            double dx = vox * 2 * t;
            //Print messages
            Console.WriteLine("Height of shell at apex: " + Math.Round(h,3));
            Console.WriteLine("Distance shell travels horizontally: " + Math.Round(dx,3));
        }


    }
}
