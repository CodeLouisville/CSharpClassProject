# Week 4

In week 4, we should be working in the Week4 branch. We start with the basic ASP.NET MVC Web Application created in weeks 1-3. Following these instructions, we will modify the application to add the Create and Read functionality that make up the "C" and "R" in CRUD (Create/Read/Update/Delete). In the end, the user will be able to add people to an in-memory data store.

III. Implement CRUD (Create/Read/Update/Delete) Operations

	A. Create a Person class. Later in this project, it will be used to represent a person in our database. This is an example of a data model. It is tempting to name it PersonDataModel to distinguish it from our PersonViewModel class. However, when we get to the part where we add Entity Framework to our project, our tables will be named based on the names we give to our data model classes. In order to avoid breaking from this convention, we will just name the class Person.

		1. Right-click the Models folder, select Add, and click Class. Name the class Person. Replace all of the code in the Person.cs file with the following:

			namespace Lunch.Models
			{
				public class Person
				{
					public int PersonId { get; set; }
					public string LastName { get; set; }
					public string FirstName { get; set; }
				}
			}

		2. Note that there are two distinct differences between the Person class and the PersonViewModel class.
		
			a. The data type of PersonId, int, is not followed by a question mark (int?). The question mark is a way to make a value type nullable. Since the Person class represents an instance of a Person in our database, we do not expect this field to ever be empty. However, a PersonViewModel might be used for adding a new person to our database, one who has not yet been assigned a PersonId.
			b. Unlike PersonViewModel, the Person class does not have a property named FullName. That is because FullName exists only for displaying a person. It is calculated based on FirstName and LastName, and should not be stored in a database.

	B. Modify the PersonController, and create a view to handle **Read** operations.

		1. Replace all of the code in the PersonController with the code below. This moves our initial list of people out of the Index action and into a static variable. In addition, instead of being a List<PersonViewModel>, it is now a List<Person> to better resemble what we will get when we query the database to retrieve people. Don't worry if this is confusing. Later, we will completely replace this static variable. We are only doing this to postpone introducing the concepts around using a database, so that we can focus on the concepts around MVC and HTTP.

			using Lunch.Models;
			using System.Collections.Generic;
			using System.Linq;
			using System.Web.Mvc;

			namespace Lunch.Controllers
			{
				public class PersonController : Controller
				{
					public static List<Person> People = new List<Person>
					{
						new Person { PersonId = 1, LastName = "Gibbons", FirstName = "Peter" },
						new Person { PersonId = 2, LastName = "Bolton", FirstName = "Michael" },
						new Person { PersonId = 3, LastName = "Nagheenanajar", FirstName = "Samir"},
						new Person { PersonId = 4, LastName = "Smykowski", FirstName = "Tom" },
						new Person { PersonId = 5, LastName = "Waddams", FirstName = "Milton" },
						new Person { PersonId = 6, LastName = "Lumbergh", FirstName = "Bill" }
					};

					public ActionResult Index()
					{
						var personList = new PersonListViewModel
						{
							//Convert each Person to a PersonViewModel
							People = People.Select(p => new PersonViewModel
							{
								PersonId = p.PersonId,
								LastName = p.LastName,
								FirstName = p.FirstName
							}).ToList()
						};

						personList.TotalPeople = personList.People.Count;

						return View(personList);
					}
				}
			}

		2. Add a new controller action to handle retrieving a single person. Add the following code after the Index action in the PersonController:

			public ActionResult PersonDetail(int id)
			{
				var person = People.SingleOrDefault(p => p.PersonId == id);
				if (person != null)
				{
					var personViewModel = new PersonViewModel
					{
						PersonId = person.PersonId,
						LastName = person.LastName,
						FirstName = person.FirstName
					};

					return View(personViewModel);
				}

				return new HttpNotFoundResult();
			}

		3. Note that the above action will return the result of a call to the View() method. This is going to trigger ASP.NET MVC to look for a view in the Views/Person folder named PersonDetail.cshtml. We need to create that view, or this will not work. Right-click the Views/Person folder, select Add, and click View. Name the view PersonDetail, select the "Empty (without model)" template, check the box labeled "Use a layout page", and click Add. Then, modify the view with the following code:

			@model Lunch.Models.PersonViewModel
			@{
				ViewBag.Title = "Person Detail";
			}

			<h2>@Model.FullName</h2>

			<div class="row">
				<div class="col-sm-12 form-group">
					@Html.LabelFor(m => m.LastName)
					<br />@Html.DisplayFor(m => m.LastName)
				</div>
				<div class="col-sm-12 form-group">
					@Html.LabelFor(m => m.FirstName)
					<br />@Html.DisplayFor(m => m.FirstName)
				</div>
				<div class="col-sm-12">
					@Html.ActionLink("Done", "Index", null, new { @class = "btn btn-primary" })
				</div>
			</div>

		4. We now have a view that can handle displaying the details of a single person, but we need a way to get our users to navigate to that view. Since the new action is in our Person controller, we know that we need a route that begins with /Person. In addition, since we named the action PersonDetail, we can infer that the route will start with /Person/PersonDetail. Since the only parameter to the PersonDetail() action is named "id", it matches our default route template in our RouteConfig class: "{controller}/{action}/{id}". If we want to view the person with personId 3, we could browse directly to /Person/PersonDetail/3 to view that person. Next, edit the table in the Person List view (i.e. Views/Person/Index) to look like the following:

			<table class="table table-striped">
				<thead>
					<tr>
						<th>Name</th>
						<th></th>
					</tr>
				</thead>
				<tbody>
					@for (var i = 0; i < Model.People.Count; i++)
					{
						<tr>
							<td>@Model.People[i].FullName</td>
							<td>@Html.ActionLink("Detail", "PersonDetail")</td>
						</tr>
					}
				</tbody>
			</table>

		5. Run the application. (Note: If you have Visual Studio open to the PersonDetail.cshtml view when you start the application, it may throw an error. If so, just manually navigate to Person/Index by removing PersonDetail from the location in your browser. That route is useless wthout including an id.) You should now be able to click the Edit link next to any person in the table and navigate to the PersonDetail view. If you hover over the Edit link, you should be able to inspect the URL and notice that the Html.ActionLink helper constructed the proper URL to route us to the PersonDetail action. You can click the Done button to return to the Person List.

		6. Note that the labels for LastName and FirstName match the names of the properties of our PersonViewModel class. This is not ideal. To fix this, edit the PersonViewModel class, adding DisplayName attributes that will allow our Html.LabelFor() methods to use friendlier labels. Don't forget to add `using System.ComponentModel;`.

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
				}
			}
			
	C. Modify the PersonController, and create a view to handle **Create** operations.

		1. Add a new controller action to handle loading the view that will contain a form for editing a person. Add the code below after the PersonDetail action in the PersonController. Note that we are creating an empty PersonViewModel to pass to our view. This will allow us to access our model and use the Html helpers to create our interface, even though we are not interested in the actual values of the properties associated with this person. Also, note that we are explicitly passing a view name (i.e. "AddEditPerson" to the View() method. This is because we are going to create a view that will be used for both adding a new person, and editing an existing one. Since we will be breaking from the standard naming convention, we need to be explicit about the view that we want to load. MVC will not be able to figure it out for us.

			public ActionResult NewPerson()
			{
				var personViewModel = new PersonViewModel();

				return View("AddEditPerson", personViewModel);
			}

		2. Since we explicitly told the View() method to look for a view named "AddEditPerson", MVC will try to load Views/Person/AddEditPerson.cshtml. Next, we need to create a view at that location. Right-click on the Views/Person folder, select Add, and click View. Name the view AddEditPerson, and create the view exactly as we did in previous steps. Replace the code in the view with the following:

			@model Lunch.Models.PersonViewModel
			@{
				ViewBag.Title = "Add Person";
			}

			<h2>@Model.FullName</h2>

			<div class="row">
				<div class="col-sm-6 col-xs-10">
					@using (Html.BeginForm("AddPerson", "Person"))
					{
						@Html.HiddenFor(m => m.PersonId);
						<div class="form-group">
							@Html.LabelFor(m => m.LastName)
							@Html.TextBoxFor(m => m.LastName, new { @class = "form-control" })
						</div>
						<div class="form-group">
							@Html.LabelFor(m => m.FirstName)
							@Html.TextBoxFor(m => m.FirstName, new { @class = "form-control" })
						</div>
						<button type="submit" class="btn btn-primary">Add</button>
						@Html.ActionLink("Cancel", "Index", null, new { @class = "btn btn-default" })
					}
				</div>
			</div>

		3. Next, edit the Person List view (i.e. Views/Person/Index), replacing the `Total People: @Model.TotalPeople` with the following code:

			<div class="col-sm-12 form-group">
				<label>Total People:</label> @Model.TotalPeople
			</div>
			<div class="col-sm-12">
				@Html.ActionLink("Add Person", "NewPerson", null, new { @class = "btn btn-primary" })
			</div>

		4. Add another controller action to handle receiving the new person, and persisting the data to our in-memory collection of people. Add the following action below the NewPerson action in the PersonController:

			[HttpPost]
			public ActionResult AddPerson(PersonViewModel personViewModel)
			{
				var nextPersonId = People.Max(p => p.PersonId) + 1;

				var person = new Person
				{
					PersonId = nextPersonId,
					LastName = personViewModel.LastName,
					FirstName = personViewModel.FirstName
				};

				People.Add(person);

				return RedirectToAction("Index");
			}