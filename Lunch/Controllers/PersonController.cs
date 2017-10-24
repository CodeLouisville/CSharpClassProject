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
            if (!ModelState.IsValid)
            {
                return View("AddEditPerson", personViewModel);
            }

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
            if (!ModelState.IsValid)
            {
                return View("AddEditPerson", personViewModel);
            }

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

        public ActionResult ManageFoodPreferences(int id)
        {
            using (var lunchContext = new LunchContext())
            {
                var person = lunchContext.People.Include("FoodPreferences").SingleOrDefault(p => p.PersonId == id);
                
                if (person == null)
                    return new HttpNotFoundResult();
                
                var personViewModel = new PersonViewModel {
                    PersonId = person.PersonId,
                    LastName = person.LastName,
                    FirstName = person.FirstName
                };

                //By adding .ToList() to lunchContext.Cuisines, we are forcing a single query to retrieve all cusines from the
                //database before we begin the loop. If we omit .ToList(), it may still work, but it will result in a seperate
                //round-trip to the database to get each cuisine.
                foreach (var cuisine in lunchContext.Cuisines.ToList())
                {
                    //If no rating is found, currentRating will be null. "?." is inown as the null-conditional operator. It
                    //keeps us from having to write more code to deal with null values.
                    var currentRating = person.FoodPreferences.SingleOrDefault(fp => fp.CuisineId == cuisine.CuisineId)?.Rating;

                    personViewModel.FoodPreferences.Add(new FoodPreferenceViewModel
                    {
                        Cuisine = new CuisineViewModel { CuisineId = cuisine.CuisineId, Name = cuisine.Name },
                        //If currentRating is null, we will assign -1 to indicate that there is no rating. "??" is known as
                        //the null-coalescing operator. It allows us to specify a different value if currentRating is null.
                        Rating = currentRating ?? -1
                    });
                }

                return View(personViewModel);  
            }
        }

        [HttpPost]
        public ActionResult EditFoodPreferences(PersonViewModel personViewModel)
        {
            using (var lunchContext = new LunchContext())
            {
                var person = lunchContext.People.Include("FoodPreferences").SingleOrDefault(p => p.PersonId == personViewModel.PersonId);

                if (person == null)
                    return new HttpNotFoundResult();

                foreach (var foodPreference in personViewModel.FoodPreferences)
                {
                    if (foodPreference.Rating != -1)
                    {
                        var existingFoodPreference = person.FoodPreferences.SingleOrDefault(fp => fp.CuisineId == foodPreference.Cuisine.CuisineId);
                        if (existingFoodPreference != null)
                        {
                            existingFoodPreference.Rating = foodPreference.Rating;
                        }
                        else
                        {
                            person.FoodPreferences.Add(new FoodPreference
                            {
                                CuisineId = foodPreference.Cuisine.CuisineId.Value,
                                Rating = foodPreference.Rating
                            });
                        }
                    }
                }

                lunchContext.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}