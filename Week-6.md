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

		2. Right-click the Models folder, select Add, and click Class. Name the class Restaurant. Replace all of the code in the Restaurant.cs file with the code below. In a real-world application, we might want to be able to assign many cuisines to a single restaurant. However, to keep things simple, we will pretend that each restaurant has one and only one cuisine.

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

		4. Modify the Configuration class in our Migrations folder, adding the following code inside the Seed() method, just after the code that seeds people. Note that we are calling SaveChanges() after adding cuisines, before we add restaurants. This is necessary so that we can insure that the cuisines are added before we add restaurants that are associated with these cuisines.

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

		2. Right-click the Models folder, select Add, and click Class. Name the class RestaurantViewModel. Replace all of the code in the RestaurantViewModel.cs file with the code below. Note that, as with CuisineViewModel, the RestaurantViewModel's RestaurantId column is nullable. Also, note that the Cuisine property is a CuisineViewModel, and we are not including a CuisineId directly as a property of RestaurantViewModel. Hopefully, it will become apparent why that property is necessary on a data model, but not a view model. In the data model, it defines a column in the table that will be used int he foreign-key relationship. In the view model, it would just be redundant, since the CuisineId can be derived from the Cuisine property.

			namespace Lunch.Models
			{
				public class RestaurantViewModel
				{
					public int? RestaurantId { get; set; }
					public string Name { get; set; }
					public virtual Cuisine Cuisine { get; set; }
				}
			}

		3. Right-click the Models folder, select Add, and click Class. Name the class RestaurantListViewModel. Replace all of the code in the RestaurantListViewModel.cs file with the code below.

			using System.Collections.Generic;

			namespace Lunch.Models
			{
				public class RestaurantListViewModel
				{
					public List<RestaurantViewModel> Restaurants { get; set; }
					public int TotalRestaurants { get; set; }
				}
			}

	C. Wire-up a view and controller action that will display a list of restaurants.

		1. Right-click the Controllers folder, expand Add, and select Controller. Choose "MVC 5 Controller Empty" and press Add. Name the controller RestaurantController and press Add. Note that a folder named Restaurant was created in the Views folder.

		2. Add the following to the top of the RestaurantController class.

			using Lunch.Models;

		3. Replace the Index() action with the code below. This is almost the same as the Index() action in the PersonController. The biggest difference is that we have to create a Cuisine object as part of creating a Restaurant object.

			public ActionResult Index()
			{
				using (var lunchContext = new LunchContext())
				{
					var restaurantList = new RestaurantListViewModel
					{
						//Convert each Restaurant to a RestaurantViewModel
						Restaurants = lunchContext.Restaurants.Select(r => new RestaurantViewModel
						{
							RestaurantId = r.RestaurantId,
							Name = r.Name,
							Cuisine = new CuisineViewModel
							{
								CuisineId = r.CuisineId,
								Name = r.Name
							}
						}).ToList()
					};

					restaurantList.TotalRestaurants = restaurantList.Restaurants.Count;

					return View(restaurantList);
				}
			}

		4. Right click the Views/Restaurant folder, expand add, and select View. Name the view Index, select Empty (without model) for Template, and check the "Use a layout page" box, but do not specify a layout page. Click Add. Replace the contents of the new Index.cshtml file with the code below. This is almost exactly the same as the Views/Person/Index.cshtml view, with restaurants instead of people.

			@model Lunch.Models.RestaurantListViewModel

			@{
				ViewBag.Title = "Restaurants";
			}

			<h2>Restaurants</h2>

			<table class="table table-striped">
				<thead>
					<tr>
						<th>Name</th>
						<th></th>
					</tr>
				</thead>
				<tbody>
					@for (var i = 0; i < Model.Restaurants.Count; i++)
					{
						<tr>
							<td>@Model.Restaurants[i].Name</td>
							<td>
								@Html.ActionLink("Detail", "RestaurantDetail", new { id = Model.Restaurants[i].RestaurantId })
								| @Html.ActionLink("Edit", "RestaurantEdit", new { id = Model.Restaurants[i].RestaurantId })
								| <a data-toggle="modal" href="@("#deleteRestaurantModal" + i)">Delete</a>
								<div class="modal fade" id="@("deleteRestaurantModal" + i)" tabindex="-1" role="dialog">
									<div class="modal-dialog">
										<div class="modal-content">
											@using (Html.BeginForm("DeleteRestaurant", "Restaurant"))
											{
												<input type="hidden" value="@Model.Restaurants[i].RestaurantId" name="RestaurantId" />
												<div class="modal-header">
													<button type="button" class="close" data-dismiss="modal">&times;</button>
													<h4 class="modal-title">Delete @Model.Restaurants[i].Name</h4>
												</div>
												<div class="modal-body">
													Are you sure you want to delete @Model.Restaurants[i].Name?
												</div>
												<div class="modal-footer">
													<button type="submit" class="btn btn-primary">Yes</button>
													<button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
												</div>
											}
										</div>
									</div>
								</div>
							</td>
						</tr>
					}
				</tbody>
			</table>

			<div class="col-sm-12 form-group">
				<label>Total Restaurants:</label> @Model.TotalRestaurants
			</div>
			<div class="col-sm-12">
				@Html.ActionLink("Add Restaurant", "RestaurantAdd", null, new { @class = "btn btn-primary" })
			</div>

		5. Edit the Views/Shared/_Layout.cshtml file, adding the following code below the People menu item. At this point, you can run the application and use the menu to navigate to the restaurant list. However, all of the links are broken since we have not implemented controller actions for adding, editing, etc.

			<li>@Html.ActionLink("Restaurants", "Index", "Restaurant")</li>

	D. Wire-up a view and corresponding controller action that will allow viewing restaurant details.

		1. Add the following method after the Index() action in the RestaurantController class.

			public ActionResult RestaurantDetail(int id)
			{
				using (var lunchContext = new LunchContext())
				{
					var restaurant = lunchContext.Restaurants.SingleOrDefault(p => p.RestaurantId == id);
					if (restaurant != null)
					{
						var restaurantViewModel = new RestaurantViewModel
						{
							RestaurantId = restaurant.RestaurantId,
							Name = restaurant.Name,
							Cuisine = new CuisineViewModel
							{
								CuisineId = restaurant.CuisineId,
								Name = restaurant.Cuisine.Name
							}
						};

						return View(restaurantViewModel);
					}
				}

				return new HttpNotFoundResult();
			}

		2. Right click the Views/Restaurant folder, expand add, and select View. Name the view RestaurantDetail, select Empty (without model) for Template, and check the "Use a layout page" box, but do not specify a layout page. Click Add. Replace the contents of the new RestaurantDetail.cshtml file with the code below.

			@model Lunch.Models.RestaurantViewModel
			@{
				ViewBag.Title = "Restaurant Detail";
			}

			<h2>@Model.Name</h2>

			<div class="row">
				<div class="col-sm-12 form-group">
					@Html.LabelFor(m => m.Name)
					<br />@Html.DisplayFor(m => m.Name)
				</div>
				<div class="col-sm-12 form-group">
					@Html.LabelFor(m => m.Cuisine)
					<br />@Html.DisplayFor(m => m.Cuisine.Name)
				</div>
				<div class="col-sm-12">
					@Html.ActionLink("Done", "Index", null, new { @class = "btn btn-primary" })
				</div>
			</div>

	E. Wire-up a view and corresponding controller actions that will allow adding and editing restaurants.

		1. Add the following actions after the RestaurantDetail() action in the RestaurantController class. This is a lot of code, but it is almost identical to the code in the PersonController that we already created. You should notice two distinct differences. The RestaurantAdd() and RestaurantEdit() actions add a list of Cuisines converted to SelectListItems onto the ViewBag. This is important in the next step.

			public ActionResult RestaurantAdd()
			{
				using (var lunchContext = new LunchContext())
				{
					ViewBag.Cuisines = lunchContext.Cuisines.Select(c => new SelectListItem
					{
						Value = c.CuisineId.ToString(),
						Text = c.Name
					}).ToList();
				}

				var restaurantViewModel = new RestaurantViewModel();

				return View("AddEditRestaurant", restaurantViewModel);
			}

			[HttpPost]
			public ActionResult AddRestaurant(RestaurantViewModel restaurantViewModel)
			{
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

			public ActionResult RestaurantEdit(int id)
			{
				using (var lunchContext = new LunchContext())
				{
					ViewBag.Cuisines = lunchContext.Cuisines.Select(c => new SelectListItem
					{
						Value = c.CuisineId.ToString(),
						Text = c.Name
					}).ToList();

					var restaurant = lunchContext.Restaurants.SingleOrDefault(p => p.RestaurantId == id);
					if (restaurant != null)
					{
						var restaurantViewModel = new RestaurantViewModel
						{
							RestaurantId = restaurant.RestaurantId,
							Name = restaurant.Name,
							Cuisine = new CuisineViewModel
							{
								CuisineId = restaurant.CuisineId,
								Name = restaurant.Cuisine.Name
							}
						};

						return View("AddEditRestaurant", restaurantViewModel);
					}
				}

				return new HttpNotFoundResult();
			}

			[HttpPost]
			public ActionResult EditRestaurant(RestaurantViewModel restaurantViewModel)
			{
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

			[HttpPost]
			public ActionResult DeleteRestaurant(RestaurantViewModel restaurantViewModel)
			{
				using (var lunchContext = new LunchContext())
				{
					var restaurant = lunchContext.Restaurants.SingleOrDefault(p => p.RestaurantId == restaurantViewModel.RestaurantId);

					if (restaurant != null)
					{
						lunchContext.Restaurants.Remove(restaurant);
						lunchContext.SaveChanges();

						return RedirectToAction("Index");
					}
				}

				return new HttpNotFoundResult();
			}

		2. Right click the Views/Restaurant folder, expand add, and select View. Name the view AddEditRestaurant, select Empty (without model) for Template, and check the "Use a layout page" box, but do not specify a layout page. Click Add. Replace the contents of the new AddEditRestaurant.cshtml file with the code below. Note that we are using the ViewBag.Cuisines object that we added in the previous step to bind a list of cuisines to our dropdown list.

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
						</div>
						<div class="form-group">
							<label for="Cuisine_CuisineId">Cuisine</label>
							@Html.DropDownListFor(m => m.Cuisine.CuisineId, ViewBag.Cuisines as IEnumerable<SelectListItem>, new { @class = "form-control" })
						</div>
						<button type="submit" class="btn btn-primary">@(isEditMode ? "Save" : "Add")</button>
						@Html.ActionLink("Cancel", "Index", null, new { @class = "btn btn-default" })
					}
				</div>
			</div>

	F. Run the application and test all CRUD functions for restaurants. The following concepts can be introduced:

	    1. Adding a one-to-many relationship between two entities using Entity Framework.
	    2. Using the DropDownListFor() html helper method to bind a list of options for the user to select.
		3. Using ViewBag to pass information to the view that is not part of the model.