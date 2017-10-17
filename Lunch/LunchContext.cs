using Lunch.Models;
using System.Data.Entity;

namespace Lunch
{
    public class LunchContext : DbContext
    {
        public LunchContext() : base("name=Lunch") { }

        public virtual DbSet<Person> People { get; set; }

        public virtual DbSet<Cuisine> Cuisines { get; set; }

        public virtual DbSet<Restaurant> Restaurants { get; set; }

        public virtual DbSet<FoodPreference> FoodPreferences { get; set; }
    }
}