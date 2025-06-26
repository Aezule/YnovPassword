using System;

namespace YnovPassword.Modele
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string TypeProfile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
