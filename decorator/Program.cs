using System;
using System.IO;
using System.Linq;

public interface IDataSource
{
    void WriteData(string data);
    string ReadData();
}

public class FileDataSource : IDataSource
{
    private readonly string _filename;

    public FileDataSource(string filename)
    {
        _filename = filename ?? throw new ArgumentNullException(nameof(filename));
        Console.WriteLine($"[FileDataSource] Инициализирован для файла '{_filename}'");
    }

    public void WriteData(string data)
    {
        Console.WriteLine($"[FileDataSource] Запись данных в файл '{_filename}'");
        File.WriteAllText(_filename, data);
        Console.WriteLine($"[FileDataSource] Данные '{Truncate(data)}' записаны.");
    }

    public string ReadData()
    {
        Console.WriteLine($"[FileDataSource] Чтение данных из файла '{_filename}'");
        if (!File.Exists(_filename))
        {
            Console.WriteLine($"[FileDataSource] Файл '{_filename}' не найден.");
            return string.Empty;
        }
        string data = File.ReadAllText(_filename);
        Console.WriteLine($"[FileDataSource] Данные '{Truncate(data)}' прочитаны.");
        return data;
    }

    private string Truncate(string value, int maxLength = 50)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
}

public abstract class DataSourceDecorator : IDataSource
{
    protected IDataSource _wrappee;

    protected DataSourceDecorator(IDataSource source)
    {
        _wrappee = source ?? throw new ArgumentNullException(nameof(source));
    }

    public virtual void WriteData(string data)
    {
        _wrappee.WriteData(data);
    }

    public virtual string ReadData()
    {
        return _wrappee.ReadData();
    }

    protected string Truncate(string value, int maxLength = 50)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
}

public class EncryptionDecorator : DataSourceDecorator
{
    public EncryptionDecorator(IDataSource source) : base(source) { }

    public override void WriteData(string data)
    {
        Console.WriteLine("[EncryptionDecorator] Шифрование данных");
        string encryptedData = Encrypt(data);
        Console.WriteLine($"[EncryptionDecorator] Зашифровано в '{Truncate(encryptedData)}'. Передача дальше");
        base.WriteData(encryptedData);
    }

    public override string ReadData()
    {
        Console.WriteLine("[EncryptionDecorator] Получение данных от предыдущего компонента...");
        string encryptedData = base.ReadData();
        Console.WriteLine($"[EncryptionDecorator] Получены данные '{Truncate(encryptedData)}'. Расшифровка");
        string decryptedData = Decrypt(encryptedData);
        Console.WriteLine($"[EncryptionDecorator] Данные расшифрованы в '{Truncate(decryptedData)}'.");
        return decryptedData;
    }

    private string Encrypt(string data)
    {
         char[] charArray = data.ToCharArray();
         Array.Reverse(charArray);
         return $"<encrypted>{new string(charArray)}</encrypted>";
    }

    private string Decrypt(string data)
    {
        if (data.StartsWith("<encrypted>") && data.EndsWith("</encrypted>"))
        {
            data = data.Substring("<encrypted>".Length, data.Length - "<encrypted>".Length - "</encrypted>".Length);
            char[] charArray = data.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        Console.WriteLine("[EncryptionDecorator] ВНИМАНИЕ: Данные не в ожидаемом формате шифрования.");
        return data;
    }
}

public class CompressionDecorator : DataSourceDecorator
{
    public CompressionDecorator(IDataSource source) : base(source) { }

    public override void WriteData(string data)
    {
        Console.WriteLine("[CompressionDecorator] Сжатие данных...");
        string compressedData = Compress(data);
        Console.WriteLine($"[CompressionDecorator] Сжато в '{Truncate(compressedData)}'. Передача дальше");
        base.WriteData(compressedData);
    }

    public override string ReadData()
    {
        Console.WriteLine("[CompressionDecorator] Получение данных от предыдущего компонента");
        string compressedData = base.ReadData();
        Console.WriteLine($"[CompressionDecorator] Получены данные '{Truncate(compressedData)}'. Распаковка");
        string decompressedData = Decompress(compressedData);
         Console.WriteLine($"[CompressionDecorator] Данные распакованы в '{Truncate(decompressedData)}'.");
        return decompressedData;
    }

    private string Compress(string data)
    {
        return $"<compressed>{data}</compressed>";
    }

    private string Decompress(string data)
    {
         if (data.StartsWith("<compressed>") && data.EndsWith("</compressed>"))
         {
             return data.Substring("<compressed>".Length, data.Length - "<compressed>".Length - "</compressed>".Length);
         }
         Console.WriteLine("[CompressionDecorator] ВНИМАНИЕ: Данные не в ожидаемом формате сжатия.");
         return data;
    }
}

class Program
{
    static void Main()
    {
        string filename = "data.txt";
        string originalData = "Hello, World.";

        Console.WriteLine($"Исходные данные: '{originalData}'");
        Console.WriteLine($"Целевой файл: '{filename}'");

        IDataSource source = new FileDataSource(filename);
        source = new EncryptionDecorator(source);
        source = new CompressionDecorator(source);

        Console.WriteLine("Вызов WriteData на внешнем декораторе (Compression)");
        source.WriteData(originalData);

        Console.WriteLine("Вызов ReadData на внешнем декораторе (Compression)");
        string readData = source.ReadData();

        Console.WriteLine($"\nПрочитанные и полностью обработанные данные: '{readData}'");
        Console.WriteLine($"\nПроверка: Исходные данные совпадают с прочитанными? -> {originalData.Equals(readData)}");

        Console.WriteLine("\nСодержимое файла data.txt");
        try
        {
            Console.WriteLine(System.IO.File.ReadAllText(filename));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось прочитать файл: {ex.Message}");
        }

        try
        {
            System.IO.File.Delete(filename);
            Console.WriteLine($"\nФайл '{filename}' удален.");
        }
        catch { }
    }
}
