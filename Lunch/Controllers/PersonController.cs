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
                    new PersonViewModel { PersonId = 1, LastName = "Gibbons", FirstName = "Peter" },
                    new PersonViewModel { PersonId = 2, LastName = "Bolton", FirstName = "Michael" },
                    new PersonViewModel { PersonId = 3, LastName = "Nagheenanajar", FirstName = "Samir"},
                    new PersonViewModel { PersonId = 4, LastName = "Smykowski", FirstName = "Tom" },
                    new PersonViewModel { PersonId = 5, LastName = "Waddams", FirstName = "Milton" },
                    new PersonViewModel { PersonId = 6, LastName = "Lumbergh", FirstName = "Bill" }
                }
            };

            personList.TotalPeople = personList.People.Count;

            return View(personList);
        }
    }
}