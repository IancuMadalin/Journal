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
    private UIElement? currentPanel;

    public MainWindow()
    {
        InitializeComponent();
        AddEntryPanel.Finished += AddEntryPanel_OnFinished;
        DisplayAreaControl.Finished += DisplayAreaControl_OnFinished;
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

    private void HidePanel()
    {
        if(currentPanel != null)
        {
            currentPanel.Visibility = Visibility.Collapsed;
            currentPanel = null;
        }
    }

    private void AddEntryPanel_OnFinished(object sender, EventArgs e)
    {
        HidePanel();
    }

    private void DisplayAreaControl_OnFinished(object sender, EventArgs e)
    {
        HidePanel();
    }
    private void ViewAllEntries_OnClick(object sender, RoutedEventArgs e)
    {
        ShowPanel(DisplayAreaControl);
    }
    
    private void AddEntryButton_OnClick(object sender, RoutedEventArgs e)
    {
        ShowPanel(AddEntryPanel);
    }
    
    
    
}