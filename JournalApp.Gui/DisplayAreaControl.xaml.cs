using System.Windows.Controls;
using System.Windows;

namespace JournalApp.Gui;

public partial class DisplayAreaControl : UserControl
{
    public event EventHandler? Finished;
    public event EventHandler<JournalEntry>? EditRequested;
    
    private readonly Journal journal = Journal.Instance;
    private JournalEntry? currentEntry;
    
    public DisplayAreaControl()
    {
        InitializeComponent();
        EntryListBox.ItemsSource = journal.Entries;
    }
    private void EntryListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (EntryListBox.SelectedItem is JournalEntry entry)
        {
            Title.Text = "Title: " + entry.Name;
            Tags.Text = "Tags: " + entry.Tags;
            Date.Text = "Date: " + entry.Date.ToShortDateString();
            //LastDate.Text = "Last modified: " + entry.LastModified;
            EntryContent.Text = entry.Content;
            
            EditDeleteButtons.Visibility =  Visibility.Visible;
            currentEntry = entry;
        }
        else
        {
            Title.Text = String.Empty;
            Date.Text = String.Empty;
            EntryContent.Text = String.Empty;
            currentEntry = null;
            EditDeleteButtons.Visibility =  Visibility.Hidden;
        }
    }
    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        //add some checks, also should open a different window that asks for an "Are u sure, bro?"
        journal.DeleteEntry(currentEntry);
    }

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        Finished?.Invoke(this, EventArgs.Empty);
    }

    private void EditButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (currentEntry == null)
            return;
        EditRequested?.Invoke(this, currentEntry);
    }

    public void SelectEntry(JournalEntry entry)
    {
        EntryListBox.SelectedItem = entry;
        EntryListBox.ScrollIntoView(entry);
    }
}