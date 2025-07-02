using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Linq;
using YnovPassword.Modele;
using YnovPassword.Views;
//using YnovPassword.ViewModels;

namespace YnovPassword;

public partial class App : Application
{
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

            var savedEmail = AuthStorage.LoadLogin();

            if (!string.IsNullOrEmpty(savedEmail))
            {
                var user = context.DBUtilisateur.FirstOrDefault(u => u.Email == savedEmail);
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
        }

        base.OnFrameworkInitializationCompleted();

        using (var dbContext = new PasswordContext())
        {
            dbContext.Database.Migrate();
        }
    }
}
