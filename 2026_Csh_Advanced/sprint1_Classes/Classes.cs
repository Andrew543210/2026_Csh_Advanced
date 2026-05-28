using System;
using System.Collections.Generic;

namespace _2026_Csh_Advanced.sprint1_Classes
{
    // ==============================
    // 1. CloseableResource (базовий)
    // ==============================
    public abstract class CloseableResource
    {
        public void Close()
        {
            Console.WriteLine("CloseableResource.Close() called");
        }
    }

    // ==============================
    // 2. MyAccessModifiers
    // ==============================
    public class MyAccessModifiers
    {
        private int birthYear;
        protected string personalInfo;
        private protected string IdNumber;
        public static byte averageMiddleAge = 50;

        public MyAccessModifiers(int birthYear, string idNumber, string personalInfo)
        {
            this.birthYear = birthYear;
            this.IdNumber = idNumber;
            this.personalInfo = personalInfo;
        }

        public int Age => DateTime.Now.Year - birthYear;

        public string NickName { get; internal set; }
        internal string Name { get; set; }

        protected internal void HasLivedHalfOfLife() { }

        public override bool Equals(object obj)
        {
            if (obj is MyAccessModifiers other) return this == other;
            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Name, Age, personalInfo);

        public static bool operator ==(MyAccessModifiers left, MyAccessModifiers right)
        {
            if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
            if (ReferenceEquals(right, null)) return false;
            return left.Name == right.Name && left.Age == right.Age && left.personalInfo == right.personalInfo;
        }

        public static bool operator !=(MyAccessModifiers left, MyAccessModifiers right) => !(left == right);
    }

    // ==============================
    // 3. Point
    // ==============================
    public class Point
    {
        private int x;
        private int y;

        public Point(int x, int y) { this.x = x; this.y = y; }
        internal int[] GetXYPair() => new int[] { x, y };
        protected internal double Distance(int x, int y)
        {
            int dx = x - this.x;
            int dy = y - this.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        protected internal double Distance(Point point) => point == null ? 0 : Distance(point.x, point.y);
        public static explicit operator double(Point p) => Math.Sqrt(p.x * p.x + p.y * p.y);
    }

    // ==============================
    // 4. Fraction
    // ==============================
    public class Fraction
    {
        private readonly int numerator;
        private readonly int denominator;

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0) throw new ArgumentException("Denominator cannot be zero.");
            int common = GCD(Math.Abs(numerator), Math.Abs(denominator));
            int sign = (numerator * denominator < 0) ? -1 : 1;
            this.numerator = (Math.Abs(numerator) / common) * sign;
            this.denominator = Math.Abs(denominator) / common;
        }

        private static int GCD(int a, int b)
        {
            while (b != 0) { int t = b; b = a % b; a = t; }
            return a;
        }

        public static Fraction operator +(Fraction f) => f;
        public static Fraction operator -(Fraction f) => new Fraction(-f.numerator, f.denominator);
        public static Fraction operator !(Fraction f) => new Fraction(f.denominator, f.numerator);

        public static Fraction operator +(Fraction a, Fraction b) =>
            new Fraction(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.denominator);
        public static Fraction operator -(Fraction a, Fraction b) =>
            new Fraction(a.numerator * b.denominator - b.numerator * a.denominator, a.denominator * b.denominator);
        public static Fraction operator *(Fraction a, Fraction b) =>
            new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        public static Fraction operator /(Fraction a, Fraction b) =>
            new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);

        public override bool Equals(object obj) =>
            obj is Fraction other && numerator == other.numerator && denominator == other.denominator;
        public override int GetHashCode() => HashCode.Combine(numerator, denominator);
        public static bool operator ==(Fraction a, Fraction b) => Equals(a, b);
        public static bool operator !=(Fraction a, Fraction b) => !Equals(a, b);
        public override string ToString() => $"{numerator} / {denominator}";
    }

    // ==============================
    // 5. DisposePatternImplementer
    // ==============================
    public class DisposePatternImplementer : CloseableResource, IDisposable
    {
        private bool disposed = false;
        ~DisposePatternImplementer() => Dispose(false);
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) Console.WriteLine("Disposing by developer");
                else Console.WriteLine("Disposing by GC");
                Close();
                disposed = true;
            }
        }
    }

    // ==============================
    // 6. Person, Child, Adult
    // ==============================
    public class Person
    {
        protected int yearOfBirth;
        protected string name;
        protected string healthInfo;

        public Person(int yearOfBirth, string name, string healthInfo)
        {
            this.yearOfBirth = yearOfBirth;
            this.name = name;
            this.healthInfo = healthInfo;
        }

        public string GetHealthStatus() => $"{name}: {yearOfBirth}. {healthInfo}";
    }

    public class Child : Person
    {
        private string childIDNumber;
        public Child(int yearOfBirth, string name, string healthInfo, string childIDNumber)
            : base(yearOfBirth, name, healthInfo) { this.childIDNumber = childIDNumber; }
        public string GetHealthStatus() => $"{name}: {yearOfBirth}. {healthInfo}";
        public override string ToString() => $"{name} {childIDNumber}";
    }

    public class Adult : Person
    {
        private string passportNumber;
        public Adult(int yearOfBirth, string name, string healthInfo, string passportNumber)
            : base(yearOfBirth, name, healthInfo) { this.passportNumber = passportNumber; }
        public string GetHealthStatus() => $"{name}: {yearOfBirth}. {healthInfo}";
        public override string ToString() => $"{name} {passportNumber}";
    }

    // ==============================
    // Клас для запуску всіх демонстрацій (як Collections.RunCollections)
    // ==============================
    public static class Classes
    {
        public static void RunClasses()
        {
            Console.WriteLine("========== Sprint1: Classes, Access Modifiers, System.Object ==========\n");

            // 1. Point
            Console.WriteLine("--- Point ---");
            Point p = new Point(3, 4);
            Console.WriteLine($"Point (3,4) as double: {(double)p}");
            Console.WriteLine($"Distance to (0,0): {p.Distance(0, 0)}");
            Console.WriteLine();

            // 2. Fraction
            Console.WriteLine("--- Fraction ---");
            Fraction f1 = new Fraction(1, 2);
            Fraction f2 = new Fraction(1, 3);
            Console.WriteLine($"{f1} + {f2} = {f1 + f2}");
            Console.WriteLine($"{f1} * {f2} = {f1 * f2}");
            Console.WriteLine($"!{f1} = {!f1}");
            Console.WriteLine();

            // 3. Person hierarchy
            Console.WriteLine("--- Person hierarchy ---");
            Child child = new Child(2010, "Alice", "Good", "ID123");
            Adult adult = new Adult(1990, "Bob", "Excellent", "AB567");
            Console.WriteLine(child);
            Console.WriteLine(child.GetHealthStatus());
            Console.WriteLine(adult);
            Console.WriteLine(adult.GetHealthStatus());
            Console.WriteLine();

            // 4. Dispose pattern
            Console.WriteLine("--- Dispose pattern ---");
            using (var resource = new DisposePatternImplementer())
            {
                Console.WriteLine("Inside using block");
            }
            Console.WriteLine();

            // 5. MyAccessModifiers (short demo)
            Console.WriteLine("--- MyAccessModifiers ---");
            var obj = new MyAccessModifiers(2000, "ID1", "Personal info");
            obj.Name = "John";
            Console.WriteLine($"Age: {obj.Age}");
            Console.WriteLine($"Equals to self: {obj.Equals(obj)}");
            Console.WriteLine();

            Console.WriteLine("========== End of Sprint1 ==========");
        }
    }
}