# Week 6

In week 6, we should be working in the Week6 branch. We start with the basic ASP.NET MVC Web Application created in weeks 1-5. Following these instructions, we will add new entities to our data models and our database. We will then create a one-to-many relationship between two entities. 

V. Add new entities to our data models, along with views, view models, etc. to manage these objects.

	A. Create new data models for cuisines and restaurants, and add a migration to update our database with the new tables.

		1. Right-click the Models folder, select Add, and click Class. Name the class Cuisine. Replace all of the code in the Cuisine.cs file with the code below.

			namespace Lunch.Models
			{
				public class Cuisine
				{
					public int CuisineId { get; set; }
					public string Name { get; set; }
				}
			}

		2. Right-click the Models folder, select Add, and click Class. Name the class Restaurant. Replace all of the code in the Restaurant.cs file with the code below. In a real-world application, we might want to be able to assign many cuisines to a single restaurant, but to keep things simple, we will pretend that each restaurant has one and only one cuisine.

			namespace Lunch.Models
			{
				public class Restaurant
				{
					public int RestaurantId { get; set; }
					public string Name { get; set; }
					public int CuisineId { get; set; }
					public virtual Cuisine Cuisine { get; set; }
				}
			}

		3. Modify the LunchContext class, adding the following code at the end of the class, just after the line where we added the People DbSet property.

			public virtual DbSet<Cuisine> Cuisines { get; set; }
			public virtual DbSet<Restaurant> Restaurants { get; set; }

		4. Modify the Configuration class in our Migrations folder, adding the following code inside the Seed() method, just after the code that seeds people. Note that we are calling SaveChanges() after adding cuisines, before we add restaurants. This is necessary so that we can insure that the cuisines are added before we add restaraunts that are associated with these cuisines.

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
                new Restaurant { RestaurantId = 1, Name = "Alfredo's Pizza Cafe", CuisineId = 3 },
                new Restaurant { RestaurantId = 1, Name = "Chili's", CuisineId = 7 },
                new Restaurant { RestaurantId = 1, Name = "Cooper's Seafood", CuisineId = 6 },
                new Restaurant { RestaurantId = 1, Name = "Poor Richard's Pub", CuisineId = 1 },
                new Restaurant { RestaurantId = 1, Name = "Benihana", CuisineId = 4 },
                new Restaurant { RestaurantId = 1, Name = "Auntie Anne's Pretzels", CuisineId = 1 },
                new Restaurant { RestaurantId = 1, Name = "Brunetti's Pizza", CuisineId = 3 },
                new Restaurant { RestaurantId = 1, Name = "Cugino's", CuisineId = 3 },
                new Restaurant { RestaurantId = 1, Name = "Dee Jay's", CuisineId = 1 },
                new Restaurant { RestaurantId = 1, Name = "Farley's Restaurant", CuisineId = 6 }
            );

		5. From the Tools menu, expand NuGet Package Manager and select Package Manager Console. From the PM> prompt, type Add-Migration and press <Enter>. When prompted for the Name, type CuisinesAndRestaurants. Inspect the migration that was created, and note that when the CreateTable() method is instructing Entity Framework to create the Restaurants table, it is also telling it to add a foreign key on the CuisineId column, creating a relationship to the Cuisine's table. This was all accomplished by following naming conventions, although we could have done this explicitly if we really wanted to break from those conventions.

		6. From the PM> prompt, type Update-Database and press <Enter>. At this point, you can inspect the data in your database, and you should see the newly seeded data.

	B. Add view models for restaurants and cuisines.

		1. Right-click the Models folder, select Add, and click Class. Name the class CuisineViewModel. Replace all of the code in the CuisineViewModel.cs file with the code below. Note that the only difference between the Cuisine class and the CuisineViewModel class is that CuisineId is nullable (int?).

			namespace Lunch.Models
			{
				public class CuisineViewModel
				{
					public int? CuisineId { get; set; }
					public string Name { get; set; }
				}
			}

		2. Right-click the Models folder, select Add, and click Class. Name the class Restaurant. Replace all of the code in the RestaurantViewModel.cs file with the code below. Note that, as with CuisineViewModel, the RestaurantViewModel's RestaurantId column is nullable. Also, note that the Cuisine property is a CuisineViewModel, and we aren't including a CuisineId directly as a property of RestarauntViewModel. Hopefully, it will become apparent why that property is necessary on a data model, but not a view model. In the data model, it defines a column in the table that will be used int he foreign-key relationship. In the view model, it would just be redundant, since the CuisineId can be derived from the Cuisine property.

			namespace Lunch.Models
			{
				public class Restaurant
				{
					public int? RestaurantId { get; set; }
					public string Name { get; set; }
					public virtual Cuisine Cuisine { get; set; }
				}
			}

	C. Wire-up a view and controller action that will display a list of restaurants.

		1. TODO: Complete part C instructions.

	D. Wire-up a view and corresponding controller actions that will allow adding and editing restaurants.

		1. TODO: Complete part D instructions.



