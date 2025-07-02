using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace YnovPassword.Modele
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string TypeProfile { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Foreign key to DBUtilisateur
        public int UtilisateurId { get; set; }

        // Navigation property
        [ForeignKey("UtilisateurId")]
        public DBUtilisateur Utilisateur { get; set; } = null!;


    }



}
