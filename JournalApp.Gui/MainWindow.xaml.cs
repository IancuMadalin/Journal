using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JournalApp.Gui;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Journal journal = Journal.Instance;
    private JournalEntry? currentEntry;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ViewAllEntries_OnClick(object sender, RoutedEventArgs e)
    {
        
        EntryListBox.ItemsSource = journal.Entries;
        EntryListBox.Visibility = Visibility.Visible;
        DisplayArea.Visibility = Visibility.Visible;
        
    }

    private void Hide_OnCLick(object sender, RoutedEventArgs e)
    {
        EntryListBox.Visibility = Visibility.Hidden;
        DisplayArea.Visibility = Visibility.Hidden;
        
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

    private void AddEntryButton_OnClick(object sender, RoutedEventArgs e)
    {
        //needs work on the design
        //might go with Show() instead of ShowDialog() for this one
        //worth pondering
        AddEntryWindow window = new AddEntryWindow();
        window.Owner  = this;
        window.Left = 0;
        window.Top = ButtonStack.PointToScreen(new Point(0, ButtonStack.ActualHeight)).Y;
        window.Width = this.ActualWidth-10;
        window.Height = this.ActualHeight - ButtonStack.ActualHeight-75;
        window.ShowDialog();
    }
    
    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        //add some checks, also should open a different window that asks for an "Are u sure, bro?"
        journal.DeleteEntry(currentEntry);
    }
}