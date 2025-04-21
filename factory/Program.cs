public interface IVehicle
{
    void Drive();
    void Stop();
}

public class Car : IVehicle
{
    public void Drive()
    {
        Console.WriteLine("Автомобиль едет");
    }

    public void Stop()
    {
        Console.WriteLine("Автомобиль останавливается");
    }
}

public class Truck : IVehicle
{
    public void Drive()
    {
        Console.WriteLine("Грузовик везет груз");
    }

    public void Stop()
    {
        Console.WriteLine("Грузовик останавливается");
    }
}

public class Motorcycle : IVehicle
{
    public void Drive()
    {
        Console.WriteLine("Мотоцикл едет");
    }

    public void Stop()
    {
        Console.WriteLine("Мотоцикл останавливается.");
    }
}

public abstract class VehicleCreator
{
    public abstract IVehicle CreateVehicle();

    public void TestDrive()
    {
        IVehicle vehicle = CreateVehicle();
        Console.WriteLine($"select {vehicle.GetType().Name}");
        vehicle.Drive();
        vehicle.Stop();
        Console.WriteLine("end");
    }
}

public class CarCreator : VehicleCreator
{
    public override IVehicle CreateVehicle()
    {
        return new Car();
    }
}

public class TruckCreator : VehicleCreator
{
    public override IVehicle CreateVehicle()
    {
        return new Truck();
    }
}

public class MotorcycleCreator : VehicleCreator
{
    public override IVehicle CreateVehicle()
    {
        return new Motorcycle();
    }
}

public class Program
{
    public static void Main()
    {
        VehicleCreator creator = null;

        while (true)
        {
            Console.WriteLine("\nВыберите тип транспортного средства для создания:");
            Console.WriteLine("1: Автомобиль");
            Console.WriteLine("2: Грузовик");
            Console.WriteLine("3: Мотоцикл");
            Console.WriteLine("0: Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    creator = new CarCreator();
                    break;
                case "2":
                    creator = new TruckCreator();
                    break;
                case "3":
                    creator = new MotorcycleCreator();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный ввод.");
                    creator = null;
                    continue;
            }

            if (creator != null)
            {
                creator.TestDrive();
            }
        }
    }
}
