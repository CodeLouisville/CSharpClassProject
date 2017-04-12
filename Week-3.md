#Week 2

In week 3, we should be working in the Week3 branch. We start with the basic ASP.NET MVC Web Application created
weeks 1 and 2. Following these instructions, we will modify the application to have a main menu, use the Bootstrap
responsive design framework, and impelement a main menu.

III. Implement Responsive Design, Views, and Models

     A. Edit the Layout view to use Bootstrap.
	    1. Open Views/Shared/_Layout.cshtml for editing.
		2. Add the following link tag inside the head tag. This will load the Bootstrap cascading stylesheet from
		   a content delivery network (CDN). At the time of this tutorial, version 3.x is the latest non-alpha
		   version of Bootstrap.

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

     B. Create a view model that will be used to represent the Person view.
	    1. Right-click the Models folder and select Add -> Class...
		2. Name the new class PersonViewModel and replace its contents with the following code...

		   namespace Lunch.Models
           {
               public class PersonViewModel
               {
                   public int? PersonId { get; set; }
                   public string LastName { get; set; }
                   public string FirstName { get; set; }
               }
           }

	 C. Create a new controller for managing People.
		1. Right-click the Controllers folder and select Add -> Controller...
		2. Select MVC 5 Controller - Empty. Click Add.
		3. For Controller name, enter PersonController. Click Add.
		4. Note that the PersonController class has a method called Index() that returns an ActionResult. The
	 	   return statement returns the result of the controller's View() method. Also, note that a Person folder
		   was added under the Views folder.

	 D. Add a Person/Index view.
	    1. Right-click the Views/Person folder and select Add -> View...
	    2. For View name, enter Index.
	    3. Check the "Use a layout page" box, but don't enter anything in the box. Click Add.

TODO: Complete Week 3 Tutorial