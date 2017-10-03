using Lunch.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Lunch.Controllers
{
    public class PersonController : Controller
    {
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

        public ActionResult PersonAdd()
        {
            var personViewModel = new PersonViewModel();

            return View("AddEditPerson", personViewModel);
        }

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
    }
}