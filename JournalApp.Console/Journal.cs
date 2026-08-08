namespace JournalApp;
using System.Text.Json;

public class Journal
{
    private static Journal? _instance;
    public static Journal Instance => _instance ??= new Journal();

    public List<JournalEntry> Entries { get; } = new();
    
    private Journal()
    {
        try
        {
            foreach (var entryPath in Directory.GetFiles(Constants.savesFolderPath))
            {
                try
                {
                    string lines = File.ReadAllText(entryPath);
                    JournalEntry? entry = JsonSerializer.Deserialize<JournalEntry>(lines);
                    if (entry != null)
                        Entries.Add(entry);
                    else
                    {
                        Console.WriteLine("Entry not added correctly:");
                        Console.WriteLine($"{entryPath}");
                    }
                }
                catch (JsonException)
                {
                    Console.WriteLine("Entry not added correctly:");
                    Console.WriteLine($"{entryPath}");
                }
            }

        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Directory not found");
        }
        
    }
    
    public Constants.TitleValidationResult validTitle(string  title)
    {
        if (title.Length == 0)
        {
            return Constants.TitleValidationResult.Empty;
        }

        if (title.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            return Constants.TitleValidationResult.InvalidCharacters;
        }
        foreach (JournalEntry entry in Entries)
        {
            if (entry.Name == title)
            {
                return Constants.TitleValidationResult.Duplicate;
            }
        }
        return Constants.TitleValidationResult.Valid;
    }

    
    public void AddEntry(string name, DateOnly date, string content)
    {
        
             string filePath = Path.Combine(Constants.savesFolderPath, $"{name}.json");
             JournalEntry newEntry = new JournalEntry(name, date, content);
             Entries.Add(newEntry);
             File.WriteAllText(filePath, JsonSerializer.Serialize<JournalEntry>(newEntry));
    }
    
    public void ReadEntry(JournalEntry entry)
    {
        entry.Display();
    }
    
    public void DeleteEntry(JournalEntry entry)
    {
        File.Delete(entry.GetFilePath());
        Entries.Remove(entry);
        
    }

    public struct SearchResults
    {
        public JournalEntry? exactmatch;
        public List<JournalEntry> partialMatches = new() ;
        
        public SearchResults() { }
    }
    
    public SearchResults search(string target)
    {
        SearchResults results = new();
        
        
        foreach (JournalEntry entry in Entries)
        {
            if (entry.Name == target)
            {
                results.exactmatch = entry;
            }
            else if (entry.Name.Contains(target) || target.Contains(entry.Name))
            {
                results.partialMatches.Add(entry);
            }
        }
        return results;
        
    }
    
}