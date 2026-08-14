namespace JournalApp;   

public class JournalEntry
{
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public string Content { get; set; }

    public JournalEntry()
    {}

    public JournalEntry(string title, DateOnly date, string content)
    {
        Name = title;
        Date = date;
        Content = content;
    }
    
    public void Display()
    {
        Console.WriteLine($"Title: {Name}");
        Console.WriteLine($"Date: {Date}");
        Console.WriteLine($"Content: {Content}");
    }
    public string GetFilePath()
    {
        return Path.Combine(Constants.savesFolderPath, $"{Name}.json");
    }
}