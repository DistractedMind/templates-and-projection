using System;
using System.Collections.Generic;
using System.Linq;

public interface IObserver
{
    void Update(string stockSymbol, decimal price);
}

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string stockSymbol, decimal price);
}

public class StockMarket : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private Dictionary<string, decimal> _stockPrices = new Dictionary<string, decimal>();

    public void SetStockPrice(string stockSymbol, decimal price)
    {
        bool isNew = !_stockPrices.ContainsKey(stockSymbol);
        _stockPrices[stockSymbol] = price;

        Console.WriteLine($"\nStockMarket: Цена {stockSymbol} {(isNew ? "установлена" : "изменена")} на {price:C}");
        Notify(stockSymbol, price);
    }

    public void Attach(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            Console.WriteLine($"{observer.GetType().Name} подписался на обновления StockMarket.");
        }
    }

    public void Detach(IObserver observer)
    {
        if (_observers.Remove(observer))
        {
            Console.WriteLine($"{observer.GetType().Name} отписался от обновлений StockMarket.");
        }
    }

    public void Notify(string stockSymbol, decimal price)
    {
        Console.WriteLine($"StockMarket: Оповещение подписчиков об изменении {stockSymbol}.");
        foreach (var observer in _observers.ToList())
        {
            observer.Update(stockSymbol, price);
        }
    }
}

public class Trader : IObserver
{
    public string Name { get; }

    public Trader(string name)
    {
        Name = name;
    }

    public void Update(string stockSymbol, decimal price)
    {
        Console.WriteLine($"Trader '{Name}': Заметил изменение! {stockSymbol} теперь стоит {price:C}. Анализ.");
        if (stockSymbol == "AAPL" && price < 150)
        {
            Console.WriteLine($"Trader '{Name}': покупает AAPL");
        }
        else if (stockSymbol == "MSFT" && price > 300)
        {
             Console.WriteLine($"Trader '{Name}': продает MSFT");
        }
    }
}

public class Broker : IObserver
{
    public string CompanyName { get; }

    public Broker(string companyName)
    {
        CompanyName = companyName;
    }

    public void Update(string stockSymbol, decimal price)
    {
        Console.WriteLine($"Broker '{CompanyName}': Получено обновление: {stockSymbol} = {price:C}. Обновляю данные клиентов.");
    }
}

public class ObserverDemo
{
    public static void Main()
    {
        var stockMarket = new StockMarket();

        var trader1 = new Trader("Ivan");
        var trader2 = new Trader("Max");
        var broker1 = new Broker("Global Investments");

        stockMarket.Attach(trader1);
        stockMarket.Attach(trader2);
        stockMarket.Attach(broker1);

        stockMarket.SetStockPrice("AAPL", 145.50m);
        stockMarket.SetStockPrice("MSFT", 290.75m);

        stockMarket.Detach(trader2);

        stockMarket.SetStockPrice("AAPL", 148.20m);
        stockMarket.SetStockPrice("MSFT", 305.10m);
    }
}
