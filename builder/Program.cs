using System.Text;

public enum StorageType
{
    HDD,
    SSD
}

public class Computer
{
    public string Processor { get; internal set; }
    public string VideoCard { get; internal set; }
    public int RamGB { get; internal set; }
    public StorageType StorageType { get; internal set; }
    public int StorageCapacityGB { get; internal set; }
    public string OperatingSystem { get; internal set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"PC config");
        sb.AppendLine($"Processor:        {Processor ?? "Not set"}");
        sb.AppendLine($"Video Card:       {VideoCard ?? "Not set"}");
        sb.AppendLine($"RAM:              {RamGB} GB");
        sb.AppendLine($"Storage:          {StorageCapacityGB} GB {StorageType}");
        sb.AppendLine($"Operating System: {OperatingSystem ?? "Not set"}");
        return sb.ToString();
    }
}

public interface IComputerBuilder
{
    void Reset();
    void BuildProcessor();
    void BuildVideoCard();
    void BuildRAM();
    void BuildStorage();
    void BuildOperatingSystem();
    Computer GetComputer();
}

public class GamingComputerBuilder : IComputerBuilder
{
    private Computer _computer;

    public GamingComputerBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _computer = new Computer();
    }

    public void BuildProcessor()
    {
        _computer.Processor = "Core i9 / Ryzen 9";
    }

    public void BuildVideoCard()
    {
        _computer.VideoCard = "NVIDIA GeForce RTX 4080 / AMD Radeon RX 7900 XTX";
    }

    public void BuildRAM()
    {
        _computer.RamGB = 32;
    }

    public void BuildStorage()
    {
        _computer.StorageType = StorageType.SSD;
        _computer.StorageCapacityGB = 2048;
    }

    public void BuildOperatingSystem()
    {
        _computer.OperatingSystem = "Windows 11 Pro";
    }

    public Computer GetComputer()
    {
        Computer result = _computer;
        Reset();
        return result;
    }
}

public class OfficeComputerBuilder : IComputerBuilder
{
    private Computer _computer;

    public OfficeComputerBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _computer = new Computer();
    }

    public void BuildProcessor()
    {
        _computer.Processor = "Core i3 / Ryzen 3";
    }

    public void BuildVideoCard()
    {
        _computer.VideoCard = "Integrated Graphics";
    }

    public void BuildRAM()
    {
        _computer.RamGB = 8;
    }

    public void BuildStorage()
    {
        _computer.StorageType = StorageType.HDD;
        _computer.StorageCapacityGB = 500;
    }

    public void BuildOperatingSystem()
    {
        _computer.OperatingSystem = "Windows 11 Home";
    }

    public Computer GetComputer()
    {
        Computer result = _computer;
        Reset();
        return result;
    }
}

public class ComputerDirector
{
    private IComputerBuilder _builder;

    public void SetBuilder(IComputerBuilder builder)
    {
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    public void ConstructComputer()
    {
        if (_builder == null)
        {
            throw new InvalidOperationException("Builder not set.");
        }
        _builder.BuildProcessor();
        _builder.BuildVideoCard();
        _builder.BuildRAM();
        _builder.BuildStorage();
        _builder.BuildOperatingSystem();
    }

    public void ConstructComputerWithoutOS()
    {
         if (_builder == null)
        {
            throw new InvalidOperationException("Builder not set.");
        }
        _builder.BuildProcessor();
        _builder.BuildVideoCard();
        _builder.BuildRAM();
        _builder.BuildStorage();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Сборка директором");

        var director = new ComputerDirector();

        var gamingBuilder = new GamingComputerBuilder();
        director.SetBuilder(gamingBuilder);

        Console.WriteLine("собираем игровой пк...");
        director.ConstructComputer();

        Computer gamingComputer = gamingBuilder.GetComputer();
        Console.WriteLine("Игровой пк готов:");
        Console.WriteLine(gamingComputer);

        var officeBuilder = new OfficeComputerBuilder();
        director.SetBuilder(officeBuilder);

        Console.WriteLine("собираем офисный пк...");
        director.ConstructComputer();

        Computer officeComputer = officeBuilder.GetComputer();
        Console.WriteLine("Офисный пк готов:");
        Console.WriteLine(officeComputer);


        Console.WriteLine("\nСборка клиентом");

        var gamingBuilderManual = new GamingComputerBuilder();
        Console.WriteLine("собираем игровой пк (вручную)...");
        gamingBuilderManual.BuildProcessor();
        gamingBuilderManual.BuildVideoCard();
        gamingBuilderManual.BuildRAM();
        gamingBuilderManual.BuildStorage();
        gamingBuilderManual.BuildOperatingSystem();

        Computer gamingComputerManual = gamingBuilderManual.GetComputer();
        Console.WriteLine("Игровой пк (ручная сборка) готов:");
        Console.WriteLine(gamingComputerManual);


        var officeBuilderManual = new OfficeComputerBuilder();
        Console.WriteLine("собираем офисный пк (вручную)...");
        officeBuilderManual.BuildProcessor();
        officeBuilderManual.BuildVideoCard();
        officeBuilderManual.BuildStorage();
        officeBuilderManual.BuildOperatingSystem();

        Computer officeComputerManual = officeBuilderManual.GetComputer();
        Console.WriteLine("Офисный пк (ручная сборка, неполная) готов:");
        Console.WriteLine(officeComputerManual);
    }
}
