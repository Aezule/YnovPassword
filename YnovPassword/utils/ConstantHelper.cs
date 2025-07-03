using System.Collections.Generic;

namespace YnovPassword.utils
{
    internal class ConstantHelper
    {
        public const string password = "YnovPassword";
        public const int id = 1;
        public const string Nom = "DeMadrido";
        public const string Prenom = "Roberto";
        public const string Email = "Robert";
        public const bool IsAdmin = true;



        public static readonly List<string> TypeProfileList = new List<string>
        {
            "Netflix",
            "Amazon",
            "Google",
            "Facebook",
            "Twitter",
            "LinkedIn",
            "Microsoft",
            "Apple",
            "Spotify",
            "Adobe"
        };

        public static readonly List<string> MotDictionnaire = new List<string>
        {
            "pomme",
            "banane",
            "cerise",
            "datte",
            "fraise",
            "raisin",
            "melon",
            "kiwi",
            "citron",
            "mangue",
            "abricot",
            "pêche",
            "prune",
            "poire",
            "orange",
            "framboise",
            "myrtille",
            "groseille",
            "pastèque",
            "pamplemousse",
            "noix",
            "amande",
            "châtaigne",
            "figue",
            "mûre"
        };

    }
}
