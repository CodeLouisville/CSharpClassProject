using System.Collections.Generic;

namespace Lunch.Models
{
    public class Person
    {
        public Person()
        {
            FoodPreferences = new HashSet<FoodPreference>();
        }

        public int PersonId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public virtual ICollection<FoodPreference> FoodPreferences { get; set; }
    }
}