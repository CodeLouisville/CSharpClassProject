# Week 8

Week 8 is a bonus week. In week 8, we should be working in the Week8 branch. We start with the basic ASP.NET MVC Web Application created in weeks 1-7. Following these instructions, we will add server-side validation to ensure that our application is capturing meaningful and consistent data.

VII. Add validation rules to our models, and wire up our views and controller actions to test the rules and notify the user when the input does not conform to these rules.

	A. Make the necessary changes to make a person's first and last name required.

		1. Run the application. Edit a person by removing the person's last name. Save the change to return to the person list. Note that we now have a person with only a first name. Look at the data in the Person table. You should see that the person's last name is NULL. We did not get an error message because we did not specify that the person's last name was required in our data model. 

		2. IMPORTANT! Edit the person with the missing last name, setting it to any value. Otherwise, we will not be able to apply a rule that makes LastName required since the existing data does not conform to this rule.

		3. Add Required attributes to LastName and FirstName in our Person class in Models/Person.cs. You will need to add a using statement to reference System.ComponentModel.DataAnnotations. It should look like the following:

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

		4. In Package Manager Console, run the Add-Migration command. Name the migration PersonNameRequired. Inspect the newly created migration and note that the LastName and FirstName columns of the Person table will be altered to be non-nullable.
		
		5. In Package Manager Console, run the Update-Database command to apply the new migration.

		6. As you did in step #1, run the application, edit a person by removing the person's last name, and attempt to save your changes. The application should throw an exception. This exception is of type DbEntityValidationException. This means that Entity Framework determined that the data that we asked to save to our database does not meet the validation rules. This is good news, because it means that we do not have to worry about this nonconforming polluting our database. However, we still need to handle the error more elegantly and provide a better experience for our users.

		7. Modify the PersonViewModel class, adding the same Required attributes to LastName and FirstName. This time, we will include an error message to show to our user. It should resemble the following:

			using System.Collections.Generic;
			using System.ComponentModel;
			using System.ComponentModel.DataAnnotations;

			namespace Lunch.Models
			{
				public class PersonViewModel
				{
					public PersonViewModel()
					{
						FoodPreferences = new List<FoodPreferenceViewModel>();
					}

					public int? PersonId { get; set; }

					[DisplayName("Last Name")]
					[Required(ErrorMessage = "Last Name is required.")]
					public string LastName { get; set; }

					[DisplayName("First Name")]
					[Required(ErrorMessage = "First Name is required.")]
					public string FirstName { get; set; }

					[DisplayName("Name")]
					public string FullName => FirstName + " " + LastName;

					public List<FoodPreferenceViewModel> FoodPreferences { get; set; }
				}
			}

		8. Modify the PersonController class changing the EditPerson() action to check the state of the PersonViewModel that it receives from the browser. This is done by checking the ModelState.IsValid property. If ModelState.IsValid is false, we should return the user to the AddEditPerson view, passing the invalid model back to the view.

		    [HttpPost]
			public ActionResult EditPerson(PersonViewModel personViewModel)
			{
				if (!ModelState.IsValid)
				{
					return View("AddEditPerson", personViewModel);
				}

				using (var lunchContext = new LunchContext())
				{
					var person = lunchContext.People.SingleOrDefault(p => p.PersonId == personViewModel.PersonId);

					if (person != null)
					{
						person.LastName = personViewModel.LastName;
						person.FirstName = personViewModel.FirstName;
						lunchContext.SaveChanges();

						return RedirectToAction("Index");
					}
				}

				return new HttpNotFoundResult();
			}

		9. Modify the PersonController class changing the AddPerson() action to check the state of the PersonViewModel, in the same manner.

		    [HttpPost]
			public ActionResult AddPerson(PersonViewModel personViewModel)
			{
				if (!ModelState.IsValid)
				{
					return View("AddEditPerson", personViewModel);
				}

				using (var lunchContext = new LunchContext())
				{
					var person = new Person
					{
						LastName = personViewModel.LastName,
						FirstName = personViewModel.FirstName
					};

					lunchContext.People.Add(person);
					lunchContext.SaveChanges();
				}

				return RedirectToAction("Index");
			}

		10. Modify the AddEditPerson view, adding placeholders for the error messages, using the ValidationMessageFor() html helper.

			@model Lunch.Models.PersonViewModel
			@{
				var isEditMode = Model.PersonId != null;
				ViewBag.Title = isEditMode ? "Edit " + Model.FullName : "Add Person";
			}

			<h2>@Model.FullName</h2>

			<div class="row">
				<div class="col-sm-6 col-xs-10">
					@using (Html.BeginForm(isEditMode ? "EditPerson" : "AddPerson", "Person"))
					{
						@Html.HiddenFor(m => m.PersonId)
						<div class="form-group">
							@Html.LabelFor(m => m.LastName)
							@Html.TextBoxFor(m => m.LastName, new { @class = "form-control" })
							@Html.ValidationMessageFor(m => m.LastName, null, new { @class = "text-danger" })
						</div>
						<div class="form-group">
							@Html.LabelFor(m => m.FirstName)
							@Html.TextBoxFor(m => m.FirstName, new { @class = "form-control" })
							@Html.ValidationMessageFor(m => m.FirstName, null, new { @class = "text-danger" })
						</div>
						<button type="submit" class="btn btn-primary">@(isEditMode ? "Save" : "Add")</button>
						@Html.ActionLink("Cancel", "Index", null, new { @class = "btn btn-default" })
						@Html.ValidationSummary(null, new { @class = "alert-danger", style = "margin-top: 15px; padding: 10px 0;" });
					}
				</div>
			</div>

		11. Finally, run the application again. This time omit the last name and first name. Attempt to save your changes. You should see an error message under each input. You should also see a summary of all errors at the bottom of the view. We are now elegantly handling validation errors for adding and editing people.
			
	B. Add validation to our Restaurant model and view-model. Wire up the controller and view to elegantly handle invalid input.

		1. Modify the Restaurant class, adding a Required attribute on the Name property. Note that CuisineId has a data type of int. Because int is a value type in C#, it is inherently non-nullable already. If we had needed the cuisine to be optional, we would have implemented it as a nullable integer, by using the int? type. The class should now look like the following:

			using System.ComponentModel.DataAnnotations;

			namespace Lunch.Models
			{
				public class Restaurant
				{
					public int RestaurantId { get; set; }

					[Required]
					public string Name { get; set; }

					public int CuisineId { get; set; }

					public virtual Cuisine Cuisine { get; set; }
				}
			}

		2. In Package Manager Console, run the Add-Migration command. Name the migration RestaurantNameRequired. Inspect the newly created migration and note that the Name column of the Restaurant table will be altered to be non-nullable.
		
		3. In Package Manager Console, run the Update-Database command to apply the new migration.

		4. Modify the RestaurantViewModel, adding a Required attribute with an error message for the Name property.

			using System.ComponentModel.DataAnnotations;

			namespace Lunch.Models
			{
				public class RestaurantViewModel
				{
					public int? RestaurantId { get; set; }

					[Required(ErrorMessage = "Name is required.")]
					public string Name { get; set; }

					public CuisineViewModel Cuisine { get; set; }
				}
			}

		5. Modify the CuisineViewModel, adding a Required attribute with an error message for the CuisineId property.

			using System.ComponentModel.DataAnnotations;

			namespace Lunch.Models
			{
				public class CuisineViewModel
				{
					[Required(ErrorMessage = "Cuisine is required.")]
					public int? CuisineId { get; set; }

					public string Name { get; set; }
				}
			}

		6. Modify the AddRestaurant() action in the RestaurantController class, checking ModelState.IsValid() and returning the user to the view if the model is found to be invalid. It is necessary to re-populate the list of cuisines since we will have to bind it to the cuisine dropdown again.

			[HttpPost]
			public ActionResult AddRestaurant(RestaurantViewModel restaurantViewModel)
			{
				if (!ModelState.IsValid)
				{
					using (var lunchContext = new LunchContext())
					{
						ViewBag.Cuisines = lunchContext.Cuisines.Select(c => new SelectListItem
						{
							Value = c.CuisineId.ToString(),
							Text = c.Name
						}).ToList();

						return View("AddEditRestaurant", restaurantViewModel);
					}
				}

				using (var lunchContext = new LunchContext())
				{
					var restaurant = new Restaurant
					{
						Name = restaurantViewModel.Name,
						CuisineId = restaurantViewModel.Cuisine.CuisineId.Value
					};

					lunchContext.Restaurants.Add(restaurant);
					lunchContext.SaveChanges();
				}

				return RedirectToAction("Index");
			}

		7. Modify the EditRestaurant() action in the RestaurantController class, checking ModelState.IsValid() and returning the user to the view if the model is found to be invalid. It is necessary to re-populate the list of cuisines since we will have to bind it to the cuisine dropdown again.

		    [HttpPost]
			public ActionResult EditRestaurant(RestaurantViewModel restaurantViewModel)
			{
				if (!ModelState.IsValid)
				{
					using (var lunchContext = new LunchContext())
					{
						ViewBag.Cuisines = lunchContext.Cuisines.Select(c => new SelectListItem
						{
							Value = c.CuisineId.ToString(),
							Text = c.Name
						}).ToList();

						return View("AddEditRestaurant", restaurantViewModel);
					}
				}

				using (var lunchContext = new LunchContext())
				{
					var restaurant = lunchContext.Restaurants.SingleOrDefault(p => p.RestaurantId == restaurantViewModel.RestaurantId);

					if (restaurant != null)
					{
						restaurant.Name = restaurantViewModel.Name;
						restaurant.CuisineId = restaurantViewModel.Cuisine.CuisineId.Value;
						lunchContext.SaveChanges();

						return RedirectToAction("Index");
					}
				}

				return new HttpNotFoundResult();
			}

		8. Modify the AddEditRestaurant view, adding validation messages for the restaurant name and cuisine, and a validation summary. Also, modify the DropDownListFor() helper, adding an additional parameter to define the text of an empty item. This will require the user to select a cuisine, rather than defaulting to the first item in the list.

			@model Lunch.Models.RestaurantViewModel
			@{
				var isEditMode = Model.RestaurantId != null;
				ViewBag.Title = isEditMode ? "Edit " + Model.Name : "Add Restaurant";
			}

			<h2>@Model.Name</h2>

			<div class="row">
				<div class="col-sm-6 col-xs-10">
					@using (Html.BeginForm(isEditMode ? "EditRestaurant" : "AddRestaurant", "Restaurant"))
					{
						@Html.HiddenFor(m => m.RestaurantId)
						<div class="form-group">
							@Html.LabelFor(m => m.Name)
							@Html.TextBoxFor(m => m.Name, new { @class = "form-control" })
							@Html.ValidationMessageFor(m => m.Name, null, new { @class = "text-danger" })
						</div>
						<div class="form-group">
							<label for="Cuisine_CuisineId">Cuisine</label>
							@Html.DropDownListFor(m => m.Cuisine.CuisineId, ViewBag.Cuisines as IEnumerable<SelectListItem>, "Select a Cuisine...", new { @class = "form-control" })
							@Html.ValidationMessageFor(m => m.Cuisine.CuisineId, null, new { @class = "text-danger" })
						</div>
						<button type="submit" class="btn btn-primary">@(isEditMode ? "Save" : "Add")</button>
						@Html.ActionLink("Cancel", "Index", null, new { @class = "btn btn-default" })
						@Html.ValidationSummary(null, new { @class = "alert-danger", style = "margin-top: 15px; padding: 10px 0;" });
					}
				</div>
			</div>

		9. Run the application, and attempt to save a restaurant with no name and/or cuisine. You have now elegantly handled invalid user input.

	C. Although Required is the most common validatin rule, there are many more validation attributes including StringLength, RegularExpression, DataType, and more. We can also use the jQuery Validation Plugin to get richer client-side validation with little effort. It is worth the time to research all that ASP.NET MVC Validation can do. An hour spent implementing validation in your application can save tens, or even hundreds of hours spent cleaning invalid data on the back-end. The following concepts can be introduced:

		1. Using DataAnnotations on data models to change the way that EntityFramework creates columns in your database.
		2. Using DataAnnotations on view models, along with the ValidationMessageFor() and ValidationSummary() html helpers in your views.
		3. Evaluating ModelState.IsValid to run validation rules against your model before you persist data to the database.