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
            new Person { PersonId = 1, LastName = "Halpert", FirstName = "Jim" },
            new Person { PersonId = 2, LastName = "Beesly", FirstName = "Pam" },
            new Person { PersonId = 3, LastName = "Scott", FirstName = "Michael"},
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

        public ActionResult PersonAdd()
        {
            var personViewModel = new PersonViewModel();

            return View("AddEditPerson", personViewModel);
        }

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
    }
}