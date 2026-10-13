namespace JournalApp;
using System.Text.Json;
using System.Collections.ObjectModel;

public class Journal
{
    private static Journal? _instance;
    public static Journal Instance => _instance ??= new Journal();

    public ObservableCollection<JournalEntry> Entries { get; } = new();
    //ObservableCollcetion e un list mai fancy, cumva a mers din prima
    public ObservableCollection<string> Tags { get; } = new();
    
    private Journal()
    {
        Directory.CreateDirectory(Constants.EntriesFolderPath);
        try
        {
            foreach (var entryPath in Directory.GetFiles(Constants.EntriesFolderPath))
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
        try
        {
            if (File.Exists(Constants.TagsFilePath))
            {
                string json = File.ReadAllText(Constants.TagsFilePath);
                List<String>? tagsList = JsonSerializer.Deserialize<List<String>>(json);
                if (tagsList != null)
                {
                    foreach (String tag in tagsList)
                    {
                            AddTagWithoutSaving(tag); 
                    }
                }
            }
        }
        catch (JsonException)
        {
            File.Move(Constants.TagsFilePath, Constants.TagsFilePath + ".corrupt", overwrite: true);
        }
        bool registryChanged = false;
        foreach (JournalEntry entry in Entries)
        {
            foreach (String tag in entry.Tags)
            {
                if (AddTagWithoutSaving(tag) == Constants.TagValidationResult.Valid)
                {
                    registryChanged = true;
                }
            }
        }
        if (registryChanged)
        {
            SaveTags();
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
        
             string filePath = Path.Combine(Constants.EntriesFolderPath, $"{name}.json");
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
        if (entry != null)
        {
            File.Delete(entry.GetFilePath());
            Entries.Remove(entry);
        }
            
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

    public static string NormalizeTag(string tag)
    {
        tag = tag.Trim().ToUpperInvariant();
        return tag;
    }

    private Constants.TagValidationResult ValidTag(string tag)
    {
        //expects a normalized tag
        if (tag.Length == 0)
        {
            return Constants.TagValidationResult.Empty;
        }
        if (Tags.Contains(tag))
        {
            return Constants.TagValidationResult.Duplicate;
        }
        return Constants.TagValidationResult.Valid;
    }

    public Constants.TagValidationResult AddTag(string tag)
    {
        var verdict = AddTagWithoutSaving(tag);
        if (verdict == Constants.TagValidationResult.Valid)
        {
            SaveTags();
            return Constants.TagValidationResult.Valid;
        }
        return verdict;
    }
    
    private void SaveTags()
    {
        File.WriteAllText(Constants.TagsFilePath, JsonSerializer.Serialize(Tags));
    }

    private Constants.TagValidationResult AddTagWithoutSaving(string tag)
    {
        tag = NormalizeTag(tag);
        var tagValidationResult = ValidTag(tag);
        if (tagValidationResult == Constants.TagValidationResult.Valid)
        {
            Tags.Add(tag);
        }

        return tagValidationResult;
    }
    
}