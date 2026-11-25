using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JournalApp.Gui;

public partial class EntryEditor : UserControl
{
    public event EventHandler? Finished;
    public event EventHandler<JournalEntry>? EditSaved;
    private readonly Journal journal = Journal.Instance;
    
    private JournalEntry? editingEntry;

    private List<String> workingTags = new();
    
    
    public EntryEditor()
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
        switch (journal.validTitle(TitleInputBox.Text, editingEntry))
            {
                case Constants.TitleValidationResult.Valid:
                    if (editingEntry == null)
                    {
                        journal.AddEntry(TitleInputBox.Text, DateOnly.FromDateTime(DateTime.Now), 
                                                ContentInputBox.Text, workingTags);
                        Reset();
                        NewEntryResponseBlock.Visibility = Visibility.Visible;
                        NewEntryResponseBlock.Foreground = new SolidColorBrush(Colors.Green);
                        NewEntryResponseBlock.Text = "New Entry added successfully";
                        TagPickerPanel.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        var editedEntry = journal.EditEntry(editingEntry, TitleInputBox.Text,ContentInputBox.Text, workingTags);
                        Reset();
                        if (editedEntry != null)
                            EditSaved?.Invoke(this, editedEntry);
                        else
                            Finished?.Invoke(this, EventArgs.Empty);
                    }
                    
                    return;
                case Constants.TitleValidationResult.Empty:
                    NewEntryResponseBlock.Text = "Title can't be empty";
                    return;

                case Constants.TitleValidationResult.InvalidCharacters:
                    NewEntryResponseBlock.Text = "Title cannot contain these characters:  /, \\, :, *, ?, \", <, >, ";
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

    public void LoadEntry(JournalEntry entry)
    {
        Reset();
        editingEntry = entry;
        TitleInputBox.Text = entry.Name;
        ContentInputBox.Text = entry.Content;
        
        workingTags.Clear();
        foreach (var tag in entry.Tags)
        {
            workingTags.Add(tag);
        }
        AddNewEntryButton.Content = "Save changes";
        RefreshTagsDisplay();
    }

    public void Reset()
    {
        TitleInputBox.Text = String.Empty;
        ContentInputBox.Text = String.Empty;
        
        NewEntryResponseBlock.Visibility = Visibility.Hidden;
        AddNewEntryButton.Content = "Add new entry";
        
        TagPickerPanel.Visibility = Visibility.Collapsed;
        TagSearchBox.Text = String.Empty;
        AddTagButton.Content = "Add Tag";
        
        editingEntry = null;
        workingTags.Clear();
        RefreshTagsDisplay();
    }

    private void RefreshTagsDisplay()
    {
        if (workingTags.Count == 0)
            EntryTagsBlock.Text = "No tags";
        else
            EntryTagsBlock.Text = string.Join(", ", workingTags);
        
    }

    private void AddTagButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (TagPickerPanel.Visibility == Visibility.Collapsed)
        {
            TagPickerPanel.Visibility =  Visibility.Visible;
            AddTagButton.Content = "Close menu";
            TagSearchBox.Text = String.Empty;
            RebuildTagOptions();
        }
        else
        {
            TagPickerPanel.Visibility = Visibility.Collapsed;
            AddTagButton.Content =  "Add Tag";
        }
    }

    private void RebuildTagOptions()
    {
        TagMessageBlock.Foreground = Brushes.Black;
        TagOptionsPanel.Children.Clear();
        var searchedTag = Journal.NormalizeTag(TagSearchBox.Text);
        int k = 0;
        foreach (var tag in journal.Tags.OrderBy(x => x))
        {
            if (!tag.Contains(searchedTag))
            {
                continue;
            }
            var box = new CheckBox();
            box.Content = tag;
            box.IsChecked = workingTags.Contains(tag);
            box.Click += TagCheckBox_OnClick;
            TagOptionsPanel.Children.Add(box);
            k++;
        }
        if (journal.Tags.Count == 0)
            TagMessageBlock.Text = "No tags yet. Type a name and click Create Tag";
        else if (k == 0)
            TagMessageBlock.Text = "No tags found";
        else
            TagMessageBlock.Text = String.Empty;
        
    }

    private void TagCheckBox_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox box && box.Content is string tag)
        {
            //RebuildTagOptions();
            if (box.IsChecked == true && !workingTags.Contains(tag))
            {
                workingTags.Add(tag);
            }
            else if (box.IsChecked == false )
            {
                workingTags.Remove(tag);
            }
            RefreshTagsDisplay();
        }
    }

    private void TagSearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        RebuildTagOptions();
    }

    private void CreateTagButton_OnClick(object sender, RoutedEventArgs e)
    {
        switch (journal.CreateTag(TagSearchBox.Text))
        {
            case Constants.TagValidationResult.Valid:
            {
                TagMessageBlock.Foreground =  Brushes.Green;
                RebuildTagOptions();
                TagMessageBlock.Text = $"Tag {TagSearchBox.Text} added successfully";
                return;
                
            }
            case Constants.TagValidationResult.Empty:
            {
                TagMessageBlock.Foreground = Brushes.Red;
                TagMessageBlock.Text = $"Tag cannot be empty";
                return;
            }
            case Constants.TagValidationResult.Duplicate:
            {
                TagMessageBlock.Foreground = Brushes.Red;
                TagMessageBlock.Text = $"Tag {TagSearchBox.Text} already exists";
                return;
            }
        }
    }
}