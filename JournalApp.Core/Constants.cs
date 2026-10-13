namespace JournalApp;

public  static class Constants
{
    public static readonly string AppFolderPath = 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JournalApp");

    public static readonly string EntriesFolderPath =
        Path.Combine(AppFolderPath, "Entries");
    
    public static readonly string TagsFilePath = 
        Path.Combine(AppFolderPath,"Tags.json");
    public enum TitleValidationResult { Valid, Empty, InvalidCharacters, Duplicate }
    
    public enum TagValidationResult { Valid, Empty, Duplicate }
}