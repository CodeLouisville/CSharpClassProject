# Week 3

In week 3, we should be working in the Week3 branch. We start with the basic ASP.NET MVC Web Application created in weeks 1 and 2. Following these instructions, we will modify the application to use the Bootstrap responsive design framework.

III. Implement Responsive Design, Views, and Models

    A. Edit the _Layout, adding Bootstrap, jQUery, and more.

		1. Open Views/Shared/_Layout.cshtml for editing.
		2. Add the following link tag inside the head tag below the title element. This will load the Bootstrap
		   cascading stylesheet from a content delivery network (CDN). At the time of this tutorial, version 3.x is
		   the latest non-alpha version of Bootstrap.

		   <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" 
				rel="stylesheet" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" 
				crossorigin="anonymous">

		3. Add the following two script tags to the bottom of your body tag. Bootstrap 3.x depends on version
		   1.9.0 or higher of jQuery, so that JavaScript library must be loaded first.

			<script src="https://code.jquery.com/jquery-3.2.1.min.js" 
				integrity="sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4=" 
				crossorigin="anonymous"></script>
			<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" 
				integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" 
				crossorigin="anonymous"></script>

		4. Add a div with the .container class around your body content to implement Bootstrap's grid system.

			<div class="container">
				<h1>Lunch!</h1>
				<div>@RenderBody()</div>
				<footer>Lunch &copy; @DateTime.Now.Year Hungry Developers</footer>
			</div>

    B. Create a view model that will be used to represent a person.

		1. Right-click the Models folder and select Add -> Class...
		2. Name the new class PersonViewModel and replace its contents with the following code:

			namespace Lunch.Models
			{
				public class PersonViewModel
				{
					public int? PersonId { get; set; }
					public string LastName { get; set; }
					public string FirstName { get; set; }
					public string FullName => FirstName + " " + LastName;
				}
			}

	C. Create another view model that will be used to represent the person list.

		1. Right-click the Models folder and select Add -> Class...
		2. Name the new class PersonListViewModel and replace its contents with the following code:

			using System.Collections.Generic;

			namespace Lunch.Models
			{
				public class PersonListViewModel
				{
					public List<PersonViewModel> People { get; set; }
					public int TotalPeople { get; set; }
				}
			}

	D. Create a new controller for managing People.

		1. Right-click the Controllers folder and select Add -> Controller...
		2. Select MVC 5 Controller - Empty. Click Add.
		3. For Controller name, enter PersonController. Click Add.
		4. Note that the PersonController class has a method called Index() that returns an ActionResult. The
	 	   return statement returns the result of the controller's View() method. Also, note that a Person folder
		   was automatically added under the Views folder.

	E. Add a Person/Index view. This is the view that will display the PersonListViewModel. It would normally be
	   named PersonList. However, by naming it Index, we are wiring it up as the default route under Person/.

		1. Right-click the Views/Person folder and select Add -> View...
		2. For View name, enter Index.
		3. Check the "Use a layout page" box, but don't enter anything in the input below it. By leaving this box
		   blank, the default _Layout view will be used. Click Add.
		4. Wire up the new view to use the PersonListViewModel class as its model. To do this, add the following
		   line to the top of the view:

			@model Lunch.Models.PersonListViewModel

		5. Change ViewBag.Title = "Index" to ViewBag.Title = "People".
		6. Change <h2>Index</h2> to <h2>People</h2>.

	F. Hard-code some people to display in the Person/Index view, and send the list to the view.

		1. Add the following using statement at the top of the PersonController:
		
			using Lunch.Models;
		   
		2. In PersonController, modify the Index() action to look like the following:

			public ActionResult Index()
			{
				var personList = new PersonListViewModel
				{
					People = new List<PersonViewModel>
					{
						new PersonViewModel { PersonId = 1, LastName = "Gibbons", FirstName = "Peter" },
						new PersonViewModel { PersonId = 2, LastName = "Bolton", FirstName = "Michael" },
						new PersonViewModel { PersonId = 3, LastName = "Nagheenanajar", FirstName = "Samir"},
						new PersonViewModel { PersonId = 4, LastName = "Smykowski", FirstName = "Tom" },
						new PersonViewModel { PersonId = 5, LastName = "Waddams", FirstName = "Milton" },
						new PersonViewModel { PersonId = 6, LastName = "Lumbergh", FirstName = "Bill" }
					}
				};

				return View(personList);
			}

	G. Modify the Person/Index to display each person in a table.

		1. Add the following markup to the bottom of the Person/Index view:

			<table class="table table-striped">
				<thead>
					<tr>
						<th>Name</th>
					</tr>
				</thead>
				<tbody>
					@for (var i = 0; i < Model.People.Count; i++)
					{
						<tr>
							<td>@Model.People[i].FullName</td>
						</tr>
					}
				</tbody>
			</table>

			Total People: @Model.TotalPeople

		2. Run the application and manually browse to /Person. Then, browse to /Person/Index. Note that the
		   result is the same.