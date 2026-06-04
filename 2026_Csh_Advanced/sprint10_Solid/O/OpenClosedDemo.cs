using System;
using System.Collections.Generic;

namespace _2026_Csh_Advanced.sprint10_Solid.O
{
    public static class OpenClosedDemo
    {
        public static void Demo()
        {
            Console.WriteLine("--- O: Open/Closed Principle ---");
            List<IShape> shapes = new List<IShape>
            {
                new Rectangle { Width = 5, Height = 10 },
                new Circle { Radius = 3 },
                new Triangle { Base = 4, Height = 6 }
            };
            foreach (var shape in shapes)
                Console.WriteLine($"Area: {shape.Area():F2}");
            Console.WriteLine("(Кожна нова фігура реалізує IShape, код не змінюється)\n");
        }
    }

    public interface IShape
    {
        double Area();
    }

    public class Rectangle : IShape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Area() => Width * Height;
    }

    public class Circle : IShape
    {
        public double Radius { get; set; }
        public double Area() => Math.PI * Radius * Radius;
    }

    public class Triangle : IShape
    {
        public double Base { get; set; }
        public double Height { get; set; }
        public double Area() => 0.5 * Base * Height;
    }
}