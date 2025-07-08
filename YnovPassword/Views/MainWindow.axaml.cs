using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using HashLib;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Digests;
using SHA3.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using YnovPassword.Modele;
using YnovPassword.utils;
using System.Diagnostics;

namespace YnovPassword.Views;

public partial class MainWindow : Window
{
    private PasswordContext _dbContext = new PasswordContext();
    private ObservableCollection<Account> _accounts = new ObservableCollection<Account>();
    private ObservableCollection<TypeProfile> _typeProfiles = new ObservableCollection<TypeProfile>();
    public ObservableCollection<string> Results { get; } = new ObservableCollection<string>();

    private readonly int _currentUserId = (int)AuthStorage.LoadLogin()?.Id;

    private readonly bool _isAdmin = AuthStorage.LoadLogin()?.IsAdmin ?? false;

    public MainWindow()
    {
        InitializeComponent();
        LoadDataFromDatabase(_currentUserId, _isAdmin);
        LoadTypeProfiles();

    }

    private void LoadTypeProfiles()
    {
        var profiles = _dbContext.TypeProfiles.ToList();
        _typeProfiles.Clear();

        _typeProfiles.Add(new TypeProfile { Id = 0, Name = "Tous" });

        foreach (var p in profiles)
            _typeProfiles.Add(p);

        cbTypeProfileFilter.ItemsSource = _typeProfiles;
        cbTypeProfileFilter.SelectedIndex = 0;
    }

    private void LoadDataFromDatabase(int currentUserId, bool isAdmin)
    {
        IQueryable<Account> query;

        if (isAdmin)
        {
            query = _dbContext.Accounts;
        }
        else
        {
            query = _dbContext.Accounts.Where(a => a.UtilisateurId == currentUserId);
        }

        var accountsFromDb = query.ToList();

        _accounts.Clear();
        foreach (var account in accountsFromDb)
        {
            _accounts.Add(account);
        }

        lstAccounts.ItemsSource = _accounts;

        txtCompteTotal.Text = _accounts.Count.ToString();
    }


    /// <summary>
    /// Fonction pour ouvrir la page d'information et de modification d'un compte lorsque l'utilisateur clique dessus.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Account_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is Account account)
        {
            var detailsWindow = new AccountDetailsWindow(account);
            detailsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            await detailsWindow.ShowDialog(this);

            if (detailsWindow.IsSaved)
            {
                LoadDataFromDatabase(_currentUserId, _isAdmin);
            }
        }
    }
    private async Task CheckPasswordsExposedAsync()
    {
        using var httpClient = new HttpClient();

        Results.Clear();

        foreach (var account in _accounts)
        {
            string decryptedPwd = PasswordHelper.Decrypt(account.Password);
            string hashPrefix = ComputeKeccak512HashPrefix(decryptedPwd);

            string url = ConstantHelper.pwndURL + hashPrefix.Substring(0, 10);

            try
            {
                var response = await httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Results.Add($"Compte '{account.Name}': Mot de passe NON exposé.");
                }
                else if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var jObj = JObject.Parse(json);
                    string count = jObj["SearchPassAnon"]?["count"]?.ToString() ?? "0";

                    Results.Add($"Compte '{account.Name}': Mot de passe exposé {count} fois !");
                }
                else
                {
                    Results.Add($"Compte '{account.Name}': Erreur API {response.StatusCode}.");
                }
            }
            catch (Exception ex)
            {
                PasswordContext.GetInExceptionErrorLog(ex);
                Results.Add($"Compte '{account.Name}': Exception {ex.Message}.");
            }
        }

        // Afficher tout dans la popup
        ShowMessageInPopup(string.Join(Environment.NewLine, Results));
    }



    private async void Btnpwnd_Click(object sender, RoutedEventArgs e)
    {
        await CheckPasswordsExposedAsync();
    }

    private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void CbTypeProfileFilter_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }


    private async void BtnAddTypeProfile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddTypeProfileWindow();
        var result = await dialog.ShowDialog<string?>(this);
        if (!string.IsNullOrEmpty(result))
        {
            LoadTypeProfiles();
            cbTypeProfileFilter.SelectedItem = _dbContext.TypeProfiles.FirstOrDefault(tp => tp.Name == result);
        }

    }



    private void ApplyFilters()
    {
        string searchText = txtRecherche.Text?.ToLower() ?? string.Empty;

        var selectedTypeProfile = cbTypeProfileFilter.SelectedItem as TypeProfile;
        int selectedTypeId = selectedTypeProfile?.Id ?? 0;

        IEnumerable<Account> filtered = _accounts;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            filtered = filtered.Where(a =>
                a.Name.ToLower().Contains(searchText) ||
                a.URL.ToLower().Contains(searchText) ||
                a.Login.ToLower().Contains(searchText));
        }

        if (selectedTypeId != 0) // Assuming Id=0 means "All"
        {
            filtered = filtered.Where(a => a.TypeProfileId == selectedTypeId);
        }

        var filteredList = filtered.ToList();

        lstAccounts.ItemsSource = filteredList;
        txtCompteTotal.Text = filteredList.Count.ToString();
    }


    private async void BtnAjouterCompte_Click(object sender, RoutedEventArgs e)
    {
        // Créer et afficher la fenêtre d'ajout d'un nouveau compte
        var newAccountWindow = new AccountDetailsWindow();
        newAccountWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        await newAccountWindow.ShowDialog(this);

        // Si un nouveau compte a été ajouté, recharger les données
        if (newAccountWindow.IsSaved)
        {
            LoadDataFromDatabase(_currentUserId, _isAdmin);
        }
    }


    private void ShowMessageInPopup(string message)
    {
        PopupMessage.Text = message;
        MessagePopup.IsOpen = true;
    }

    private void ClosePopup_Click(object? sender, RoutedEventArgs e)
    {
        MessagePopup.IsOpen = false;
    }

    private void BtnDeconnexion_Click(object? sender, RoutedEventArgs e)
    {
        AuthStorage.ClearLogin();

        var loginWindow = new LoginWindow(new PasswordContext());
        loginWindow.Show();
        this.Close();
    }


    private void PDF_Click(object? sender, RoutedEventArgs e)
    {
        var helpFile = Path.Combine(AppContext.BaseDirectory, "Help", "index.html");

        var psi = new ProcessStartInfo
        {
            FileName = helpFile,
            UseShellExecute = true
        };
        Process.Start(psi);


    }

    private async void BtnImport_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            SystemException ex = new SystemException("Erreur lors de l'ouverture de la page Windows.");
            PasswordContext.GetInExceptionErrorLog(ex);
            return;
        }

        var fileType = new FilePickerFileType("CSV Files")
        {
            Patterns = new[] { "*.csv" }
        };

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Importer un fichier CSV",
            AllowMultiple = false,
            FileTypeFilter = new[] { fileType }
        });

        if (files is null || files.Count == 0)
            return;

        var file = files[0];

        try
        {
            await using var stream = await file.OpenReadAsync();
            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);

            var mots = new List<Dictionnaire>();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                mots.Add(new Dictionnaire
                {
                    Mot = line.Trim()
                });
            }

            if (mots.Count > 0)
            {
                using var db = new PasswordContext();
                await db.AddRangeAsync(mots);
                await db.SaveChangesAsync();

                ShowMessageInPopup("Importation réussie !");

            }
            else
            {
                ShowMessageInPopup("Aucun mot trouvé dans le fichier.");
            }
        }
        catch (Exception ex)
        {
            PasswordContext.GetInExceptionErrorLog(ex);
        }
    }





    private async void ViewButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Account account)
        {
            var detailsWindow = new AccountDetailsWindow(account);
            detailsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            await detailsWindow.ShowDialog(this);

            if (detailsWindow.IsSaved)
            {
                LoadDataFromDatabase(_currentUserId, _isAdmin);
            }
        }
    }

    private async void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Account account)
        {
            var dialog = new ConfirmDialog($"Are you sure you want to delete '{account.Name}'?");
            var result = await dialog.ShowDialog<bool>(this);

            if (result)
            {
                // Récupère l'entité attachée au contexte via la clé primaire
                var accountToDelete = await _dbContext.Accounts.FindAsync(account.Id);

                if (accountToDelete != null)
                {
                    _dbContext.Accounts.Remove(accountToDelete);
                    await _dbContext.SaveChangesAsync();

                    LoadDataFromDatabase(_currentUserId, _isAdmin);
                }
            }
        }
    }


    private string ComputeKeccak512HashPrefix(string input)
    {
        var keccak = new KeccakDigest(512);
        var inputBytes = Encoding.UTF8.GetBytes(input);
        keccak.BlockUpdate(inputBytes, 0, inputBytes.Length);
        var result = new byte[64];
        keccak.DoFinal(result, 0);
        return BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();
    }

    private async void CopyButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Account account)
        {
            var passwordToCopy = PasswordHelper.Decrypt(account.Password);

            await this.Clipboard.SetTextAsync(passwordToCopy);
        }
    }




    protected override void OnClosed(System.EventArgs e)
    {
        _dbContext.Dispose();
        base.OnClosed(e);
    }
}
