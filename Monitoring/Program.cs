using System;
using System.IO;

// Делегат для подій монітора файлів
public delegate void FileMonitorDelegate(string filePath);

// Клас для моніторингу файлів
public class FileMonitor
{
    // Події монітора файлів
    public event FileMonitorDelegate OnFileCreated;
    public event FileMonitorDelegate OnFileDeleted;
    public event FileMonitorDelegate OnFileModified;
    public event EventHandler<RenamedEventArgs> OnFileRenamed;

    // Каталог для моніторингу
    private readonly string monitoredDirectory;

    // Об'єкт для моніторингу файлів
    private readonly FileSystemWatcher fileSystemWatcher;

    // Конструктор
    public FileMonitor(string directoryPath)
    {
        monitoredDirectory = directoryPath;

        // Ініціалізація FileSystemWatcher
        fileSystemWatcher = new FileSystemWatcher(monitoredDirectory);

        // Підписка на події FileSystemWatcher
        fileSystemWatcher.Created += OnCreated;
        fileSystemWatcher.Deleted += OnDeleted;
        fileSystemWatcher.Changed += OnChanged;
        fileSystemWatcher.Renamed += OnRenamed;
    }

    // Метод для початку моніторингу
    public void StartMonitoring()
    {
        fileSystemWatcher.EnableRaisingEvents = true;
        Console.WriteLine($"Система моніторингу запущена для каталогу: {monitoredDirectory}");
    }

    // Метод для зупинки моніторингу
    public void StopMonitoring()
    {
        fileSystemWatcher.EnableRaisingEvents = false;
        Console.WriteLine($"Система моніторингу зупинена для каталогу: {monitoredDirectory}");
    }

    // Обробник події створення файлу
    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        OnFileCreated?.Invoke(e.FullPath);
    }

    // Обробник події видалення файлу
    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        OnFileDeleted?.Invoke(e.FullPath);
    }

    // Обробник події зміни файлу
    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        OnFileModified?.Invoke(e.FullPath);
    }

    // Обробник події перейменування файлу
    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        OnFileRenamed?.Invoke(this, e);
    }
}

class Program
{
    static void Main()
    {
        // Шлях до каталогу для моніторингу
        string directoryPath = @"C:\Users\Andre\source\repos\Monitoring";

        // Створення екземпляру класу FileMonitor
        FileMonitor fileMonitor = new FileMonitor(directoryPath);

        // Підписка на події монітора файлів за допомогою лямбда-виразів
        fileMonitor.OnFileCreated += filePath => Console.WriteLine($"Створено файл: {filePath}");
        fileMonitor.OnFileDeleted += filePath => Console.WriteLine($"Видалено файл: {filePath}");
        fileMonitor.OnFileModified += filePath => Console.WriteLine($"Змінено файл: {filePath}");
        fileMonitor.OnFileRenamed += (sender, args) => Console.WriteLine($"Перейменовано файл: {args.OldFullPath} => {args.FullPath}");

        // Початок моніторингу
        fileMonitor.StartMonitoring();

        Console.WriteLine("Система моніторингу файлів запущена. Натисніть Enter для завершення.");
        Console.ReadLine();

        // Зупинка моніторингу перед завершенням програми
        fileMonitor.StopMonitoring();
    }
}
