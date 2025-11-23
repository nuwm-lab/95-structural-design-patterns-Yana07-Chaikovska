using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

// -------------------------
// 1. Target Interface
// -------------------------
public interface IFileReader
{
    List<string> Read(string filePath);
}

// -------------------------
// 2. Existing classes (Adaptees)
// -------------------------
public class TxtReader
{
    public List<string> LoadTxt(string path)
    {
        return File.ReadAllLines(path).ToList();
    }
}

public class CsvReader
{
    public List<string[]> LoadCsv(string path)
    {
        return File.ReadAllLines(path)
                   .Select(line => line.Split(','))
                   .ToList();
    }
}

public class JsonReader
{
    public List<string> LoadJson(string path)
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<string>>(json);
    }
}

// -------------------------
// 3. Adapters
// -------------------------

public class TxtReaderAdapter : IFileReader
{
    private readonly TxtReader _txtReader = new TxtReader();

    public List<string> Read(string filePath)
    {
        return _txtReader.LoadTxt(filePath);
    }
}

public class CsvReaderAdapter : IFileReader
{
    private readonly CsvReader _csvReader = new CsvReader();

    public List<string> Read(string filePath)
    {
        var rows = _csvReader.LoadCsv(filePath);
        return rows.Select(r => string.Join(", ", r)).ToList();
    }
}

public class JsonReaderAdapter : IFileReader
{
    private readonly JsonReader _jsonReader = new JsonReader();

    public List<string> Read(string filePath)
    {
        return _jsonReader.LoadJson(filePath);
    }
}

// -------------------------
// 4. Client
// -------------------------

public class FileProcessor
{
    private readonly IFileReader _reader;

    public FileProcessor(IFileReader reader)
    {
        _reader = reader;
    }

    public void PrintFile(string filePath)
    {
        Console.WriteLine($"=== Reading: {Path.GetFileName(filePath)} ===");
        Console.WriteLine();

        var lines = _reader.Read(filePath);

        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }

        Console.WriteLine("\n--------------------------------------\n");
    }
}

// -------------------------
// 5. Demo / Main
// -------------------------

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Demo: Adapter Pattern (TXT, CSV, JSON)\n");

        // !!! Вкажи свої файли тут !!!
        string txtFile = "data.txt";
        string csvFile = "data.csv";
        string jsonFile = "data.json";

        // TXT
        IFileReader txtReader = new TxtReaderAdapter();
        new FileProcessor(txtReader).PrintFile(txtFile);

        // CSV
        IFileReader csvReader = new CsvReaderAdapter();
        new FileProcessor(csvReader).PrintFile(csvFile);

        // JSON
        IFileReader jsonReader = new JsonReaderAdapter();
        new FileProcessor(jsonReader).PrintFile(jsonFile);
    }
}
