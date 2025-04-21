public interface IMessageSender
{
    void Send(string header, string body);
}

public class LegacyPrinter
{
    public void PrintMessage(string text)
    {
        Console.WriteLine("-- Legacy Printer Output --");
        Console.WriteLine($"Печать сообщения: '{text}'");
        Console.WriteLine("---------------------------");
    }
}

public class PrinterAdapter : IMessageSender
{
    private readonly LegacyPrinter _legacyPrinter;

    public PrinterAdapter(LegacyPrinter legacyPrinter)
    {
        _legacyPrinter = legacyPrinter ?? throw new ArgumentNullException(nameof(legacyPrinter));
        Console.WriteLine("PrinterAdapter: Адаптер создан, содержит LegacyPrinter.");
    }

    public void Send(string header, string body)
    {
        Console.WriteLine($"PrinterAdapter: Вызван Send('{header}', '{body}').");
        string combinedMessage = $"[{header}]\n{body}";
        Console.WriteLine($"PrinterAdapter: Данные преобразованы в строку: \"{combinedMessage.Replace("\n", "\\n")}\"");

        Console.WriteLine("PrinterAdapter: Вызываем PrintMessage() у LegacyPrinter");
        _legacyPrinter.PrintMessage(combinedMessage);
        Console.WriteLine("PrinterAdapter: Вызов PrintMessage() завершен.");
    }
}

public class Client
{
    public void SendMessageViaSender(IMessageSender sender, string title, string content)
    {
        Console.WriteLine("\nClient: Вызываем Send() через интерфейс IMessageSender");
        sender.Send(title, content);
        Console.WriteLine("Client: Вызов Send() завершен.");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("\nСоздаем экземпляр LegacyPrinter");
        LegacyPrinter oldPrinter = new LegacyPrinter();

        Console.WriteLine("\nСоздаем экземпляр PrinterAdapter, передавая ему LegacyPrinter");
        IMessageSender messageSenderAdapter = new PrinterAdapter(oldPrinter);

        Client client = new Client();
        string messageHeader = "Важное Уведомление";
        string messageBody = "Система будет перезагружена в полночь.";

        client.SendMessageViaSender(messageSenderAdapter, messageHeader, messageBody);
    }
}
