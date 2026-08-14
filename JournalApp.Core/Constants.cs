namespace JournalApp;

public  static class Constants
{
    public static readonly string savesFolderPath =
        Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!
                .FullName,
            "Saves");
    public enum TitleValidationResult { Valid, Empty, InvalidCharacters, Duplicate }
}