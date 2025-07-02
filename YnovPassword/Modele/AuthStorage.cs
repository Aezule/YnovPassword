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
        private static string FilePath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MyApp", "login.json");

        public static void SaveLogin(string email)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            var json = JsonSerializer.Serialize(new { Email = email });
            File.WriteAllText(FilePath, json);
        }

        public static string? LoadLogin()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                return data?["Email"];
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
