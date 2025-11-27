using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ===== 1. Єдиний інтерфейс =====
public interface IFileReader
{
    string ReadFile(string path);
}

// ===== 2. Специфічні класи (імітація сторонніх API) =====

// TXT Reader (не можна змінювати)
public class TxtReader
{
    public string LoadTxt(string path)
    {
        return File.ReadAllText(path);
    }
}

// CSV Reader (не можна змінювати)
public class CsvReader
{
    public string[] LoadCsv(string path)
    {
        return File.ReadAllLines(path);
    }
}

// JSON Reader (не можна змінювати)
public class JsonReader
{
    public JToken LoadJson(string path)
    {
        string json = File.ReadAllText(path);
        return JToken.Parse(json); // типізовано, не dynamic
    }
}

// ===== 3. Адаптери з інжекцією залежностей =====

public class TxtReaderAdapter : IFileReader
{
    private readonly TxtReader txtReader;

    public TxtReaderAdapter(TxtReader reader)
    {
        txtReader = reader;
    }

    public string ReadFile(string path)
    {
        ValidateFile(path);
        return txtReader.LoadTxt(path);
    }

    private void ValidateFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"TXT file not found: {path}");
    }
}

public class CsvReaderAdapter : IFileReader
{
    private readonly CsvReader csvReader;

    public CsvReaderAdapter(CsvReader reader)
    {
        csvReader = reader;
    }

    public string ReadFile(string path)
    {
        ValidateFile(path);
        var lines = csvReader.LoadCsv(path);
        return string.Join("\n", lines);
    }

    private void ValidateFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"CSV file not found: {path}");
    }
}

public class JsonReaderAdapter : IFileReader
{
    private readonly JsonReader jsonReader;

    public JsonReaderAdapter(JsonReader reader)
    {
        jsonReader = reader;
    }

    public string ReadFile(string path)
    {
        ValidateFile(path);

        try
        {
            JToken obj = jsonReader.LoadJson(path);
            return obj.ToString(Formatting.Indented);
        }
        catch (JsonException ex)
        {
            throw new Exception($"JSON parsing error in file {path}. Details: {ex.Message}");
        }
    }

    private void ValidateFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"JSON file not found: {path}");
    }
}

// ===== 4. Client =====

class Program
{
    static void Main(string[] args)
    {
        try
        {
            IFileReader txt = new TxtReaderAdapter(new TxtReader());
            IFileReader csv = new CsvReaderAdapter(new CsvReader());
            IFileReader json = new JsonReaderAdapter(new JsonReader());

            Console.WriteLine("=== TXT ===");
            Console.WriteLine(txt.ReadFile("data.txt"));

            Console.WriteLine("\n=== CSV ===");
            Console.WriteLine(csv.ReadFile("data.csv"));

            Console.WriteLine("\n=== JSON ===");
            Console.WriteLine(json.ReadFile("data.json"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
