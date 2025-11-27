using System;
using System.IO;
using Newtonsoft.Json;

// ===== 1. Єдиний інтерфейс =====
public interface IFileReader
{
    string ReadFile(string path);
}

// ===== 2. Специфічні класи =====

// TXT reader
public class TxtReader
{
    public string LoadTxt(string path)
    {
        return File.ReadAllText(path);
    }
}

// CSV reader
public class CsvReader
{
    public string[] LoadCsv(string path)
    {
        return File.ReadAllLines(path);
    }
}

// JSON reader
public class JsonReader
{
    public dynamic LoadJson(string path)
    {
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject(json);
    }
}

// ===== 3. Адаптери =====

// TXT adapter
public class TxtReaderAdapter : IFileReader
{
    private readonly TxtReader _txtReader = new TxtReader();

    public string ReadFile(string path)
    {
        return _txtReader.LoadTxt(path);
    }
}

// CSV adapter
public class CsvReaderAdapter : IFileReader
{
    private readonly CsvReader _csvReader = new CsvReader();

    public string ReadFile(string path)
    {
        var lines = _csvReader.LoadCsv(path);
        return string.Join("\n", lines);
    }
}

// JSON adapter
public class JsonReaderAdapter : IFileReader
{
    private readonly JsonReader _jsonReader = new JsonReader();

    public string ReadFile(string path)
    {
        var obj = _jsonReader.LoadJson(path);
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }
}

// ===== 4. Client =====

class Program
{
    static void Main(string[] args)
    {
        IFileReader txt = new TxtReaderAdapter();
        IFileReader csv = new CsvReaderAdapter();
        IFileReader json = new JsonReaderAdapter();

        Console.WriteLine("=== TXT FILE ===");
        Console.WriteLine(txt.ReadFile("data.txt"));

        Console.WriteLine("\n=== CSV FILE ===");
        Console.WriteLine(csv.ReadFile("data.csv"));

        Console.WriteLine("\n=== JSON FILE ===");
        Console.WriteLine(json.ReadFile("data.json"));
    }
}