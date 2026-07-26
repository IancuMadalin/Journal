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

    public void DisplayEntries()
    {
        int i = 1;
        if (Entries.Count == 0)
            Console.WriteLine("No entries found");
        foreach (var entry in Entries)
        {
                Console.WriteLine($"{i++}: {entry.Name}"); 
        }
        
    }

    public void Instructions()
    {
        Console.Write("Instructions: \n");
        Console.Write("Press 0 to end session \n");
        Console.Write("Press 1 to view all entries \n");
        Console.Write("Press 2 to add a new entry \n");
        Console.Write("Press 3 to read an entry \n");
        Console.Write("Press 4 to delete a file \n");
        Console.WriteLine("Press 5 to search an entry");
        Console.Write("\n");
    }

    public bool validTitle(string  title)
    {
        if (title.Length == 0)
        {
            Console.WriteLine("Title cannot be NULL string");
            return false;
        }

        if (title.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
        {
            Console.WriteLine("Tittle cannot contain these characters:  /, \\, :, *, ?, \", <, >, ");
            return false;
        }
        foreach (JournalEntry entry in Entries)
        {
            if (entry.Name == title)
            {
                Console.WriteLine("Entry " + entry.Name + " already exists");
                return false;
            }
        }
        return true;
    }

    public bool askUserConfirmation()
    {
        bool conf;
        while (true)
        {
            Console.WriteLine("You are about to make a change to this journal. Do you wish to continue? (yes/no) ");
            string? userConfirmation = Console.ReadLine();
            if (userConfirmation == "yes" || userConfirmation == "y")
            {
                conf = true;
                break;
            }
            if (userConfirmation == "no" || userConfirmation == "n")
            {
                conf = false;
                break;
            }
            Console.WriteLine("Invalid input");
        }
        return conf;
    }

    public void AddEntry()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        string name, content;
        DateOnly date;
        Console.Write("Entry title >>>  ");
        name = Console.ReadLine() ?? "";
        Console.Write("\n");

        while (!validTitle(name))
        {
            Console.WriteLine("Invalid entry name");
            Console.Write("Entry title >>>  ");
            name = Console.ReadLine() ?? "";
        }
        Console.Write("Entry content >>>  ");
        date = DateOnly.FromDateTime(DateTime.Now);
        content = Console.ReadLine() ?? ""; // chestia asta inseamna ca daca content e null devine string-ul de dupa "??"
        Console.Write("\n");
        if (askUserConfirmation())
        {
             string filePath = Path.Combine(Constants.savesFolderPath, $"{name}.json");
             JournalEntry newEntry = new JournalEntry(name, date, content);
             Entries.Add(newEntry);
             File.WriteAllText(filePath, JsonSerializer.Serialize<JournalEntry>(newEntry));
             Console.WriteLine("Entry saved");
        }
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public void ReadEntry()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("Type the number of the entry you want to read: ");
        int userInput;
        bool intCheck = int.TryParse(Console.ReadLine(), out userInput);
        if (intCheck)
        {
            if (userInput < 1 || userInput > Entries.Count)
            {
                Console.WriteLine("Invalid input, returning to main menu");
            }
            else
            {
                Entries[userInput-1].Display();
            }
        }
        else
        {
            Console.WriteLine("Invalid input, returning to main menu");
        }
        
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public void ReadEntry(JournalEntry entry)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        entry.Display();
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public void DeleteEntry()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Type the number of the entry you want to delete: ");
        int userInput;
        bool intCheck = int.TryParse(Console.ReadLine(), out userInput);
        if (intCheck)
        {
            if (userInput < 1 || userInput > Entries.Count)
            {
                Console.WriteLine("Invalid input, returning to main menu");
            }
            else
            {
                if (askUserConfirmation())
                {
                    File.Delete(Entries[userInput-1].GetFilePath());
                    Entries.RemoveAt(userInput - 1);
                }
                
            }
        }
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public void DeleteEntry(JournalEntry entry)
    {
        File.Delete(entry.GetFilePath());
        Entries.Remove(entry);
        
    }

    public void search()
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.Write("Type the name of the entry you want to search>>> ");
        string target = Console.ReadLine() ?? "";
        string exactMatch = "";
        int i = -1;
        List<JournalEntry> partialMatches = new();
        foreach (JournalEntry entry in Entries)
        {
            if (entry.Name == target)
            {
                exactMatch = entry.Name;
                i = Entries.IndexOf(entry) + 1;
            }
            else if (entry.Name.Contains(target) || target.Contains(entry.Name))
            {
                partialMatches.Add(entry);
            }
        }
        Console.WriteLine("Exact matches:");
        if (exactMatch != "")
        {
            Console.WriteLine($"{i}. {exactMatch}");
        }
        else
        {
            Console.WriteLine("No exact  matches were found");
        }

        if (partialMatches.Count > 0)
        {
            Console.WriteLine($"Other possible matches: ");
            foreach (JournalEntry entry in partialMatches)
            {
                Console.WriteLine($"{Entries.IndexOf(entry) + 1}. {entry.Name}");
            }
        }
        Console.ForegroundColor = ConsoleColor.Green;
    }

    public void OpenMenu()
    {
        int userInput;
        bool running = true;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Welcome to whatever this is!! \n");
        Instructions();
        while (running)
        {
            Console.Write(">>>  ");
            bool intCheck = int.TryParse(Console.ReadLine(), out userInput);
            if (intCheck)
                switch (userInput)
                {
                    case 0:
                        running = false;
                        break;
                    case 1:
                        DisplayEntries(); 
                        break;
                    case 2:
                        AddEntry();
                        break;
                    case 3:
                        ReadEntry();
                        break;
                    case 4:
                        DeleteEntry();
                        break;
                    case 5:
                        search();
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            else
            {
                Console.WriteLine("Invalid input");
            }
        } 
        Console.ResetColor();
    }
}