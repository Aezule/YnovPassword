
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using YnovPassword.utils;


namespace YnovPassword.Modele
{
    public class PasswordContext : DbContext
    {
        private static readonly string logFolder = "Logs";
        private static readonly string logFileName = "error_log.txt";
        private static readonly string logFilePath = Path.Combine(logFolder, logFileName);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=DBYnovPassword.db");

            }
        }

        public PasswordContext()
        {
        }

        // Constructeur avec options
        public PasswordContext(DbContextOptions<PasswordContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<DBUtilisateur> DBUtilisateur { get; set; } = null!;

        public DbSet<Dictionnaire> Dictionnaires { get; set; } = null!;

        public DbSet<TypeProfile> TypeProfiles { get; set; } = null!;


        public static void GetInExceptionErrorLog(Exception ex)
        {
            try
            {
                // Make sure the folder exists, if not, create it
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                string logEntry = $"[{DateTime.Now}] Exception: {ex.Message}{Environment.NewLine}" +
                                  $"Stack Trace: {ex.StackTrace}{Environment.NewLine}" +
                                  $"--------------------------------------------------{Environment.NewLine}";

                // Append the log entry to the file inside the folder
                File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception logEx)
            {
                Console.WriteLine("Logging failed:");
                Console.WriteLine(logEx.Message);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            base.OnModelCreating(modelBuilder);


            string hashedPassword = PasswordHelper.ComputeSha256Hash(ConstantHelper.password);


            modelBuilder.Entity<DBUtilisateur>().HasData(
                new DBUtilisateur
                {
                    Id = ConstantHelper.id,
                    Nom = ConstantHelper.Nom,
                    Prenom = ConstantHelper.Prenom,
                    Email = ConstantHelper.Email,
                    MotDePasse = hashedPassword,
                    IsAdmin = true
                }
            );

            for (int i = 1; i < ConstantHelper.TypeProfileList.Count; i++)
            {
                modelBuilder.Entity<TypeProfile>().HasData(
                    new TypeProfile { Id = i + 1, Name = ConstantHelper.TypeProfileList[i] }
                );
            }

            for (int i = 1; i < ConstantHelper.MotDictionnaire.Count; i++)
            {
                modelBuilder.Entity<Dictionnaire>().HasData(
                    new Dictionnaire { Id = i + 1, Mot = ConstantHelper.MotDictionnaire[i] }
                );
            }



        }




    }
}
