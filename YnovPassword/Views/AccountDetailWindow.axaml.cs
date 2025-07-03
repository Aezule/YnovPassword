using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using YnovPassword.Modele;

namespace YnovPassword.Views
{
    public partial class AccountDetailsWindow : Window
    {
        readonly private Account _account;
        readonly private PasswordContext _dbContext;
        readonly private bool _isNewAccount;
        private readonly int _currentUserId = (int)AuthStorage.LoadLogin()?.Id;

        public bool IsSaved { get; private set; }

        public AccountDetailsWindow()
        {
            InitializeComponent();
            _dbContext = new PasswordContext();
            _isNewAccount = true;
            _account = new Account();
            IsSaved = false;
            LoadTypeProfilesAsync();
        }

        public AccountDetailsWindow(Account account)
        {
            InitializeComponent();
            _dbContext = new PasswordContext();
            _account = account;
            _isNewAccount = false;
            IsSaved = false;

            // Remplir les champs avec les données du compte
            LoadAccountData();
        }
        private async Task LoadTypeProfilesAsync()
        {
            var profiles = await _dbContext.TypeProfiles.ToListAsync();
            cbTypeProfile.ItemsSource = profiles;

            var selected = profiles.FirstOrDefault(p => p.Id == _account.TypeProfileId);
            cbTypeProfile.SelectedItem = selected;
        }

        private void LoadAccountData()
        {

            txtName.Text = _account.Name;
            txtUrl.Text = _account.URL;
            txtLogin.Text = _account.Login;
            txtPassword.Text = PasswordHelper.Decrypt(_account.Password);
            LoadTypeProfilesAsync();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _account.Name = txtName.Text;

            if (cbTypeProfile.SelectedItem is TypeProfile selectedProfile)
            {
                var trackedProfile = await _dbContext.TypeProfiles.FindAsync(selectedProfile.Id);
                if (trackedProfile != null)
                {
                    _account.TypeProfile = trackedProfile;
                    _account.TypeProfileId = trackedProfile.Id;
                }
                else
                {
                    _account.TypeProfile = null!;
                    _account.TypeProfileId = 0;
                }
            }
            else
            {
                _account.TypeProfile = null!;
                _account.TypeProfileId = 0;
            }

            _account.URL = txtUrl.Text;
            _account.Login = txtLogin.Text;
            _account.Password = _account.Password = PasswordHelper.Encrypt(txtPassword.Text);

            _account.UtilisateurId = _currentUserId;

            if (_isNewAccount)
            {
                _dbContext.Accounts.Add(_account);
            }
            else
            {
                _dbContext.Accounts.Update(_account);
            }

            await _dbContext.SaveChangesAsync();
            IsSaved = true;
            Close();
        }






        private void OnGeneratePasswordClicked(object sender, RoutedEventArgs e)
        {
            string generatedPassword = GenerateStrongPassword(24);
            txtPassword.Text = generatedPassword;
        }

        private static string GenerateStrongPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+";
            var res = new char[length];
            var rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                res[i] = valid[rnd.Next(valid.Length)];
            }
            return new string(res);
        }

        private void OnEditPasswordClicked(object sender, RoutedEventArgs e)
        {
            txtPassword.IsReadOnly = false;
            txtPassword.Focus();
            txtPassword.SelectAll();
        }
        public async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new AccountDetailsWindow
            {
                Title = "Confirmation",
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 300,
                Height = 150,
                Owner = this
            };

            var confirmationText = new TextBlock
            {
                Text = "Voulez-vous vraiment supprimer ce compte ?",
                Margin = new Avalonia.Thickness(20, 20, 20, 10),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            };

            var buttonsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Margin = new Avalonia.Thickness(0, 0, 20, 20)
            };

            var yesButton = new Button { Content = "Oui", Margin = new Avalonia.Thickness(5) };
            var noButton = new Button { Content = "Non", Margin = new Avalonia.Thickness(5) };

            buttonsPanel.Children.Add(yesButton);
            buttonsPanel.Children.Add(noButton);

            var mainPanel = new StackPanel();
            mainPanel.Children.Add(confirmationText);
            mainPanel.Children.Add(buttonsPanel);

            messageBox.Content = mainPanel;

            yesButton.Click += async (s, args) =>
            {
                messageBox.Close();
                if (!_isNewAccount)
                {
                    _dbContext.Accounts.Remove(_account);
                    await _dbContext.SaveChangesAsync();
                }
                IsSaved = true;
                Close();
            };

            noButton.Click += (s, args) => messageBox.Close();

            await messageBox.ShowDialog(this);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
