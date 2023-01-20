/*
	Author: Giovanni Forma
	Date: 1.17.2023
	Description: A library of simple, unoptimized integral approximations. Contains Left Rectangular Approximation Method, Trapezoidal Rule, and Simpsons Rule.
	Source: https://www.mathsisfun.com/calculus/integral-approximations.html
	Notes:
		Function f(x) can be modified for your use
		I am not at Calculus level, so this may be a poor way of doing this.
*/

using System;
using SSTVWorker;

namespace Integration
{
    public class Integrators
    {
        public static double LRAMIntegrate(double a, double b, int rectNum)
        {
            double dx = Math.Abs(a - b) / rectNum;
            double cX = a;
            double area = 0f;
            
            for(int i = 0; i < rectNum; i++)
            {
                cX += dx;
                double height = m(cX);
                double width = dx;
                area += height * width;
            }
            
            return area;
        }
        
        public static double TrapezoidIntegrate(double a, double b, int traNum)
        {
            double dx = Math.Abs(a - b) / traNum;
            double cX = a;
            double area = 0f;
            
            for(int i = 0; i < traNum; i++)
            {
                area += (m(cX) + m(cX + dx) / 2) * dx;
                cX += dx;
            }
            
            return area;
        }
        
        public static double SimpsonsIntegrate(double a, double b, int num, string color)
        {
            // this is simpsons 1/3 rule

            if(num < 6 || !((num % 2) == 0))
            {
                return 0f;
            }

            // 1, 4, 2 ...(4, 2)... 4, 1
            double dx = Math.Abs(a - b) / num;
            double cX = a;
            double area = 0f;

            area += m(cX, color);
            cX += dx;
            area += (4*m(cX, color));
            cX += dx;
            area += (2*m(cX, color));
            cX += dx;

            num -= 4;

            for(int i = 0; i < (num / 2); i ++) {
                area += (4*m(cX, color));
                cX += dx;
                area += (2*m(cX, color));
                cX += dx;
            }

            area += (4*m(cX, color));
            cX += dx;
            area += m(cX, color);
            cX += dx;

            //Console.WriteLine(area);

            
            area = (dx / 3) * area;
            return area;
        }

        public static double CompositeSimpsonsIntegrate(double a, double b, int num, int n, string color)
        {
            // this is composite simpsons 1/3 rule

            if(num < 6 || !((num % 2) == 0))
            {
                return 0f;
            } else if(n < 2) {
                return 0f;
            }

            // 1, 4, 2 ...(4, 2)... 4, 1
            double dx = Math.Abs(a - b) / num;
            double cX = a;
            double area = 0f;
            double h = (b - a) / n;

            for(int i = 0; i < n;) {
                area += SimpsonsIntegrate(a + (i*h), a + ((i+1)*h), num, color);
                i++;
            }

            return area;
        }
        
        public static double m(double xy, string color)
        {
            return SSTVWorker.Program.getColor((int)Math.Floor(xy), color) / 256; // SSTVWorker.Program.f(xy, color);
        }
        public static double m(double xy)
        {
            return SSTVWorker.Program.getColor((int)Math.Floor(xy), "g") / 256; // SSTVWorker.Program.f(xy, color);
        }
    }
}