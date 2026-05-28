using System;

namespace _2026_Csh_Advanced.sprint2_InhPol
{
    // ==============================
    // 1. Ієрархія Employee -> Developer, Tester
    // ==============================
    public class Employee
    {
        internal string name;
        private DateTime hiringDate;

        public Employee(string name, DateTime hiringDate)
        {
            this.name = name;
            this.hiringDate = hiringDate;
        }

        public int Experience()
        {
            var today = DateTime.Now;
            int years = today.Year - hiringDate.Year;
            if (hiringDate.Date > today.AddYears(-years)) years--;
            return years;
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"{name} has {Experience()} years of experience");
        }
    }

    public class Developer : Employee
    {
        private string programmingLanguage;

        public Developer(string name, DateTime hiringDate, string programmingLanguage)
            : base(name, hiringDate)
        {
            this.programmingLanguage = programmingLanguage;
        }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"{name} is {programmingLanguage} programmer");
        }
    }

    public class Tester : Employee
    {
        private bool isAutomation; // виправимо орфографію

        public Tester(string name, DateTime hiringDate, bool isAutomation)
            : base(name, hiringDate)
        {
            this.isAutomation = isAutomation;
        }

        public override void ShowInfo()
        {
            string testerType = isAutomation ? "automated" : "manual";
            Console.WriteLine($"{name} is {testerType} tester and has {Experience()} year(s) of experience");
        }
    }

    // ==============================
    // 2. Абстрактний клас Car та похідні SportCar, Lory
    // ==============================
    public abstract class Car
    {
        internal string mark;
        internal int prodYear;

        public Car(string mark, int prodYear)
        {
            this.mark = mark;
            this.prodYear = prodYear;
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Mark : {mark},\nProducted in {prodYear}");
        }
    }

    public class SportCar : Car
    {
        private int maxSpeed;

        public SportCar(string mark, int prodYear, int maxSpeed) : base(mark, prodYear)
        {
            this.maxSpeed = maxSpeed;
        }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Maximum speed is {maxSpeed}");
        }
    }

    public class Lory : Car
    {
        private double loadCapacity;

        public Lory(string mark, int prodYear, double loadCapacity) : base(mark, prodYear)
        {
            this.loadCapacity = loadCapacity;
        }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"The load capacity is {loadCapacity}");
        }
    }

    // ==============================
    // 3. Віртуальні методи - Science, Mathematics, Physics
    // ==============================
    public class Science
    {
        public virtual void Awards()
        {
            Console.WriteLine("We can obtain The Nobel Prize");
        }
    }

    public class Mathematics : Science
    {
        public override void Awards()
        {
            Console.WriteLine("We don't need any awards, but we still can obtain The Abel Prize that nobody else can!");
        }
    }

    public class Physics : Science
    {
        // не перевизначає Awards, отримує успадковану версію
    }

    // ==============================
    // 4. Абстрактний клас ChessFigure та конкретні фігури
    // ==============================
    public abstract class ChessFigure
    {
        public abstract void Move();
    }

    public class Bishop : ChessFigure
    {
        public override void Move()
        {
            Console.WriteLine("Moves by a diagonal!");
        }
    }

    public class Rook : ChessFigure
    {
        public override void Move()
        {
            Console.WriteLine("Moves straight!");
        }
    }

    // ==============================
    // 5. Статичний клас для демонстрації
    // ==============================
    public static class Inheritance
    {
        public static void RunInheritance()
        {
            Console.WriteLine("========== Sprint2: Inheritance & Polymorphism ==========\n");

            // 1. Демонстрація Employee, Developer, Tester
            Console.WriteLine("--- Employee hierarchy ---");
            Employee emp = new Employee("John", new DateTime(2015, 6, 1));
            emp.ShowInfo();

            Developer dev = new Developer("Alice", new DateTime(2020, 3, 15), "C#");
            dev.ShowInfo();

            Tester tester = new Tester("Bob", new DateTime(2018, 8, 20), true);
            tester.ShowInfo();
            Console.WriteLine();

            // 2. Car, SportCar, Lory
            Console.WriteLine("--- Car hierarchy ---");
            Car[] cars = new Car[]
            {
                new SportCar("Ferrari", 2020, 350),
                new Lory("Volvo", 2019, 15.5)
            };
            foreach (Car car in cars)
            {
                car.ShowInfo();
                Console.WriteLine();
            }

            // 3. Science, Mathematics, Physics
            Console.WriteLine("--- Science awards ---");
            Science sci = new Science();
            sci.Awards();
            Mathematics math = new Mathematics();
            math.Awards();
            Physics phys = new Physics();
            phys.Awards(); // викличе батьківський метод
            Console.WriteLine();

            // 4. Chess figures
            Console.WriteLine("--- Chess figures ---");
            ChessFigure[] figures = new ChessFigure[]
            {
                new Bishop(),
                new Rook()
            };
            foreach (ChessFigure fig in figures)
            {
                fig.Move();
            }
            Console.WriteLine();

            Console.WriteLine("========== End of Sprint2 ==========");
        }
    }
}