using System;
using System.Collections.Generic;
using System.Threading;

public interface IDatabase
{
    string GetData(string query);
}

public class RealDatabase : IDatabase
{
    public string GetData(string query)
    {
        Console.WriteLine($"[RealDatabase] Выполняется ДОЛГИЙ запрос к БД: '{query}'");
        Thread.Sleep(2000);
        string result = $"Данные для запроса '{query}' получены из БД в {DateTime.Now}";
        Console.WriteLine($"[RealDatabase] Запрос '{query}' выполнен.");
        return result;
    }
}

public class DatabaseProxy : IDatabase
{
    private readonly IDatabase _realDatabase;
    private readonly Dictionary<string, string> _cache;

    public DatabaseProxy(IDatabase realDatabase)
    {
        _realDatabase = realDatabase ?? throw new ArgumentNullException(nameof(realDatabase));
        _cache = new Dictionary<string, string>();
        Console.WriteLine("[DatabaseProxy] Прокси инициализирован.");
    }

    public string GetData(string query)
    {
        Console.WriteLine($"[DatabaseProxy] Получен запрос: '{query}'");

        if (_cache.TryGetValue(query, out string cachedResult))
        {
            Console.WriteLine($"[DatabaseProxy] Возвращаем результат из кэша для запроса '{query}'.");
            return cachedResult + " (из кэша)";
        }
        else
        {
            Console.WriteLine($"[DatabaseProxy] Запрашиваем данные у RealDatabase для '{query}'");
            string realResult = _realDatabase.GetData(query);

            _cache[query] = realResult;
            Console.WriteLine($"[DatabaseProxy] Результат для '{query}' сохранен в кэш.");

            return realResult;
        }
    }

    public void ClearCache()
    {
        Console.WriteLine("[DatabaseProxy] Очистка кэша.");
        _cache.Clear();
        Console.WriteLine("[DatabaseProxy] Кэш очищен.");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        IDatabase realDb = new RealDatabase();
        IDatabase database = new DatabaseProxy(realDb);

        Console.WriteLine("\nПервый запрос пользователей");
        string users1 = database.GetData("SELECT * FROM Users WHERE Active=1");
        Console.WriteLine($"Результат: {users1}\n");

        Console.WriteLine("\nВторой запрос пользователей");
        string users2 = database.GetData("SELECT * FROM Users WHERE Active=1");
        Console.WriteLine($"Результат: {users2}\n");

        Console.WriteLine("\nПервый запрос продуктов");
        string products1 = database.GetData("SELECT Name, Price FROM Products");
        Console.WriteLine($"Результат: {products1}\n");

        Console.WriteLine("\nТретий запрос пользователей");
        string users3 = database.GetData("SELECT * FROM Users WHERE Active=1");
        Console.WriteLine($"Результат: {users3}\n");

        Console.WriteLine("\nВторой запрос продуктов");
        string products2 = database.GetData("SELECT Name, Price FROM Products");
        Console.WriteLine($"Результат: {products2}\n");

        ((DatabaseProxy)database).ClearCache();
        Console.WriteLine("\nЗапрос пользователей ПОСЛЕ очистки кэша");
        string users4 = database.GetData("SELECT * FROM Users WHERE Active=1");
        Console.WriteLine($"Результат: {users4}\n");
    }
}
