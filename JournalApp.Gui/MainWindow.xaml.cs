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
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ViewAllEntries_OnClick(object sender, RoutedEventArgs e)
    {
        //throw new NotImplementedException();
        EntryListBox.ItemsSource = journal.Entries;
        EntryListBox.Visibility =  Visibility.Visible;
        DisplayArea.Visibility =  Visibility.Visible;
    }

    private void Hide_OnCLick(object sender, RoutedEventArgs e)
    {
        EntryListBox.Visibility =  Visibility.Hidden;
        DisplayArea.Visibility =  Visibility.Hidden;
    }

    private void EntryListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //throw new NotImplementedException();
        if (EntryListBox.SelectedItem is JournalEntry entry)
        {
            Title.Text = "Title: " + entry.Name;
            Date.Text = "Date: " + entry.Date.ToShortDateString();
            Content.Text = entry.Content;
        }
    }
}