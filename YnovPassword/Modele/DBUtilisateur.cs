using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnovPassword.Modele
{
    class DBUtilisateur
    {

        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public DBUtilisateur(string nom, string prenom, string email, string motDePasse, string role)
        {
            Nom = nom;
            Prenom = prenom;
            Email = email;
            MotDePasse = motDePasse;
        }

        public DBUtilisateur()
        {
            Nom = string.Empty;
            Prenom = string.Empty;
            Email = string.Empty;
            MotDePasse = string.Empty;
        }

        public override string ToString()
        {
            return $"{Nom} {Prenom} - {Email}";
        }


    }
}
