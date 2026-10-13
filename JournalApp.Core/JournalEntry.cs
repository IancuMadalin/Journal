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

    public JournalEntry(string title, DateOnly date, string content)
    {
        Name = title;
        Date = date;
        Content = content;
    }

    public JournalEntry(string title, DateOnly date, string content, List<string> tags)
    {
        Name = title;
        Date = date;
        Content = content;
        Tags = tags;
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
}