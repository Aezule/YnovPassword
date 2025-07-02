using System.ComponentModel.DataAnnotations;

namespace YnovPassword.Modele
{
    public class Dictionnaire
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le mot est requis.")]
        public string Mot { get; set; } = string.Empty;

    }
}
