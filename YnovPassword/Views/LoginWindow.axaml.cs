using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using YnovPassword.Modele;
using YnovPassword.Views;

namespace YnovPassword;

public partial class LoginWindow : Window
{

    private readonly PasswordContext _context;
    public LoginWindow(PasswordContext context)
    {
        _context = context;
        InitializeComponent();

        btnLogin.Click += LoginClick;
        btnCancel.Click += BtnCancel_Click;
    }

    private async void LoginClick(object? sender, RoutedEventArgs e)
    {
        var username = txtUsername.Text ?? "";
        var password = txtPassword.Text ?? "";

        string hashedPassword = PasswordHelper.ComputeSha256Hash(password);

        var user = await _context.DBUtilisateur
            .FirstOrDefaultAsync(u => u.Email == username && u.MotDePasse == hashedPassword);

        if (user != null)
        {

            AuthStorage.SaveLogin(user.Email);
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        else
        {
            await ShowMessage("Identifiants incorrects.");
        }
    }

    private void BtnCancel_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private async Task ShowMessage(string message)
    {
        var dialog = new Window
        {
            Width = 300,
            Height = 100,
            Title = "Erreur",
            Content = new TextBlock
            {
                Text = message,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            }
        };

        await dialog.ShowDialog(this);
    }

}
