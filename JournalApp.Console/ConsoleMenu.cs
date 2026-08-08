namespace JournalApp;

public class ConsoleMenu
{
    private readonly Journal journal;
    
    public void DisplayEntries()
    {
        int i = 1;
        if (journal.Entries.Count == 0)
            Console.WriteLine("No entries found");
        foreach (var entry in journal.Entries)
        {
            Console.WriteLine($"{i++}: {entry.Name}"); 
        }
        
    }

    public ConsoleMenu()
    {
        journal = Journal.Instance;
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

    public bool ValidTitle(string title)
    {
        switch (journal.validTitle(title))
        {
            case Constants.TitleValidationResult.Empty:
                Console.WriteLine("Title cannot be NULL string");
                return false;
                
            case Constants.TitleValidationResult.InvalidCharacters:
                Console.WriteLine("Tittle cannot contain these characters:  /, \\, :, *, ?, \", <, >, ");
                return false;
                
            case Constants.TitleValidationResult.Duplicate:
                Console.WriteLine("Entry " + title + " already exists");
                return false;
                
            case Constants.TitleValidationResult.Valid:
                return true;
            default:
                Console.WriteLine("Unknown error");
                return false;
        }
    }
    
    public bool AskUserConfirmation()
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

        while (!ValidTitle(name) )
        {
            Console.WriteLine("Invalid entry name");
            Console.Write("Entry title >>>  ");
            name = Console.ReadLine() ?? "";
        }
        Console.Write("Entry content >>>  ");
        date = DateOnly.FromDateTime(DateTime.Now);
        content = Console.ReadLine() ?? ""; // chestia asta inseamna ca daca content e null devine string-ul de dupa "??"
        Console.Write("\n");
        if (AskUserConfirmation())
        {
            journal.AddEntry(name, date, content);
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
            if (userInput < 1 || userInput > journal.Entries.Count)
            {
                Console.WriteLine("Invalid input, returning to main menu");
            }
            else
            {
                journal.ReadEntry(journal.Entries[userInput-1]);
            }
        }
        else
        {
            Console.WriteLine("Invalid input, returning to main menu");
        }
        
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
            if (userInput < 1 || userInput > journal.Entries.Count)
            {
                Console.WriteLine("Invalid input, returning to main menu");
            }
            else
            {
                if (AskUserConfirmation())
                {
                    journal.DeleteEntry(journal.Entries[userInput-1]);
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.Green;
    }
    
    public void Search()
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.Write("Type the name of the entry you want to search>>> ");
        string target = Console.ReadLine() ?? "";
        
        Journal.SearchResults results = journal.search(target);
        
        Console.WriteLine("Exact matches:");
        if (results.exactmatch != null)
        {
            Console.WriteLine($"{journal.Entries.IndexOf(results.exactmatch) + 1}. {results.exactmatch.Name}");
        }
        else
        {
            Console.WriteLine("No exact  matches were found");
        }

        if (results.partialMatches.Count > 0)
        {
            Console.WriteLine($"Other possible matches: ");
            foreach (JournalEntry entry in results.partialMatches)
            {
                Console.WriteLine($"{journal.Entries.IndexOf(entry) + 1}. {entry.Name}");
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
                        Search();
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