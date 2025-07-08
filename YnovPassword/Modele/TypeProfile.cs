using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnovPassword.Modele
{
    public class TypeProfile
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();



        public TypeProfile(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }

        public TypeProfile(string name)
        {
            Name = name;
        }

        public TypeProfile()
        {

        }
    }
}
