using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IFileSystemItem
{
    string Name { get; }
    void PrintInfo(int indentLevel = 0);
    long GetSize();
}

public class File : IFileSystemItem
{
    public string Name { get; }
    public long Size { get; }

    public File(string name, long size)
    {
        Name = name;
        Size = size;
        if (size < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "File size cannot be negative.");
        }
    }

    public long GetSize()
    {
        return Size;
    }

    public void PrintInfo(int indentLevel = 0)
    {
        Console.WriteLine($"{new string(' ', indentLevel * 2)}- File: {Name} ({GetSize()} bytes)");
    }
}

public class Directory : IFileSystemItem
{
    public string Name { get; }
    private List<IFileSystemItem> _children = new List<IFileSystemItem>();

    public Directory(string name)
    {
        Name = name;
    }

    public void Add(IFileSystemItem item)
    {
        _children.Add(item);
    }

    public void Remove(IFileSystemItem item)
    {
        _children.Remove(item);
    }

    public long GetSize()
    {
        return _children.Sum(child => child.GetSize());
    }

    public void PrintInfo(int indentLevel = 0)
    {
        Console.WriteLine($"{new string(' ', indentLevel * 2)}+ Directory: {Name} (Total Size: {GetSize()} bytes)");
        foreach (var child in _children)
        {
            child.PrintInfo(indentLevel + 1);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var root = new Directory("МойКомпьютер");

        var file1 = new File("Системный_журнал.log", 1024);
        var file2 = new File("Картинка.png", 5632);
        root.Add(file1);
        root.Add(file2);

        var documents = new Directory("Документы");
        var report = new File("Отчет_за_год.docx", 120500);
        var presentation = new File("Презентация.pptx", 256000);
        documents.Add(report);
        documents.Add(presentation);

        var archive = new Directory("Архив");
        var oldReport = new File("Старый_отчет.doc", 85000);
        var backup = new File("backup_2023.zip", 1572864);
        archive.Add(oldReport);
        archive.Add(backup);
        documents.Add(archive);

        root.Add(documents);

        var music = new Directory("Музыка");
        var track1 = new File("Любимый_трек.mp3", 4096000);
        var track2 = new File("Другой_трек.wav", 15240000);
        music.Add(track1);
        music.Add(track2);
        root.Add(music);

        var config = new File("config.ini", 150);
        root.Add(config);

        Console.WriteLine("Структура файловой системы:");
        root.PrintInfo();

        long totalSize = root.GetSize();
        Console.WriteLine($"\nОбщий размер '{root.Name}': {totalSize} байт");

        long documentsSize = documents.GetSize();
        Console.WriteLine($"Размер папки '{documents.Name}': {documentsSize} байт");

        long musicSize = music.GetSize();
        Console.WriteLine($"Размер папки '{music.Name}': {musicSize} байт");
    }
}
