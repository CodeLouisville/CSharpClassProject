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
        }
    }
}