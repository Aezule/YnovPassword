using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using YnovPassword.Modele;

namespace YnovPassword.Views;

public partial class AddTypeProfileWindow : Window
{
    public string? NewTypeProfileName { get; private set; }
    private PasswordContext _dbContext = new PasswordContext();


    public AddTypeProfileWindow()
    {
        InitializeComponent();
    }

    private async void BtnOk_Click(object? sender, RoutedEventArgs e)
    {
        var result = txtNewTypeProfile.Text?.Trim();

        if (string.IsNullOrWhiteSpace(result))
        {
            Close(null);
            return;
        }
        var existing = _dbContext.TypeProfiles.FirstOrDefault(tp => tp.Name == result);

        if (existing == null)
        {
            var newProfile = new TypeProfile { Name = result };
            _dbContext.TypeProfiles.Add(newProfile);
            await _dbContext.SaveChangesAsync();
            NewTypeProfileName = result;
        }
        else
        {
            NewTypeProfileName = existing.Name;
        }

        Close(NewTypeProfileName);
    }


    private void BtnCancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
