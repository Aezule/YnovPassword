using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Linq;
using YnovPassword.Modele;
using YnovPassword.Views;
//using YnovPassword.ViewModels;

namespace YnovPassword;

public partial class App : Application
{
    private StyleInclude? _currentThemeStyle;


    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Batteries.Init();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var options = new DbContextOptionsBuilder<PasswordContext>()
                .UseSqlite("Data Source=DBYnovPassword.db")
                .Options;

            var context = new PasswordContext(options);

            var savedLogin = AuthStorage.LoadLogin();

            if (savedLogin != null && !string.IsNullOrEmpty(savedLogin.Email))
            {
                var user = context.DBUtilisateur.FirstOrDefault(u => u.Email == savedLogin.Email);
                if (user != null)
                {
                    desktop.MainWindow = new MainWindow();
                }
                else
                {
                    AuthStorage.ClearLogin();
                    desktop.MainWindow = new LoginWindow(context);
                }
            }
            else
            {
                desktop.MainWindow = new LoginWindow(context);
            }


            if (Application.Current is not null)
            {
                Application.Current.ActualThemeVariantChanged += (sender, e) =>
                {
                    var newTheme = Application.Current.ActualThemeVariant;
                    changeStyleApp(newTheme);
                };

                changeStyleApp(Application.Current.ActualThemeVariant);
            }
        }

        base.OnFrameworkInitializationCompleted();

        using var dbContext = new PasswordContext();
        {
            dbContext.Database.Migrate();
        }
    }

    private void changeStyleApp(ThemeVariant themeVariant)
    {
        if (_currentThemeStyle != null)
        {
            Styles.Remove(_currentThemeStyle);
        }

        var themePath = themeVariant == ThemeVariant.Dark
            ? "avares://YnovPassword/Styles/style.axaml"
            : "avares://YnovPassword/Styles/kawaii.axaml";

        _currentThemeStyle = new StyleInclude(new Uri("avares://YnovPassword/Styles"))
        {
            Source = new Uri(themePath)
        };

        Styles.Add(_currentThemeStyle);
    }
}
