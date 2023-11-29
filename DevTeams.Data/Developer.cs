using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTeams.Data
{
    public class Developer
    {
        public Developer(){}
        public Developer(string firstName, string lastName, bool HasPluralSight)
        {
             FirstName = firstName;
             LastName = lastName;
            this.HasPluralSight = HasPluralSight;
        }
        // Unique identifier
        public int ID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { 
            get
            {
                return $"{FirstName} {LastName}";
            } 
        }
        public bool HasPluralSight { get; set; }

        public override string ToString()
        {
            string str = $"ID: {ID}\n" +
                         $"Name: {FullName}\n" +
                         $"has PluralSight: {HasPluralSight}\n";
            
            return str;
        }
    }
}