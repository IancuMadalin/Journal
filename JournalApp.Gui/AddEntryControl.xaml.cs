using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JournalApp.Gui;

public partial class AddEntryControl : UserControl
{
    public event EventHandler? Finished;
    
    private readonly Journal journal = Journal.Instance;
    public AddEntryControl()
    {
        InitializeComponent();
    }
    
    private void TitleInputBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        AddNewEntryButton.Visibility = Visibility.Visible;
        if (TitleInputBox.Text == "")
        {
            AddNewEntryButton.Visibility = Visibility.Hidden;
        }
    }
    
    private void AddNewEntryButton_OnClick(object sender, RoutedEventArgs e)
    {
        
        NewEntryResponseBlock.Visibility = Visibility.Visible;
        NewEntryResponseBlock.Background  = new SolidColorBrush(Colors.White);
        NewEntryResponseBlock.Foreground = new SolidColorBrush(Colors.Red);
        switch (journal.validTitle(TitleInputBox.Text))
            {
                case Constants.TitleValidationResult.Valid:
                    journal.AddEntry(TitleInputBox.Text, DateOnly.FromDateTime(DateTime.Now), ContentInputBox.Text);
                    NewEntryResponseBlock.Foreground = new SolidColorBrush(Colors.Green);
                    NewEntryResponseBlock.Text = "New Entry added successfully";
                    return;
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
    
    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        Finished?.Invoke(this, EventArgs.Empty);
        
    }
}