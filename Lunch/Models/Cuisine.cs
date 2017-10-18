using System.Collections.Generic;

namespace Lunch.Models
{
    public class Cuisine
    {
        public Cuisine()
        {
            FoodPreferences = new HashSet<FoodPreference>();
        }

        public int CuisineId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<FoodPreference> FoodPreferences { get; set; }
    }
}