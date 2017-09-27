# Week 4

In week 4, we should be working in the Week4 branch. We start with the basic ASP.NET MVC Web Application created in weeks 1-3. Following these instructions, we will modify the application to CRUD (Create/Read/Update/Delete) operations. In the end, the user will be able to manage people in an in-memory data store.

III. Implement CRUD (Create/Read/Update/Delete) Operations

	A. Create a Person class. Later in this project, it will be used to represent a person in our database. This is an example of a data model. It is tempting to name it PersonDataModel to distinguish it from our PersonViewModel class. However, later in the project when we add Entity Framework, our tables will be named based on the names we give to our data model classes. In order to avoid breaking from this convention, we will just name the class Person.

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
		
			a. The data type of PersonId, int, is not followed by a question mark (int?). The question mark is a way to allow a value type to be nullable. Since the Person class represents an instance of a person in our database, we do not expect this field to ever be empty. In contrast, a PersonViewModel might be used for adding a new person who has not yet been assigned a PersonId.
			b. Unlike PersonViewModel, the Person class does not have a property named FullName. That is because FullName exists only for display purposes. It is a calculated, read-only property, and should not be stored in a database.

	B. Modify the PersonController, and create a view to handle **Read** operations. Technically, we already implemented a **Read** operation when we created the Person List. We will be implementing another **Read** operation by creating a view to display a single person.

		1. Replace all of the code in the PersonController with the code below. This moves our initial list of people out of the Index action and into a static variable. In addition, instead of being a List<PersonViewModel>, it is now a List<Person> to better resemble they type of data we will get when we retrieve people from the database. Don't worry if this is a bit confusing. Later, we will completely replace this static variable. We are only doing this to postpone introducing the concepts around using a database, so that we can focus on the concepts around MVC and HTTP.

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

		4. We now have a view that can handle displaying the details of a single person. It includes a button that the user can click to return to the Person List. Next, we need a way to get our users to navigate to the Person Detail view. Since the new action is in our Person controller, we know that we need a route that begins with /Person. In addition, since we named the action PersonDetail, we can infer that the route will start with /Person/PersonDetail. Since the PersonDetail() action's only parameter is named "id", it matches our default route template in our RouteConfig class: "{controller}/{action}/{id}". If we want to view the person with PersonId 3, we could browse directly to /Person/PersonDetail/3 to view that person. Next, edit the table in the Person List view (i.e. Views/Person/Index) to look like the following:

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
							<td>@Html.ActionLink("Detail", "PersonDetail", new { id = Model.People[i].PersonId })</td>
						</tr>
					}
				</tbody>
			</table>

		5. Run the application. (Note: If you have Visual Studio open to the PersonDetail.cshtml view when you start the application, it may throw an error. If so, just manually navigate to Person/Index by removing PersonDetail from the location in your browser. That route is useless wthout including an id.) You should now be able to click the Edit link next to any person in the table and navigate to the PersonDetail view. If you hover over the Edit link, you should be able to inspect the URL and notice that the Html.ActionLink helper constructed the proper URL to route us to the PersonDetail action. You can click the Done button to return to the Person List.

		6. Note that the labels for LastName and FirstName match the names of the properties of our PersonViewModel class. This is not ideal, since property names cannot contain spaces. To fix this, edit the PersonViewModel class, adding DisplayName attributes that will allow our Html.LabelFor() helper methods to use friendlier labels. Don't forget to add `using System.ComponentModel;`.

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

		1. Add a new controller action to handle loading a view that will contain a form for editing a person. Add the code below after the PersonDetail action in the PersonController. Note that we are creating an empty PersonViewModel to pass to our view. This will allow us to access our model and use Html helper methods to create our user interface. Also, note that we are explicitly passing a view name (i.e. "AddEditPerson" to the View() method. This is because we are going to create a view that will be used for both adding a new person, and editing an existing one. Since we will be breaking from the standard naming convention, we need to be explicit about the view that we want to load. MVC will not be able to figure it out for us.

			public ActionResult PersonAdd()
			{
				var personViewModel = new PersonViewModel();

				return View("AddEditPerson", personViewModel);
			}

		2. Since we explicitly told the View() method to look for a view named "AddEditPerson", MVC will try to load Views/Person/AddEditPerson.cshtml. For this to work, we need to create a view at that location. Right-click on the Views/Person folder, select Add, and click View. Name the view AddEditPerson, and create the view exactly as we did in previous steps. Replace the code in the view with the following:

			@model Lunch.Models.PersonViewModel
			@{
				ViewBag.Title = "Add Person";
			}

			<h2>@Model.FullName</h2>

			<div class="row">
				<div class="col-sm-6 col-xs-10">
					@using (Html.BeginForm("AddPerson", "Person"))
					{
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

		3. Next, edit the Person List view (i.e. Views/Person/Index), replacing `Total People: @Model.TotalPeople` with the following code:

			<div class="col-sm-12 form-group">
				<label>Total People:</label> @Model.TotalPeople
			</div>
			<div class="col-sm-12">
				@Html.ActionLink("Add Person", "PersonAdd", null, new { @class = "btn btn-primary" })
			</div>

		4. Add another controller action to handle receiving the new person, and persisting the data to our in-memory collection of people. Add the following action below the PersonAdd action in the PersonController:

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

		5. Run the application, and test adding a new person.
	
	D. Modify the PersonController, and update the AddEditPerson view to handle **Update** operations.
		
		1. Modify the AddEditPerson view. Right now, it is only useful for adding a person. It needs to be changed to be able to update a person as well. In order for the view to know whether to be in "Add Mode" or "Edit Mode", we can evaluate the PersonId property of our model. If PersonId is null, we are adding a new person. Otherwise, we must be editing an existing person. Change `ViewBag.Title = "Add Person"` to the following:

			var isEditMode = Model.PersonId != null;
			ViewBag.Title = isEditMode ? "Edit " + Model.FullName : "Add Person";

		2. Change the BeginForm() Html helper to the following:

			@using (Html.BeginForm(isEditMode ? "EditPerson" : "AddPerson", "Person"))
		
		3. Add the following hidden input inside the form. It will render as <input type="hidden" value="1">, allowing our PersonId to be posted back to the server, but keeping it from being editable or visible.

			@Html.HiddenFor(m => m.PersonId)

		4. Modify the submit button to contain this same conditional logic to display either "Save" or "Add":

			<button type="submit" class="btn btn-primary">@(isEditMode ? "Save" : "Add")</button>

		5. Add another controller action to handle loading the AddEditPerson view in "Edit Mode". This action will pass an existing person from our in-memory data store to the view. Note that the code in this action is almost identical to the code in the PersonDetail action. This is a good candidate for refactoring later to re-use our code rather than duplicate it. Add the following code after the AddPerson action:

			public ActionResult PersonEdit(int id)
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

					return View("AddEditPerson", personViewModel);
				}

				return new HttpNotFoundResult();
			}

		6. Add another controller action to handle receiving the updated person, and persisting the data to our in-memory collection of people. Add the following action below the PersonEdit action in the PersonController:

			[HttpPost]
			public ActionResult EditPerson(PersonViewModel personViewModel)
			{
				var person = People.SingleOrDefault(p => p.PersonId == personViewModel.PersonId);

				if (person != null)
				{
					person.LastName = personViewModel.LastName;
					person.FirstName = personViewModel.FirstName;

					return RedirectToAction("Index");
				}

				return new HttpNotFoundResult();
			}

		7. Modify the table in the Person List with the code below to add an "Edit" link that the user can click to navigate to our AddEditPerson view, and edit a person.

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
							<td>
								@Html.ActionLink("Detail", "PersonDetail", new { id = Model.People[i].PersonId })
								| @Html.ActionLink("Edit", "PersonEdit", new { id = Model.People[i].PersonId })
							</td>
						</tr>
					}
				</tbody>
			</table>

		8. Run the application, and test editing a person.

	E. Modify the PersonController, and update the AddEditPerson view to handle **Delete** operations.
	
		1. Add another controller action to handle deleting a person, and persisting to our in-memory collection of people. Add the following action below the EditPerson action in the PersonController:

			    [HttpPost]
				public ActionResult DeletePerson(PersonViewModel personViewModel)
				{
					var person = People.SingleOrDefault(p => p.PersonId == personViewModel.PersonId);

					if (person != null)
					{
						People.Remove(person);

						return RedirectToAction("Index");
					}

					return new HttpNotFoundResult();
				}

		2. Modify the table in the Person List with the code below to add a "Delete" link that the user can click to delete a person.

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
						<td>
							@Html.ActionLink("Detail", "PersonDetail", new { id = Model.People[i].PersonId })
							| @Html.ActionLink("Edit", "PersonEdit", new { id = Model.People[i].PersonId })
							| <a data-toggle="modal" href="@("#deletePersonModal" + i)">Delete</a>
							<div class="modal fade" id="@("deletePersonModal" + i)" tabindex="-1" role="dialog">
								<div class="modal-dialog">
									<div class="modal-content">
										@using (Html.BeginForm("DeletePerson", "Person"))
										{
											<input type="hidden" value="@Model.People[i].PersonId" name="PersonId" />
											<div class="modal-header">
												<button type="button" class="close" data-dismiss="modal">&times;</button>
												<h4 class="modal-title">Delete @Model.People[i].FullName</h4>
											</div>
											<div class="modal-body">
												Are you sure you want to delete @Model.People[i].FullName?
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

	3. Run the application, and test deleting a person.

	F. The following concepts can be introduced:
	    1. Model-binding in MVC controllers.
	    2. The Post-Redirect-Get (PRG) pattern.
		3. Http verbs: GET/POST.