using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YnovPassword.Modele
{
    public class DBUtilisateur
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }

        // Navigation property (one-to-many)
        public ICollection<Account> Accounts { get; set; } = new List<Account>();

        public DBUtilisateur(int id, string nom, string prenom, string email, string motDePasse)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = motDePasse;
        }

        public DBUtilisateur()
        {
            Id = 0;
            Nom = string.Empty;
            Prenom = string.Empty;
            Email = string.Empty;
            MotDePasse = string.Empty;
        }
        public DBUtilisateur(string nom, string prenom, string email, string motDePasse)
        {
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = motDePasse;
        }

        public override string ToString()
        {
            return $"{Nom} {Prenom} - {Email}";
        }



    }
}
