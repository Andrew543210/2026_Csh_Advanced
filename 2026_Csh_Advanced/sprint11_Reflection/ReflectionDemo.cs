using System;

namespace _2026_Csh_Advanced.sprint11_Reflection;

public static class ReflectionDemo
{
    public static void RunReflectionSprint()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("================ ПЕРЕВІРКА РОБОТИ РЕФЛЕКСІЇ ================\n");

        var validOrder = new OrderDto(101, "Андрій", 1500.50, "4141111122223333", "+380991112233", DateTime.Now);
        var badOrder = new OrderDto(102, "Тестовий Хакер", -50.0, "5168000011112222", "000", DateTime.Now);
        var validOrder2 = new OrderDto(103, "Ігор", 1700.50, "4141111122223333", "+380992212233", DateTime.Now);
       
        ProcessOrder(validOrder, "ОБРОБКА ВАЛІДНОГО ЗАМОВЛЕННЯ");
        ProcessOrder(badOrder, "ОБРОБКА НЕВАЛІДНОГО ЗАМОВЛЕННЯ");
        ProcessOrder(validOrder2, "ОБРОБКА ЩЕ ОДНОГО ВАЛІДНОГО ЗАМОВЛЕННЯ");

        Console.WriteLine("============================================================");
    }
    
    private static void ProcessOrder(object obj, string testLabel)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"--- {testLabel} ---");
        Console.ResetColor();

     
        if (!OrderReflectionProcessor.Validate(obj, out string error))
        {
          
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Помилка валідації]: {error}");
            Console.ResetColor();
            Console.WriteLine("Результат: Об'єкт невалідний. Аудит-лог заблоковано.\n");
            return; 
        }

        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[Валідація успішна! Генеруємо безпечний аудит-лог]:");
        Console.ResetColor();
        
        OrderReflectionProcessor.GenerateAuditLog(obj);
        Console.WriteLine();
    }
}