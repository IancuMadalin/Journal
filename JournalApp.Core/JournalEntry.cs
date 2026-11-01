namespace JournalApp;   

public class JournalEntry
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public string Content { get; set; }
    public DateTime? LastModified { get; set; }
    public List<string> Tags { get; set; } = new();
    
    public JournalEntry()
    {}
    
    public JournalEntry(string title, DateOnly date, string content, List<string>? tags = null)
    {
        Name = title;
        Date = date;
        Content = content;
        if (tags != null)
            foreach (string tag in tags)
            {
                AddTag(tag);
            }
    }
    public void Display()
    {
        Console.WriteLine($"Title: {Name}");
        Console.WriteLine($"Date: {Date}");
        Console.WriteLine($"Content: {Content}");
    }
    public string GetFilePath()
    {
        return Path.Combine(Constants.EntriesFolderPath, $"{Name}.json");
    }
    
    public void AddTag(string tag)
    {
        tag = Journal.NormalizeTag(tag);
        if (!Tags.Contains(tag) && tag.Length > 0)
            Tags.Add(tag);
    }

    public bool RemoveTag(string tag)
    {
        tag = Journal.NormalizeTag(tag);
        return Tags.Remove(tag);
    }
    
}