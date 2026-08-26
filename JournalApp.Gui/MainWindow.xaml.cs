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
        EntryListBox.Visibility = Visibility.Visible;
        DisplayArea.Visibility = Visibility.Visible;
        AddArea.Visibility = Visibility.Hidden;
    }

    private void Hide_OnCLick(object sender, RoutedEventArgs e)
    {
        EntryListBox.Visibility = Visibility.Hidden;
        DisplayArea.Visibility = Visibility.Hidden;
        AddArea.Visibility = Visibility.Hidden;
        AddNewEntryButton.Visibility = Visibility.Hidden;
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

    private void AddEntryButton_OnClick(object sender, RoutedEventArgs e)
    {
        EntryListBox.Visibility = Visibility.Collapsed;
        DisplayArea.Visibility = Visibility.Hidden;
        AddArea.Visibility = Visibility.Visible;
    }

    private void TitleInputBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        AddNewEntryButton.Visibility = Visibility.Visible;
    }

    private void AddNewEntryButton_OnClick(object sender, RoutedEventArgs e)
    {
        
        NewEntryResponseBlock.Visibility = Visibility.Visible;
        NewEntryResponseBlock.Background  = new SolidColorBrush(Colors.White);
        if (journal.validTitle(TitleInputBox.Text) == Constants.TitleValidationResult.Valid)
        {
            journal.AddEntry(TitleInputBox.Text, DateOnly.FromDateTime(DateTime.Now), ContentInputBox.Text);
            NewEntryResponseBlock.Foreground = new SolidColorBrush(Colors.Green);
            NewEntryResponseBlock.Text = "New Entry added successfully";
        }
        else
        {
            NewEntryResponseBlock.Foreground = new SolidColorBrush(Colors.Red);
            switch (journal.validTitle(TitleInputBox.Text))
            {
                case Constants.TitleValidationResult.Empty:
                    NewEntryResponseBlock.Text = "Title can't be empty";
                    return;

                case Constants.TitleValidationResult.InvalidCharacters:
                    NewEntryResponseBlock.Text = "Tittle cannot contain these characters:  /, \\, :, *, ?, \", <, >, ";
                    return;

                case Constants.TitleValidationResult.Duplicate:
                    NewEntryResponseBlock.Text = "Title already exists";
                    return;
                default:
                    NewEntryResponseBlock.Text = "Unknown error";
                    return;
            }
        }
    }
}