using System;

namespace _2026_Csh_Advanced.sprint10_Solid.L
{
    public static class LiskovSubstitutionDemo
    {
        public static void Demo()
        {
            Console.WriteLine("--- L: Liskov Substitution Principle ---");
            // Добрий приклад:
            GoodRectangle goodRect = new GoodRectangle { Width = 10, Height = 5 };
            GoodSquare goodSquare = new GoodSquare { Side = 5 };
            Console.WriteLine($"Rectangle area: {goodRect.Area()}, Square area: {goodSquare.Area()}");
            Console.WriteLine("(Square не наслідує Rectangle, тому заміна безпечна)\n");
        }
    }

    public abstract class Shape
    {
        public abstract double Area();
    }

    public class GoodRectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public override double Area() => Width * Height;
    }

    public class GoodSquare : Shape
    {
        public double Side { get; set; }
        public override double Area() => Side * Side;
    }
}