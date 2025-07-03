using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using System.Linq;
using YnovPassword.Modele;
using YnovPassword.Views;

namespace YnovPassword;

public partial class SignIn : Window
{
    private readonly PasswordContext _context;

    public SignIn(PasswordContext context)
    {
        InitializeComponent();
        _context = context;
    }

    private async void btnSignIn_Click(object? sender, RoutedEventArgs e)
    {
        txtErrorMessageEmail.Text = "";
        txtErrorMessagePassword.Text = "";
        txtErrorMessageConfirmPassword.Text = "";
        txtErrorMessageAll.Text = "";

        var email = txtEmail.Text?.Trim() ?? string.Empty;
        var password = txtPassword.Text?.Trim() ?? string.Empty;
        var confirmPassword = txtConfirmPassword.Text?.Trim() ?? string.Empty;
        var nom = txtNom.Text?.Trim() ?? string.Empty;
        var prenom = txtPrenom.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom))
        {
            txtErrorMessageAll.Text = "Tous les champs sont obligatoires.";
            return;
        }
        else if (password != confirmPassword)
        {
            txtErrorMessagePassword.Text = "Les mots de passe ne correspondent pas.";
            txtErrorMessageConfirmPassword.Text = "Les mots de passe ne correspondent pas.";
            return;
        }
        else
        {
            bool isUsed = (from u in _context.DBUtilisateur
                           where u.Email == email
                           select u).Any();

            if (isUsed)
            {
                txtErrorMessageEmail.Text = "L'email est déjà utilisé.";
                return;
            }

            var passwordEncrypted = PasswordHelper.ComputeSha256Hash(password);
            var newUser = new DBUtilisateur(nom, prenom, email, passwordEncrypted);
            _context.DBUtilisateur.Add(newUser);
            await _context.SaveChangesAsync();


            var mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }
    }


    private void btnCancel_Click(object? sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow(_context);
        loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        loginWindow.Show();
        Close();
    }


}
