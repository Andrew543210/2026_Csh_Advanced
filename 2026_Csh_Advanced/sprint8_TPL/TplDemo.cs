using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _2026_Csh_Advanced.sprint8_TPL
{
    // ==============================
    // 1. Робота з потоками (Threads)
    // ==============================
    public static class MultithreadingDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- 1. Multithreading (Thread) ---");
            
            // Передаємо метод у конструктор (компілятор сам загорне його в ParameterizedThreadStart)
            Thread myThread = new Thread(ExecuteWork);
            
            // Запуск із передачею параметра
            myThread.Start("Data for Sprint 8");

            Console.WriteLine("Main thread is waiting for the background thread...");
            myThread.Join(); // Блокуємо головний потік, поки фоновий не завершить роботу
            Console.WriteLine("Background thread finished.");
            Console.WriteLine();
        }

        private static void ExecuteWork(object? data)
        {
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Started with param: {data}");
            Thread.Sleep(1500); // Імітація тривалої роботи
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Work finished.");
        }
    }

    // ==============================
    // 2. Ланцюжки задач TPL (Task & ContinueWith)
    // ==============================
    public static class TaskContinuationDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- 2. TPL (Task & ContinueWith) ---");
            
            // Створюємо першу "холодну" задачу
            Task<int> task1 = new Task<int>(() =>
            {
                Console.WriteLine($"[Task {Task.CurrentId}] Calculating base value...");
                Thread.Sleep(1000);
                return 500;
            });

            // Реєструємо задачу-продовження. Вона запуститься сама, Start() для неї викликати НЕ МОЖНА
            Task task2 = task1.ContinueWith(previousTask =>
            {
                Console.WriteLine($"[Task {Task.CurrentId}] Continuation auto-started! Result: {previousTask.Result}");
            });

            // Стартуємо тільки першу таску
            task1.Start();
            
            // Чекаємо завершення всього ланцюжка
            task2.Wait();
            Console.WriteLine();
        }
    }

    // ==============================
    // 3. Скасування операцій (CancellationToken)
    // ==============================
    public static class CancellationDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- 3. Cancellation Token ---");
            
            using var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            // Запускаємо задачу в пулі потоків
            Task backgroundJob = Task.Run(() => LongRunningProcess(token), token);

            // Головний потік «думає» 1.5 секунди і скасовує таску
            Thread.Sleep(1500);
            Console.WriteLine("--> [Main]: User clicked cancel!");
            cts.Cancel(); // Надсилаємо сигнал скасування

            try
            {
                backgroundJob.Wait(); // Синхронно чекаємо завершення скасованої таски
            }
            catch (AggregateException ex) when (ex.InnerException is OperationCanceledException)
            {
                // Оскільки Wait() синхронний, виключення загортається в AggregateException
                Console.WriteLine("[Main]: Intercepted cancellation exception. Task stopped cleanly.");
            }
            Console.WriteLine();
        }

        private static void LongRunningProcess(CancellationToken token)
        {
            for (int i = 1; i <= 5; i++)
            {
                // Перевіряємо токен на кожному кроці
                token.ThrowIfCancellationRequested();
                Console.WriteLine($"[Task {Task.CurrentId}] Processing step {i}/5...");
                Thread.Sleep(1000);
            }
        }
    }

    // ==============================
    // 4. Синхронізація ресурсів (lock)
    // ==============================
    public static class SynchronizationDemo
    {
        private static readonly object _locker = new(); // Об'єкт-замок
        private static int _sharedCounter = 0;

        public static void Run()
        {
            Console.WriteLine("--- 4. Resource Synchronization (lock) ---");
            _sharedCounter = 0;
            List<Task> tasks = new List<Task>();

            // Запускаємо 500 паралельних потоків на інкремент
            for (int i = 0; i < 500; i++)
            {
                tasks.Add(Task.Run(IncrementCounter));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Final counter value (Expected 500): {_sharedCounter}");
            Console.WriteLine();
        }

        private static void IncrementCounter()
        {
            // Захищаємо критичну секцію від стану гонки (Race Condition)
            lock (_locker)
            {
                _sharedCounter++;
            }
        }
    }
    
    // ==============================
    // 5. Синхронізація ресурсів (SemaphoreSlim)
    // ==============================
    public class ModernSyncDemo
    {
        // Ліміт: одночасно лише 3 задачі
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3, 3);

        public static async Task<int> RunModernAsync()
        {
            Console.WriteLine(">>> Початок асинхронної обробки...");
            int processedCount = 0;
            var tasks = new List<Task>();

            for (int i = 1; i <= 10; i++)
            {
                int taskId = i; // Створюємо локальну копію для замикання
                
                tasks.Add(Task.Run(async () =>
                {
                    // 1. Асинхронно чекаємо на вільний слот
                    await _semaphore.WaitAsync();
                    
                    try
                    {
                        Console.WriteLine($"[Task {taskId}] Вхід у критичну секцію.");
                        
                        // 2. Імітуємо роботу асинхронно (не блокуємо потік)
                        await Task.Delay(1000); 
                        
                        // Безпечно оновлюємо лічильник (атомарна операція)
                        Interlocked.Increment(ref processedCount);
                        
                        Console.WriteLine($"[Task {taskId}] Вихід. Робота завершена.");
                    }
                    finally
                    {
                        // 3. Гарантовано звільняємо слот
                        _semaphore.Release();
                    }
                }));
            }

            // 4. Очікуємо завершення всіх задач без блокування потоку
            await Task.WhenAll(tasks);
            
            Console.WriteLine(">>> Всі задачі завершено.");
            return processedCount; // Повертаємо результат
        }
    }
    // ==============================
    // 6. Статичний клас для запуску спринта
    // ==============================
    public static class TplSprint
    {
        public static void RunTplSprint()
        {
            Console.WriteLine("========== Sprint8: Multithreading & TPL ==========\n");

            MultithreadingDemo.Run();
            TaskContinuationDemo.Run();
            CancellationDemo.Run();
            SynchronizationDemo.Run();

            Console.WriteLine("========== End of Sprint8 ==========");
        }
    }
}

public class Testing
{
    private static SemaphoreSlim _semaphore = new SemaphoreSlim(2, 2);
    public static async Task TPLTest()
    {
        var tasks = new List<Task>();
        Random rnd = new Random(); // Створюємо один раз для всіх

        for (int i = 1; i <= 10; i++)
        {
            int taskId = i; // Це значення "зафіксовано" для кожної таски

            tasks.Add(Task.Run(async () =>
            {
                await _semaphore.WaitAsync();
                
                try
                {
                    Console.WriteLine($"[Task {taskId}] Started. Slots left: {_semaphore.CurrentCount}");
                    
                    // 2. Імітація роботи
                    await Task.Delay(rnd.Next(1000, 3000));
                    
                    Console.WriteLine($"[Task {taskId}] Finished.");
                }
                finally
                {
                    // 3. Гарантовано звільняємо слот
                    _semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
    }
}