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
    private UIElement? currentPanel;

    public MainWindow()
    {
        InitializeComponent();
        AddEntryPanel.Finished += AddEntryPanel_OnFinished;
    }

    private void ShowPanel(UIElement panel)
    {
        if(currentPanel != null)
            {
            currentPanel.Visibility = Visibility.Collapsed;
            }
        panel.Visibility = Visibility.Visible;
        currentPanel = panel;
    }

    private void AddEntryPanel_OnFinished(object sender, EventArgs e)
    {
        AddEntryPanel.Visibility = Visibility.Collapsed;
        ButtonStack.IsEnabled = true;
    }
    private void ViewAllEntries_OnClick(object sender, RoutedEventArgs e)
    {
        
        //EntryListBox.ItemsSource = journal.Entries;
        //EntryListBox.Visibility = Visibility.Visible;
        //DisplayArea.Visibility = Visibility.Visible;
        ShowPanel(DisplayAreaControl);
    }

    

    

    private void AddEntryButton_OnClick(object sender, RoutedEventArgs e)
    {
        ShowPanel(AddEntryPanel);
        
    }
    
    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        //add some checks, also should open a different window that asks for an "Are u sure, bro?"
        journal.DeleteEntry(currentEntry);
    }
    
    
}