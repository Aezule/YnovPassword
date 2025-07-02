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

    public MainWindow()
    {
        InitializeComponent();
        //LoadDataFromDatabase();

    }



    private void LoadDataFromDatabase()
    {
        // Charger les comptes depuis la base de données
        var accountsFromDb = _dbContext.Accounts.ToList();

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
                LoadDataFromDatabase();
            }
        }
    }

    /// <summary>
    /// Fonction pour rechercher des comptes en fonction du texte saisi dans le champ de recherche.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TxtRecherche_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Filtrer les comptes en fonction du texte de recherche
        string searchText = txtRecherche.Text?.ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            // Si la recherche est vide, afficher tous les comptes
            lstAccounts.ItemsSource = _accounts;
            txtCompteTotal.Text = _accounts.Count.ToString();
            return;
        }

        // Filtrer les comptes
        var filteredAccounts = _accounts.Where(a =>
            a.Name.ToLower().Contains(searchText) ||
            a.URL.ToLower().Contains(searchText) ||
            a.TypeProfile.ToLower().Contains(searchText) ||
            a.Login.ToLower().Contains(searchText)
        ).ToList();

        lstAccounts.ItemsSource = filteredAccounts;
        txtCompteTotal.Text = filteredAccounts.Count.ToString();
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
            LoadDataFromDatabase();
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
                LoadDataFromDatabase();
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
                LoadDataFromDatabase();
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
