using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public sealed class CacheManager
{
    private static readonly Lazy<CacheManager> lazyInstance =
        new Lazy<CacheManager>(() => new CacheManager());
    public static CacheManager Instance => lazyInstance.Value;
    private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

    private CacheManager()
    {
        Console.WriteLine("CacheManager instance created.");
    }

    public object GetValue(string key)
    {
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Attempt get value for key '{key}'");
        object value = _cache.GetOrAdd(key, (k) => {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Cache miss for key '{k}'. Loading from source...");
            return LoadValueFromSource(k);
        });

        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Got value for key '{key}': '{value}'");
        return value;
    }

    public void SetValue(string key, object value)
    {
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Set value for key '{key}' to '{value}'");
        _cache.AddOrUpdate(key, value, (k, oldValue) => value);
         Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Value set for key '{key}'");
    }

    private object LoadValueFromSource(string key)
    {
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: loading '{key}' from external source...");
        Thread.Sleep(100);
        var loadedValue = $"commonValue";
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: loading '{key}'. Value: '{loadedValue}'");
        return loadedValue;
    }

}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting CacheManager Demo...");

        CacheManager cache1 = CacheManager.Instance;
        CacheManager cache2 = CacheManager.Instance;

        Console.WriteLine($"cache1 == cache2? {object.ReferenceEquals(cache1, cache2)}");

        var tasks = new Task[5];
        for (int i = 0; i < tasks.Length; i++)
        {
            int taskId = i;
            tasks[i] = Task.Run(() =>
            {
                CacheManager currentCache = CacheManager.Instance;
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} (Task {taskId}): Got CacheManager instance.");

                object val1 = currentCache.GetValue("commonKey");
                Thread.Sleep(50 + taskId * 10);
                currentCache.SetValue($"key-{taskId}", $"Value set by Task {taskId}");

                object val2 = currentCache.GetValue("commonKey");

                 if (taskId > 0) {
                     object val3 = currentCache.GetValue("key-0");
                 }

                 Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} (Task {taskId}): Instance == cache1? {object.ReferenceEquals(currentCache, cache1)}");
            });
        }

        Task.WaitAll(tasks);

        Console.WriteLine("\nAll tasks completed.");
        CacheManager finalCache = CacheManager.Instance;
        Console.WriteLine("\nCacheManager Demo finished.");
    }
}
