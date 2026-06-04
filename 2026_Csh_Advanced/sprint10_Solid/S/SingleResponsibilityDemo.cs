using System;

namespace _2026_Csh_Advanced.sprint10_Solid.S
{
    public static class SingleResponsibilityDemo
    {
        public static void Demo()
        {
            Console.WriteLine("--- S: Single Responsibility Principle ---");
            var report = new Report();
            report.Generate();
            var saver = new ReportSaver();
            saver.SaveToFile(report);
            Console.WriteLine("Report generated and saved. Check good_report.txt");
            Console.WriteLine("(Клас Report відповідає тільки за генерацію, ReportSaver – за збереження)\n");
        }
    }

    public class Report
    {
        public string Content { get; set; }
        public void Generate() => Content = "Report data";
    }

    public class ReportSaver
    {
        public void SaveToFile(Report report) => 
            System.IO.File.WriteAllText("good_report.txt", report.Content);
    }
}