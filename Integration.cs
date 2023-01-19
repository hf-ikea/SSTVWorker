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
        public static double LRAMIntegrate(float a, float b, int rectNum)
        {
            float dx = Math.Abs(a - b) / rectNum;
            float cX = a;
            double area = 0f;
            
            for(int i = 0; i < rectNum; i++)
            {
                cX += dx;
                double height = SSTVWorker.Program.f(cX, SSTVWorker.Program.bitmap);
                double width = dx;
                area += height * width;
            }
            
            return area;
        }
        
        public static double TrapezoidIntegrate(float a, float b, int traNum)
        {
            float dx = Math.Abs(a - b) / traNum;
            float cX = a;
            double area = 0f;
            
            for(int i = 0; i < traNum; i++)
            {
                area += ((SSTVWorker.Program.f(cX, SSTVWorker.Program.bitmap) + SSTVWorker.Program.f(cX + dx, SSTVWorker.Program.bitmap)) / 2) * dx;
                cX += dx;
            }
            
            return area;
        }
        
        public static double SimpsonsIntegrate(float a, float b, int num)
        {
            // this is simpsons 1/3 rule

            if(num < 6 || !((num % 2) == 0))
            {
                return 0f;
            }

            // 1, 4, 2 ...(4, 2)... 4, 1
            float dx = Math.Abs(a - b) / num;
            float cX = a;
            double area = 0f;

            area += SSTVWorker.Program.f(cX, SSTVWorker.Program.bitmap);
            cX += dx;
            area += (4*SSTVWorker.Program.f(cX, SSTVWorker.Program.bitmap));
            cX += dx;
            area += (2*SSTVWorker.Program.f(cX, SSTVWorker.Program.bitmap));
            cX += dx;

            num -= 4;

            for(int i = 0; i < (num / 2); i ++) {
                area += (4*SSTVWorker.Program.f(cX, SSTVWorker.Program.bitmap));
                cX += dx;
                area += (2*SSTVWorker.Program.f(cX, SSTVWorker.Program.bitmap));
                cX += dx;
            }

            area += (4*SSTVWorker.Program.f(cX, SSTVWorker.Program.bitmap));
            cX += dx;
            area += SSTVWorker.Program.f(cX, SSTVWorker.Program.bitmap);
            cX += dx;

            //Console.WriteLine(area);

            
            area = (dx / 3) * area;
            return area;
        }

        public static double CompositeSimpsonsIntegrate(float a, float b, int num, int n)
        {
            // this is composite simpsons 1/3 rule

            if(num < 6 || !((num % 2) == 0))
            {
                return 0f;
            } else if(n < 2) {
                return 0f;
            }

            // 1, 4, 2 ...(4, 2)... 4, 1
            float dx = Math.Abs(a - b) / num;
            float cX = a;
            double area = 0f;
            float h = (b - a) / n;

            for(int i = 0; i < n;) {
                area += SimpsonsIntegrate(a + (i*h), a + ((i+1)*h), num);
                i++;
            }

            return area;
        }
        
        // public static double f(float x)
        // {
        //     //double yabbadabbadoo = MathF.Abs(MathF.Round(MathF.Sin(x)));//MathF.Abs(MathF.Round(MathF.Sin(x * 2) + 0.5f));
        //     //Console.WriteLine(yabbadabbadoo);
        //     //return x % 2; //MathF.Abs(MathF.Log(x)); // ln(x)
        //     return MathF.Sin(x / 100);
        // }
    }
}