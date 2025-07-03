using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using YnovPassword.Modele;



namespace YnovPassword.Views;

public partial class MainWindow : Window
{
    private PasswordContext _dbContext = new PasswordContext();
    private ObservableCollection<Account> _accounts = new ObservableCollection<Account>();
    private ObservableCollection<TypeProfile> _typeProfiles = new ObservableCollection<TypeProfile>();


    private readonly int _currentUserId = (int)AuthStorage.LoadLogin()?.Id;

    public MainWindow()
    {
        InitializeComponent();
        LoadDataFromDatabase(_currentUserId);
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

    private void LoadDataFromDatabase(int currentUserId)
    {
        var accountsFromDb = _dbContext.Accounts
                                      .Where(a => a.UtilisateurId == currentUserId)
                                      .ToList();

        // Mettre à jour la collection
        _accounts.Clear();
        foreach (var account in accountsFromDb)
        {
            _accounts.Add(account);
        }

        // Assigner la collection au ItemsSource de l'ItemsControl
        lstAccounts.ItemsSource = _accounts;

        // Mettre à jour le compteur
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

            // Si des modifications ont été apportées, recharger les données
            if (detailsWindow.IsSaved)
            {
                LoadDataFromDatabase(_currentUserId);
            }
        }
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
            LoadDataFromDatabase(_currentUserId);
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
        // do later
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
                LoadDataFromDatabase(_currentUserId);
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
                _dbContext.Accounts.Remove(account);
                await _dbContext.SaveChangesAsync();
                LoadDataFromDatabase(_currentUserId);
            }

        }
    }

    private async void CopyButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Account account)
        {
            var passwordToCopy = account.Password;

            await this.Clipboard.SetTextAsync(passwordToCopy);
        }
    }




    protected override void OnClosed(System.EventArgs e)
    {
        _dbContext.Dispose();
        base.OnClosed(e);
    }
}
