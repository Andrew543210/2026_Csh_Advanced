using System;
using System.Collections.Generic;
using System.Linq;

namespace _2026_Csh_Advanced.sprint10_Solid
{
    #region 0. DATA MODELS (Чисті сутності)

    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString()[..8];
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Наприклад: "User", "Admin", "VIP"
    }

    #endregion

    #region S - SINGLE RESPONSIBILITY PRINCIPLE (Єдина відповідальність)

    // Кожен сервіс відповідає лише за ОДНУ логічну операцію.

    public interface ICustomLogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
    }

    public class AppLogger : ICustomLogger
    {
        public void LogInfo(string message) =>
            Console.WriteLine($"[INFO] [{DateTime.Now:HH:mm:ss}] {message}");

        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARN] [{DateTime.Now:HH:mm:ss}] {message}");
            Console.ResetColor();
        }

        public interface IUserValidator
        {
            void Validate(string username, string email, string password);
        }

        public class UserValidator : IUserValidator
        {
            public void Validate(string username, string email, string password)
            {
                if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
                    throw new ArgumentException("Ім'я користувача має містити не менше 3 символів.");

                if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                    throw new ArgumentException("Некоректний формат Email адреси.");

                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                    throw new ArgumentException("Пароль має бути не менше 6 символів.");
            }
        }

        public interface IPasswordHasher
        {
            string Hash(string password);
        }

        public class Sha256PasswordHasher : IPasswordHasher
        {
            public string Hash(string password)
            {
                // Імітація криптографічного хешування з сіллю
                var bytes = System.Text.Encoding.UTF8.GetBytes(password + "Salt_2026");
                var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
                return Convert.ToBase64String(hashBytes)[..20];
            }
        }

        #endregion

        #region I - INTERFACE SEGREGATION PRINCIPLE (Розділення інтерфейсів)

        // Замість одного великого "IUserRepository", ми розділили операції читання та запису.
        // Це дозволяє гнучко масштабувати систему (наприклад, читати з репліки БД, а писати в мастер-БД).

        public interface IUserReader
        {
            bool Exists(string email);
            User? GetByEmail(string email);
        }

        public interface IUserWriter
        {
            void Save(User user);
        }

        public class SqlUserRepository : IUserReader, IUserWriter
        {
            // Імітація збереження в пам'яті (In-Memory DB)
            private readonly List<User> _database = new();
            private readonly ICustomLogger _logger;

            public SqlUserRepository(ICustomLogger logger) => _logger = logger;

            public bool Exists(string email) =>
                _database.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            public User? GetByEmail(string email) =>
                _database.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            public void Save(User user)
            {
                _database.Add(user);
                _logger.LogInfo($"[Database] Користувач #{user.Id} ({user.Username}) успішно записаний в PostgreSQL.");
            }
        }

        #endregion

        #region O & L - OPEN/CLOSED & LISKOV SUBSTITUTION PRINCIPLES

        // OCP: Система відкрита для нових типів сповіщень, але закрита для зміни ядра.
        // LSP: Жоден канал сповіщень не викидає NotImplementedException, всі поводяться передбачувано.

        public interface INotificationChannel
        {
            string SupportedRole { get; }
            void SendWelcome(string destination, string username);
        }

        public class EmailNotificationChannel : INotificationChannel
        {
            public string SupportedRole => "User";

            public void SendWelcome(string destination, string username) =>
                Console.WriteLine($"[SMTP Email] Надіслано листа на {destination}: 'Вітаємо, {username}!'");
        }

        public class TelegramNotificationChannel : INotificationChannel
        {
            public string SupportedRole => "Admin";

            public void SendWelcome(string destination, string username) =>
                Console.WriteLine(
                    $"[Telegram Bot] Надіслано терміновий Alert адміністратору ({destination}): Надійшов новий адмін {username}.");
        }

        public class SmsNotificationChannel : INotificationChannel
        {
            public string SupportedRole => "VIP";

            public void SendWelcome(string destination, string username) =>
                Console.WriteLine(
                    $"[SMS Gateway] Надіслано преміум SMS на {destination}: 'Привіт, VIP-клієнте {username}!'");
        }

        // Менеджер сповіщень, який оркеструє канали, не порушуючи OCP
        public interface INotificationProcessor
        {
            void NotifyUser(User user);
        }

        public class NotificationProcessor : INotificationProcessor
        {
            private readonly IEnumerable<INotificationChannel> _channels;
            private readonly ICustomLogger _logger;

            // Патерн "Стратегія": приймаємо ВСІ зареєстровані канали списком
            public NotificationProcessor(IEnumerable<INotificationChannel> channels, ICustomLogger logger)
            {
                _channels = channels;
                _logger = logger;
            }

            public void NotifyUser(User user)
            {
                // Шукаємо потрібний канал під роль користувача
                var channel = _channels.FirstOrDefault(c =>
                    c.SupportedRole.Equals(user.Role, StringComparison.OrdinalIgnoreCase));

                if (channel != null)
                {
                    channel.SendWelcome(user.Email, user.Username);
                }
                else
                {
                    _logger.LogWarning(
                        $"Для ролі '{user.Role}' не знайдено виділеного каналу сповіщень. Використано Email за замовчуванням.");
                    new EmailNotificationChannel().SendWelcome(user.Email, user.Username);
                }
            }
        }

        #endregion

        #region D - DEPENDENCY INVERSION PRINCIPLE (Інверсія залежностей)

        // Високорівневий сервіс повністю ізольований від конкретики. 
        // Він керує процесом, спираючись виключно на інтерфейси контракти.

        public class UserService
        {
            private readonly IUserValidator _validator;
            private readonly IPasswordHasher _hasher;
            private readonly IUserWriter _userWriter;
            private readonly IUserReader _userReader;
            private readonly INotificationProcessor _notificationProcessor;
            private readonly ICustomLogger _logger;

            // Повна інверсія залежностей через конструктор
            public UserService(
                IUserValidator validator,
                IPasswordHasher hasher,
                IUserWriter userWriter,
                IUserReader userReader,
                INotificationProcessor notificationProcessor,
                ICustomLogger logger)
            {
                _validator = validator;
                _hasher = hasher;
                _userWriter = userWriter;
                _userReader = userReader;
                _notificationProcessor = notificationProcessor;
                _logger = logger;
            }

            public void RegisterUser(string username, string email, string password, string role)
            {
                _logger.LogInfo($"=== Початок реєстрації користувача: {username} ===");

                // 1. Валідація (S)
                _validator.Validate(username, email, password);

                // 2. Перевірка дублікатів (I / S)
                if (_userReader.Exists(email))
                    throw new InvalidOperationException($"Користувач з email {email} вже є в системі.");

                // 3. Хешування (S)
                string passwordHash = _hasher.Hash(password);

                // 4. Створення сутності
                var user = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = passwordHash,
                    Role = role
                };

                // 5. Збереження в репозиторій (D / I)
                _userWriter.Save(user);

                // 6. Сповіщення користувача залежно від стратегії ролі (O / L)
                _notificationProcessor.NotifyUser(user);

                _logger.LogInfo($"=== Користувач {username} успішно зареєстрований з роллю [{role}] ===\n");
            }
        }

        #endregion

        #region ТОЧКА ВХОДУ ДЛЯ ЗАПУСКУ СКОМПОНОВАНОЇ СИСТЕМИ

        public static class SolidDemo
        {
            public static void RunSolid()
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                // --- КОМПОЗИЦІЙНИЙ КОРІНЬ (Composition Root) ---
                // Тут ми один раз збираємо наш DI-пазл вручну без зовнішніх бібліотек
                ICustomLogger logger = new AppLogger();
                IUserValidator validator = new UserValidator();
                IPasswordHasher hasher = new Sha256PasswordHasher();

                var sqlRepo = new SqlUserRepository(logger);
                IUserReader userReader = sqlRepo;
                IUserWriter userWriter = sqlRepo;

                // Створюємо список підтримуваних каналів сповіщень (Демонстрація OCP)
                var channels = new List<INotificationChannel>
                {
                    new EmailNotificationChannel(),
                    new TelegramNotificationChannel(),
                    new SmsNotificationChannel()
                };
                INotificationProcessor notificationProcessor = new NotificationProcessor(channels, logger);

                // Збираємо головний сервіс
                var userService = new UserService(validator, hasher, userWriter, userReader, notificationProcessor,
                    logger);


                // --- ДЕМОНСТРАЦІЯ РОБОТИ ---

                try
                {
                    // Кейс 1: Реєстрація звичайного користувача (Email сповіщення)
                    userService.RegisterUser("andrii_dev", "andrii@gmail.com", "securePass123", "User");

                    // Кейс 2: Реєстрація Адміністратора (Спрацьовує Telegram канал без жодних if-else в UserService)
                    userService.RegisterUser("root_admin", "admin@company.com", "superSecret999", "Admin");

                    // Кейс 3: Реєстрація VIP клієнта (Спрацьовує Premium SMS канал)
                    userService.RegisterUser("elite_client", "vip@luxury.com", "grandPassword77", "VIP");

                    // Кейс 4: Перевірка валідатора (Має відловити помилку паролю)
                    Console.WriteLine("--- Тест валідації (очікуємо помилку) ---");
                    userService.RegisterUser("bad_user", "test@test.com", "123", "User");
                }
                catch (Exception ex)
                {
                    logger.LogWarning($"Перехоплено виняток: {ex.Message}");
                }
            }
        }

        #endregion
    }
};