using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YnovPassword.Modele
{
    public static class AuthStorage
    {
        public class LoginInfo
        {
            public int Id { get; set; }
            public string Email { get; set; } = "";

            public bool IsAdmin { get; set; } = false;
        }

        private static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MyApp", "login.json");

        public static void SaveLogin(int Id, string email, bool isAdmin)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            var json = JsonSerializer.Serialize(new { Id = Id, Email = email, IsAdmin = isAdmin });
            File.WriteAllText(FilePath, json);
        }

        public static LoginInfo? LoadLogin()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var data = JsonSerializer.Deserialize<LoginInfo>(json);
                return data;
            }
            return null;
        }


        public static void ClearLogin()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }

}
