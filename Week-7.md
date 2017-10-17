# Week 7

In week 7, we should be working in the Week7 branch. We start with the basic ASP.NET MVC Web Application created in weeks 1-6. Following these instructions, we will add new entities to our data models and our database. We will then create a many-to-many relationship between two entities. 

VII. Add new entities to our data models, along with views, view models, etc. to manage these objects.

	A. Create a data model for food preferences, and add a migration to update our database with the new table.

		1. Right-click the Models folder, select Add, and click Class. Name the class FoodPreference. Replace all of the code in the FoodPreference.cs file with the code below. Note the use of the Key and Column properties. By using the Key property, we are telling Entity Framework that we want a column to be included in the table's primary key. We have to explicitly tell EF which column(s) make up our primary key any time we don't follow standard naming conventions. In this case, we have two columns that make up our primary key. This is known as a composite key, and is commonly found in tables that define many-to-many relationships. These tables are sometimes referred to as "junction" tables. In this case, we are storing more information in the junction table than just the relationship between the two entities (people and cuisines). We are storing the rating that describes how much a person likes a particular cuisine.

			using System.ComponentModel.DataAnnotations;
			using System.ComponentModel.DataAnnotations.Schema;

			namespace Lunch.Models
			{
				public class FoodPreference
				{
					[Key]
					[Column(Order = 1)]
					public int PersonId { get; set; }

					[Key]
					[Column(Order = 2)]
					public int CuisineId { get; set; }

					public int Rating { get; set; }

					public virtual Person Person { get; set; }

					public virtual Cuisine Cuisine { get; set; }
				}
			}

		2. Modify the Lunch.Models.Person class as shown below. Two changes were made. First, we added a navigation property named FoodPreferences. We marked it virtual, which allows Entity Framework to give it more features than it could otherwise. Second, we added a constructor method for the Person class, and we instantiated our FoodPreferences property with a new HashSet. We could have instantiated it with any ICollection type, such as List. It is common practice to use HashSet, because it is a more primitive ICollection, and takes up slightly less memory than List. We are instantiating the collection in the Person class's constructor so that we don't have to instantiate it every time we create a new Person. It also helps us avoid unwanted NullReferenceExceptions.

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

		3. Modify the Lunch.Models.Cuisine class as shown below. These are the same changes that we made to the person class. We have now demonstrated that a person has preferences for many cuisines, while a single cuisine is preferred (or not preferred) by many people.

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

					public virtual IEnumerable<FoodPreference> FoodPreferences { get; set; }
				}
			}

		4. Modify the LunchContext class, adding the following DbSet after the Restaurants property.

			public virtual DbSet<FoodPreference> FoodPreferences { get; set; }

		5. Modify our Seed() method on the Lunch.Migrations.Configuration class, adding the following after the line that calls the context.SaveChanges() method. Be sure to do it after SaveChanges(), or the people and cuisines that are referenced won't exist in the database yet.

			context.FoodPreferences.AddOrUpdate(
                fp => new { fp.PersonId, fp.CuisineId },
                new FoodPreference { PersonId = 1, CuisineId = 1, Rating = 5 },
                new FoodPreference { PersonId = 1, CuisineId = 2, Rating = 0 },
                new FoodPreference { PersonId = 1, CuisineId = 3, Rating = 3 },
                new FoodPreference { PersonId = 2, CuisineId = 1, Rating = 4 },
                new FoodPreference { PersonId = 2, CuisineId = 2, Rating = 1 },
                new FoodPreference { PersonId = 2, CuisineId = 3, Rating = 5 }
            );

		6. From the Tools menu, expand NuGet Package Manager and select Package Manager Console. From the PM> prompt, type Add-Migration and press <Enter>. When prompted for the Name, type FoodPreferences. Inspect the migration that was created, and note that when the CreateTable() method is instructing Entity Framework to create the FoodPreferences table, it is also telling it to add a primary key on both PersonId and CuisineId. It is also adding two foreign keys on these same fields, creating the many-to-many relationship between people and cuisines.

		7. From the PM> prompt, type Update-Database and press <Enter>. At this point, you can inspect the data in your database, and you should see the newly seeded data.

	B. Add the view model for FoodPreferences.

		1. Right-click the Models folder, select Add, and click Class. Name the class FoodPreferenceViewModel. Replace all of the code in the FoodPreferenceViewModel.cs file with the code below. Note that I am making the Rating property nullable. We will use this null value later to determine whether a rating has been made. This is necessary, since zero will be considered a valid rating. Also, note that I am not including a Person property. Typically, we will be interacting with FoodPreferences through the people that they belong to, making the food preference a child of a person.

			namespace Lunch.Models
			{
				public class FoodPreferenceViewModel
				{
					public CuisineViewModel Cuisine { get; set; }
					public int? Rating { get; set; }
				}
			}

		2. Modify the PersonViewModel, adding the FoodPreferenceViewModel as a property as shown below. Since we will currently only be adding preferences to people, and not directly to cuisines, we won't bother to add a navigation property to CuisineViewModel, although it would certainly be fine to do so.

			using System.Collections.Generic;
			using System.ComponentModel;

			namespace Lunch.Models
			{
				public class PersonViewModel
				{
					public int? PersonId { get; set; }

					[DisplayName("Last Name")]
					public string LastName { get; set; }

					[DisplayName("First Name")]
					public string FirstName { get; set; }

					[DisplayName("Name")]
					public string FullName => FirstName + " " + LastName;

					public List<FoodPreferenceViewModel> FoodPreferences { get; set; }
				}
			}

		3. Modify the PersonController, adding the following controller action. You will need to add "using System.Data.Entity;" to the top of the file for this code to work. By specifying that we want to "Include" the person's food preferences, we are employing a practice called eager loading. We know that we will be accessing the person's food preferences, so we can tell Entity Framekwork to include these in the query that it generates. This way, we don't have to make multiple round-trips to the database.

			public ActionResult ManageFoodPreferences(int id)
			{
				using (var lunchContext = new LunchContext())
				{
					var person = lunchContext.People.Include("FoodPreferences").SingleOrDefault(p => p.PersonId == id);
                
					if (person != null)
					{
						var allPossibleFoodPreferences = lunchContext.Cuisines.Select(c => new FoodPreferenceViewModel
						{
							Cuisine = new CuisineViewModel { CuisineId = c.CuisineId, Name = c.Name }
						}).ToList();

						ViewBag.AllPossibleFoodPreferences = allPossibleFoodPreferences;

						var personViewModel = new PersonViewModel
						{
							PersonId = person.PersonId,
							LastName = person.LastName,
							FirstName = person.FirstName,
							FoodPreferences = person.FoodPreferences.Select(fp => new FoodPreferenceViewModel
							{
								Cuisine = new CuisineViewModel { CuisineId = fp.CuisineId },
								Rating = fp.Rating
							}).ToList()
						};

						return View(personViewModel);
					}

					return new HttpNotFoundResult();
				}
			}

		4. Right-click the Views/Person folder, expand Add, and select View. Name the view ManageFoodPreferences and use the default _Layout view. Replace the code with the following:

			@model Lunch.Models.PersonViewModel

			@{
				ViewBag.Title = Model.FullName + "'s Food Preferences";
				var FoodPreferences = ViewBag.AllPossibleFoodPreferences as List<Lunch.Models.FoodPreferenceViewModel>;
			}

			<h2>@Model.FullName's Food Preferences</h2>

			@for (var i = 0; i < FoodPreferences.Count; i++)
			{
				var selectedValue = Model.FoodPreferences.FirstOrDefault(fp => fp.Cuisine.CuisineId == FoodPreferences[i].Cuisine.CuisineId)?.Rating;

				<div class="form-group">
					<div>
						<label>@FoodPreferences[i].Cuisine.Name</label>
					</div>
					<div>
						<input type="range" id="prefSlider@(i)" value="@(selectedValue == null ? -1 : selectedValue)" oninput="setPrefDescription(@(i))" min="@(selectedValue == null ? -1 : 0)" max="5" step="1" style="width: 300px;" />
						<span id="prefDescription@(i)"></span>
					</div>
				</div>
			}

			<script type="text/javascript">
				function setPrefDescription(index) {
					var prefDescriptionLabel = document.getElementById("prefDescription" + index);
					var prefSliderValue = parseInt(document.getElementById("prefSlider" + index).value);
					switch (prefSliderValue)
					{
						case -1:
							prefDescriptionLabel.innerHTML = "Not set";
							prefDescriptionLabel.style.color = "gray";
							break;
						case 0:
							prefDescriptionLabel.innerHTML = "Hate it";
							prefDescriptionLabel.style.color = "red";
							break;
						case 1:
							prefDescriptionLabel.innerHTML = "Don't care for it";
							prefDescriptionLabel.style.color = "orange";
							break;
						case 2:
							prefDescriptionLabel.innerHTML = "If I have to";
							prefDescriptionLabel.style.color = "orange";
							break;
						case 3:
							prefDescriptionLabel.innerHTML = "It's OK";
							prefDescriptionLabel.style.color = "green";
							break;
						case 4:
							prefDescriptionLabel.innerHTML = "Like it";
							prefDescriptionLabel.style.color = "green";
							break;
						case 5:
							prefDescriptionLabel.innerHTML = "Love it";
							prefDescriptionLabel.style.color = "green";
							break;
					}
				}

				@for (var i = 0; i < FoodPreferences.Count; i++)
				{
					<text>setPrefDescription(@(i));</text>
				}
			</script>

		5. TODO: Make this work!