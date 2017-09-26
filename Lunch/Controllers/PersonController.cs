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

        public ActionResult NewPerson()
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
    }
}