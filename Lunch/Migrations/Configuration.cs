using Lunch.Models;
using System.Data.Entity.Migrations;

namespace Lunch.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<LunchContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LunchContext context)
        {
            context.People.AddOrUpdate(
                p => p.PersonId,
                new Person { PersonId = 1, LastName = "Halpert", FirstName = "Jim" },
                new Person { PersonId = 2, LastName = "Beesly", FirstName = "Pam" },
                new Person { PersonId = 3, LastName = "Scott", FirstName = "Michael" },
                new Person { PersonId = 4, LastName = "Schrute", FirstName = "Dwight" },
                new Person { PersonId = 5, LastName = "Martin", FirstName = "Angela" },
                new Person { PersonId = 6, LastName = "Bernard", FirstName = "Andy" },
                new Person { PersonId = 7, LastName = "Malone", FirstName = "Kevin" },
                new Person { PersonId = 8, LastName = "Kapoor", FirstName = "Kelly" },
                new Person { PersonId = 9, LastName = "Palmer", FirstName = "Meredith" },
                new Person { PersonId = 10, LastName = "Flenderson", FirstName = "Toby" },
                new Person { PersonId = 11, LastName = "Hudson", FirstName = "Stanley" },
                new Person { PersonId = 12, LastName = "Bratton", FirstName = "Creed" },
                new Person { PersonId = 13, LastName = "Vance", FirstName = "Phyllis" },
                new Person { PersonId = 14, LastName = "Howard", FirstName = "Ryan" },
                new Person { PersonId = 15, LastName = "Philbin", FirstName = "Darryl" }
            );

            context.Cuisines.AddOrUpdate(
                c => c.CuisineId,
                new Cuisine { CuisineId = 1, Name = "American" },
                new Cuisine { CuisineId = 2, Name = "Chinese" },
                new Cuisine { CuisineId = 3, Name = "Italian" },
                new Cuisine { CuisineId = 4, Name = "Japanese" },
                new Cuisine { CuisineId = 5, Name = "Mexican" },
                new Cuisine { CuisineId = 6, Name = "Seafood" },
                new Cuisine { CuisineId = 7, Name = "Southwestern" },
                new Cuisine { CuisineId = 8, Name = "Vegetarian" }
            );

            context.SaveChanges();

            context.Restaurants.AddOrUpdate(
                r => r.RestaurantId,
                new Restaurant { RestaurantId = 1, Name = "Pizza by Alfredo", CuisineId = 3 },
                new Restaurant { RestaurantId = 2, Name = "Alfredo's Pizza Cafe", CuisineId = 3 },
                new Restaurant { RestaurantId = 3, Name = "Chili's", CuisineId = 7 },
                new Restaurant { RestaurantId = 4, Name = "Cooper's Seafood", CuisineId = 6 },
                new Restaurant { RestaurantId = 5, Name = "Poor Richard's Pub", CuisineId = 1 },
                new Restaurant { RestaurantId = 6, Name = "Benihana", CuisineId = 4 },
                new Restaurant { RestaurantId = 7, Name = "Auntie Anne's Pretzels", CuisineId = 1 },
                new Restaurant { RestaurantId = 8, Name = "Brunetti's Pizza", CuisineId = 3 },
                new Restaurant { RestaurantId = 9, Name = "Cugino's", CuisineId = 3 },
                new Restaurant { RestaurantId = 10, Name = "Dee Jay's", CuisineId = 1 },
                new Restaurant { RestaurantId = 11, Name = "Farley's Restaurant", CuisineId = 6 }
            );
        }
    }
}