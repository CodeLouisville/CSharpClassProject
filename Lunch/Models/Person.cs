using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lunch.Models
{
    public class Person
    {
        public Person()
        {
            FoodPreferences = new HashSet<FoodPreference>();
        }

        public int PersonId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public virtual ICollection<FoodPreference> FoodPreferences { get; set; }
    }
}