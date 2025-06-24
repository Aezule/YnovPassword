using Avalonia.Controls;
using Avalonia.Interactivity;

namespace YnovPassword.Views;

public partial class ConfirmDialog : Window
{
    public ConfirmDialog()
    {
        InitializeComponent();
    }

    public ConfirmDialog(string message) : this()
    {
        MessageTextBlock.Text = message;
    }

    public bool? Result { get; private set; }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
        Result = true;
        Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Result = false;
        Close(false);
    }
}
