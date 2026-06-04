using System;

namespace _2026_Csh_Advanced.sprint10_Solid.I
{
    public static class InterfaceSegregationDemo
    {
        public static void Demo()
        {
            Console.WriteLine("--- I: Interface Segregation Principle ---");
            GoodHuman human = new GoodHuman();
            GoodRobot robot = new GoodRobot();
            human.Work(); human.Eat(); human.Sleep();
            robot.Work();
            Console.WriteLine("(Robot не змушений реалізовувати непотрібні методи)\n");
        }
    }

    public interface IWorkable { void Work(); }
    public interface IEatable { void Eat(); }
    public interface ISleepable { void Sleep(); }

    public class GoodHuman : IWorkable, IEatable, ISleepable
    {
        public void Work() => Console.WriteLine("Human works");
        public void Eat() => Console.WriteLine("Human eats");
        public void Sleep() => Console.WriteLine("Human sleeps");
    }

    public class GoodRobot : IWorkable
    {
        public void Work() => Console.WriteLine("Robot works");
    }
}