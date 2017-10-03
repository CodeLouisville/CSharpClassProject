# Week 5

In week 5, we should be working in the Week5 branch. We start with the basic ASP.NET MVC Web Application created in weeks 1-4. Following these instructions, we will convert our in-memory data store to use SQL Server Express LocalDB. We will also implement Entity Framework, an object-relational mapping (ORM) framework for .NET. 

V. Add a DbContext to create and connect to the Lunch database. The DbContext class is the main object through which our application will interact with our database. Like all C# objects, it is a class. However, unlike our models, it is a worker class. It is not just a blueprint for an entity.

	A. Right-click on the Lunch project, select Add, and click "New item...". Under Visual C#, select Data. Then, select ADO.NET Entity Data Model, change the name to "LunchContext" and click Add. In the Entity Data Model Wizard, select "Empty Code First model", and click Finish.

		1. Note that the new LunchContext class looks like any other class, but it inherits from DbContext. The DbContext class has a lot of code already implemented to deal with talking to our LocalDb database. We are simply extending it to define the schema of that database.

		2. Next, open the Web.config file and locate the ConnectionStrings node. Since we used the wizard to add our DbContext, and we named it LunchContext, we should see a connection string that is already ready to connect to a database named Lunch.LunchContext. That is not the best name for a database. It would be better if it were just called "Lunch". Also, the part that says "App=EntityFramework" is not ideal for tracing queries against our database, so we will change that to "Lunch" as well. Modify the connection string to look like the following:

			<add name="Lunch" connectionString="data source=(LocalDb)\MSSQLLocalDB;initial catalog=Lunch;integrated security=True;MultipleActiveResultSets=True;App=Lunch" providerName="System.Data.SqlClient" />

		3. Note that our packages.config has a new line that references Entity Framework. This is because the wizard added the EntityFramework NuGet package to our application, saving us the extra step.

		4. The new LunchContext class is an empty implementation of the DbContext class. Replace the entire contents of LunchContext with the following code:

			using Lunch.Models;
			using System.Data.Entity;

			namespace Lunch
			{
				public class LunchContext : DbContext
				{
					public LunchContext() : base("name=LunchContext") { }

					public virtual DbSet<Person> People { get; set; }
				}
			}

		5. The code above prescribes the following:

			a. When our LunchContext class is instantiated, our only empty constructor will call a constructor on the base class (i.e. DbContext), passing the string "name=Lunch". This tells the DbContext to look in the Web.config for a connection string named Lunch, and use that to interact with the database.
			b. Our database will have one table named People, with columns that match the names and types of the properties of the Person class.

	B. Enable Entity Framework Code-First Migrations.

		1. From the Tools menu, expand NuGet Package Manager, and select Package Manager Console. From the PM> prompt, type Enable-Migrations and press <Enter>.

		2. Note that this created a folder named Migrations with a class named Configuration that inherits from DbMigrationsConfiguration. Replace the entire contents of this file with the following:

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

		3. Return to Package Manage Console. From the PM> prompt, type Add-Migration and press <Enter>. When prompted for the Name, type InitialMigration.

		4. Note that this created a new class in our Migrations folder with a unique name. The name is prefixed with the date and time that the migration was created. It also includes the name that we supplied when creating the migration. This class contains all of the code necessary to create the Person table in our database. It also contains the code to drop the Person table in the event that we need to roll back to a previous version of our database.

	C. Modify the PersonController to use our LunchContext to read and write to the database.
	
		1. Remove the static People variable that we previously added at the top of the class. Then, modify the Index() action with the following code:

			public ActionResult Index()
			{
				using (var lunchContext = new LunchContext())
				{
					var personList = new PersonListViewModel
					{
						//Convert each Person to a PersonViewModel
						People = lunchContext.People.Select(p => new PersonViewModel
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

		2. Modify the PersonDetail() action with the following code:

			public ActionResult PersonDetail(int id)
			{
				using (var lunchContext = new LunchContext())
				{
					var person = lunchContext.People.SingleOrDefault(p => p.PersonId == id);
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
				}

				return new HttpNotFoundResult();
			}

		3. Modify the AddPerson() action with the following code:

			[HttpPost]
			public ActionResult AddPerson(PersonViewModel personViewModel)
			{
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
		
		4. Note that we no longer need to get the maximum PersonId, nor do we need to set PersonId for our new person because the database will be handling the generation of our primary key values. Also note that we are calling the SaveChanges() method on our LunchContext. Any time changes are made to the data that we want to persist to the database, we must call SaveChanges() or the changes will not be made in the database. This only applies to creating, updating, and deleting data. It does not apply to reading data, since no data changes.
		
		5. Modify the PersonEdit() action with the following code:

		    public ActionResult PersonEdit(int id)
			{
				using (var lunchContext = new LunchContext())
				{
					var person = lunchContext.People.SingleOrDefault(p => p.PersonId == id);
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
				}

				return new HttpNotFoundResult();
			}

		6. Modify the EditPerson() action with the following code:

		    [HttpPost]
			public ActionResult EditPerson(PersonViewModel personViewModel)
			{
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

		7. Modify the DeletePerson() action with the following code:

			[HttpPost]
			public ActionResult DeletePerson(PersonViewModel personViewModel)
			{
				using (var lunchContext = new LunchContext())
				{
					var person = lunchContext.People.SingleOrDefault(p => p.PersonId == personViewModel.PersonId);

					if (person != null)
					{
						lunchContext.People.Remove(person);
						lunchContext.SaveChanges();

						return RedirectToAction("Index");
					}
				}

				return new HttpNotFoundResult();
			}

		8. Note the correlation between actions that are marked with the [HttpPost] attribute, and actions that require us to call SaveChanges() on our LunchContext. That is because we are using the POST verb whenever we are modifying data. The GET verb should only ever be used when reading data. The other verbs (POST, PUT, DELETE, PATCH) can be used when modifying data. This is defined in the HTTP specification, but it is up to you as the developer to enforce this rule.

	D. Run the application and inspect your database.

		1. Run the application and browse to the Person List. Note that the first time this view is requested, it takes a while to load. This is because it is the first view requested that referenced the LunchContext(). Your database is not created until the DbContext is accessed by your code.

		2. Note that there are no people showing in the Person List. Stop the application.

		3. Under the View menu, select Server Explorer. In Server Explorer, expand Data Connections.

		4. If you still see a connection named LunchContext, you can delete it. This was added by the wizard when we created LunchContext, but we've since renamed our database to Lunch.

		5. Right-click Data Connections and select "Add Connection...". Select "Microsoft SQL Server" and click Continue. For the server name, enter (LocalDb)\MSSQLLocalDB. Under "Select or enter a database name", select Lunch. Then, press OK.

		6. In Server Explorer, expand the server, expand Tables, and expand People. You should see columns named PersonId, LastName, and FirstName.

		7. Right-click the People table and select "Show Table Data". Note that the table is empty.

		8. Return to the Package Manager Console. At the PM> prompt, type Update-Database and press <Enter>. Note that the output shows "Running Seed method".

		9. If you still have the People table open, click the refresh button to see our seed data. Otherwise, repeat step #7.

		10. Run the application and browse to the Person List. You should now see the seed data.

		11. Test all CRUD functions by creating, modifying, and deleting records. Then, stop the application.

		12. If you still have the People table open, click the refresh button to see our seed data. Otherwise, repeat step #7. Notice that your changes have persisted to the database.

		13. Run the applicaiton again, and notice that, unlike before, our data now persists even after we stop running the application.

		14. Return to the Package Manager Console. At the PM> prompt, type Update-Database and press <Enter>. Note that because our seed method used the AddOrUpdate() method, any records that we previously updated have been reset back to our seed data. Any records that we deleted have been restored. However, records that we created have been left alone.

	E. The following concepts can be introduced:

	    1. Adding and using an EntityFramework context to interact with a database.
	    2. Adding migrations to allow EntityFramework to create and update your database schema.
		3. Using Package Manager Console to run EntityFramework commands: Enable-Migrations, Add-Migration, Update-Database