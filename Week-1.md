#Week 1

In week 1, we will create a new web application. There is no Week1 branch because we will be starting from scratch.
The final product should match the code in the Week2 branch. The application can be launched in the browser, but a
403.14 (Forbidden) error is thrown.

I.   Create a New ASP.NET Application from Scratch (like Grandma used to do it)
	 
     A. Open Visual Studio. Click File -> New -> Project...
     B. Under Web, select ASP.NET Web Application (.NET Framework).
        1. Name the project Lunch. 
        2. Set the location to the place where you prefer to store your code repositories.
        3. Check the box that says "Create directory for solution".
        4. Check the box that says "Create new Git repository". This will run git init in the folder, adding
	    a .git folder, and a .gitignore file that will ignore common files and folders specific to Visual Studio.
        5. Click OK.
     C. From the "New ASP.NET Web Application" dialog, select Empty. We will start this without anything so
	    that we understand everything.
        1. Do not check any boxes under "Add folders and core references for".
        2. Do not check "Add unit tests".
        3. Leave Authentication set to "No Authentication".
        4. Click OK.
	 D. The following concepts can be introduced:
	    1. Creating a new project in Visual Studio
	    2. Viewing the project structure in Solution Explorer
	    3. Interacting with Git and GitHub with Visual Studio in Team Explorer
	    4. The anatomy of a C# application (solutions, projects, references, configuration, etc.)