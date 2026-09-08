using System.Windows.Controls;
using System.Windows;

namespace JournalApp.Gui;

public partial class DisplayAreaControl : UserControl
{
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
            Date.Text = "Date: " + entry.Date.ToShortDateString();
            Content.Text = entry.Content;
            EditDeleteButtons.Visibility =  Visibility.Visible;
            currentEntry = entry;
        }
        else
        {
            currentEntry = null;
            EditDeleteButtons.Visibility =  Visibility.Hidden;
        }
    }
    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        //add some checks, also should open a different window that asks for an "Are u sure, bro?"
        journal.DeleteEntry(currentEntry);
    }
}