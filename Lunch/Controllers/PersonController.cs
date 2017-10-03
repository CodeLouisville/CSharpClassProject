using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunch.Models;

namespace Lunch.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index()
        {
            var personList = new PersonListViewModel
            {
                People = new List<PersonViewModel>
                {
                    new PersonViewModel { PersonId = 1, LastName = "Halpert", FirstName = "Jim" },
                    new PersonViewModel { PersonId = 2, LastName = "Beesly", FirstName = "Pam" },
                    new PersonViewModel { PersonId = 3, LastName = "Scott", FirstName = "Michael"},
                    new PersonViewModel { PersonId = 4, LastName = "Schrute", FirstName = "Dwight" },
                    new PersonViewModel { PersonId = 5, LastName = "Martin", FirstName = "Angela" },
                    new PersonViewModel { PersonId = 6, LastName = "Bernard", FirstName = "Andy" },
                    new PersonViewModel { PersonId = 7, LastName = "Malone", FirstName = "Kevin" },
                    new PersonViewModel { PersonId = 8, LastName = "Kapoor", FirstName = "Kelly" },
                    new PersonViewModel { PersonId = 9, LastName = "Palmer", FirstName = "Meredith" },
                    new PersonViewModel { PersonId = 10, LastName = "Flenderson", FirstName = "Toby" },
                    new PersonViewModel { PersonId = 11, LastName = "Hudson", FirstName = "Stanley" },
                    new PersonViewModel { PersonId = 12, LastName = "Bratton", FirstName = "Creed" },
                    new PersonViewModel { PersonId = 13, LastName = "Vance", FirstName = "Phyllis" },
                    new PersonViewModel { PersonId = 14, LastName = "Howard", FirstName = "Ryan" },
                    new PersonViewModel { PersonId = 15, LastName = "Philbin", FirstName = "Darryl" }
                }
            };

            personList.TotalPeople = personList.People.Count;

            return View(personList);
        }
    }
}