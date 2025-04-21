using System;

public enum ComplexityLevel
{
    Low,
    Medium,
    High
}

public class SupportTicket
{
    public int Id { get; }
    public string Description { get; }
    public ComplexityLevel Complexity { get; }

    public SupportTicket(int id, string description, ComplexityLevel complexity)
    {
        Id = id;
        Description = description;
        Complexity = complexity;
    }

    public override string ToString()
    {
        return $"Ticket #{Id} [{Complexity}]: {Description}";
    }
}

public abstract class SupportHandler
{
    protected SupportHandler? _nextHandler;

    public void SetNextHandler(SupportHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public abstract void HandleTicket(SupportTicket ticket);
}

public class LowLevelSupport : SupportHandler
{
    public override void HandleTicket(SupportTicket ticket)
    {
        if (ticket.Complexity == ComplexityLevel.Low)
        {
            Console.WriteLine($"[LowLevelSupport]: Решен {ticket}");
        }
        else if (_nextHandler != null)
        {
            Console.WriteLine($"[LowLevelSupport]: Не могу решить {ticket}. Передаю на следующий уровень.");
            _nextHandler.HandleTicket(ticket);
        }
        else
        {
            Console.WriteLine($"[LowLevelSupport]: Не могу решить {ticket} и нет следующего уровня.");
        }
    }
}

public class MiddleLevelSupport : SupportHandler
{
    public override void HandleTicket(SupportTicket ticket)
    {
        if (ticket.Complexity == ComplexityLevel.Medium)
        {
            Console.WriteLine($"[MiddleLevelSupport]: Решен {ticket}");
        }
        else if (_nextHandler != null)
        {
            Console.WriteLine($"[MiddleLevelSupport]: Не могу решить {ticket}. Передаю на следующий уровень.");
            _nextHandler.HandleTicket(ticket);
        }
        else
        {
            Console.WriteLine($"[MiddleLevelSupport]: Не могу решить {ticket} и нет следующего уровня.");
        }
    }
}

public class HighLevelSupport : SupportHandler
{
    public override void HandleTicket(SupportTicket ticket)
    {
        if (ticket.Complexity == ComplexityLevel.High)
        {
            Console.WriteLine($"[HighLevelSupport]: Решен {ticket}");
        }
        else if (_nextHandler != null)
        {
             Console.WriteLine($"[HighLevelSupport]: Получен тикет не моего уровня ({ticket.Complexity}). Передаю дальше.");
            _nextHandler.HandleTicket(ticket);
        }
        else
        {
            Console.WriteLine($"[HighLevelSupport]: Не могу решить {ticket}. Эскалация невозможна.");
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var lowLevel = new LowLevelSupport();
        var midLevel = new MiddleLevelSupport();
        var highLevel = new HighLevelSupport();

        lowLevel.SetNextHandler(midLevel);
        midLevel.SetNextHandler(highLevel);

        var ticket1 = new SupportTicket(1, "Не могу войти в аккаунт (забыл пароль)", ComplexityLevel.Low);
        var ticket2 = new SupportTicket(2, "Сайт медленно загружается", ComplexityLevel.Medium);
        var ticket3 = new SupportTicket(3, "Ошибка 500 на сервере при выполнении операции X", ComplexityLevel.High);
        var ticket4 = new SupportTicket(4, "Как изменить аватар?", ComplexityLevel.Low);
        var ticket5 = new SupportTicket(5, "Подозрение на взлом базы данных", ComplexityLevel.High);

        Console.WriteLine("Обработка Ticket 1");
        lowLevel.HandleTicket(ticket1);
        Console.WriteLine("\nОбработка Ticket 2");
        lowLevel.HandleTicket(ticket2);
        Console.WriteLine("\nОбработка Ticket 3");
        lowLevel.HandleTicket(ticket3);
        Console.WriteLine("\nОбработка Ticket 4");
        lowLevel.HandleTicket(ticket4);
        Console.WriteLine("\nОбработка Ticket 5");
        lowLevel.HandleTicket(ticket5);
    }
}
