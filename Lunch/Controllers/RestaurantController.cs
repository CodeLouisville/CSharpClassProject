using Lunch.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Lunch.Controllers
{
    public class RestaurantController : Controller
    {
        public ActionResult Index()
        {
            using (var lunchContext = new LunchContext())
            {
                var restaurantList = new RestaurantListViewModel
                {
                    //Convert each Restaurant to a RestaurantViewModel
                    Restaurants = lunchContext.Restaurants.Select(r => new RestaurantViewModel
                    {
                        RestaurantId = r.RestaurantId,
                        Name = r.Name,
                        Cuisine = new CuisineViewModel
                        {
                            CuisineId = r.CuisineId,
                            Name = r.Name
                        }
                    }).ToList()
                };

                restaurantList.TotalRestaurants = restaurantList.Restaurants.Count;

                return View(restaurantList);
            }
        }

        public ActionResult RestaurantDetail(int id)
        {
            using (var lunchContext = new LunchContext())
            {
                var restaurant = lunchContext.Restaurants.SingleOrDefault(p => p.RestaurantId == id);
                if (restaurant != null)
                {
                    var restaurantViewModel = new RestaurantViewModel
                    {
                        RestaurantId = restaurant.RestaurantId,
                        Name = restaurant.Name,
                        Cuisine = new CuisineViewModel
                        {
                            CuisineId = restaurant.CuisineId,
                            Name = restaurant.Cuisine.Name
                        }
                    };

                    return View(restaurantViewModel);
                }
            }

            return new HttpNotFoundResult();
        }

        public ActionResult RestaurantAdd()
        {
            using (var lunchContext = new LunchContext())
            {
                ViewBag.Cuisines = lunchContext.Cuisines.Select(c => new SelectListItem
                {
                    Value = c.CuisineId.ToString(),
                    Text = c.Name
                }).ToList();
            }

            var restaurantViewModel = new RestaurantViewModel();

            return View("AddEditRestaurant", restaurantViewModel);
        }

        [HttpPost]
        public ActionResult AddRestaurant(RestaurantViewModel restaurantViewModel)
        {
            if (!ModelState.IsValid)
            {
                using (var lunchContext = new LunchContext())
                {
                    ViewBag.Cuisines = lunchContext.Cuisines.Select(c => new SelectListItem
                    {
                        Value = c.CuisineId.ToString(),
                        Text = c.Name
                    }).ToList();

                    return View("AddEditRestaurant", restaurantViewModel);
                }
            }

            using (var lunchContext = new LunchContext())
            {
                var restaurant = new Restaurant
                {
                    Name = restaurantViewModel.Name,
                    CuisineId = restaurantViewModel.Cuisine.CuisineId.Value
                };

                lunchContext.Restaurants.Add(restaurant);
                lunchContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult RestaurantEdit(int id)
        {
            using (var lunchContext = new LunchContext())
            {
                ViewBag.Cuisines = lunchContext.Cuisines.Select(c => new SelectListItem
                {
                    Value = c.CuisineId.ToString(),
                    Text = c.Name
                }).ToList();

                var restaurant = lunchContext.Restaurants.SingleOrDefault(p => p.RestaurantId == id);
                if (restaurant != null)
                {
                    var restaurantViewModel = new RestaurantViewModel
                    {
                        RestaurantId = restaurant.RestaurantId,
                        Name = restaurant.Name,
                        Cuisine = new CuisineViewModel
                        {
                            CuisineId = restaurant.CuisineId,
                            Name = restaurant.Cuisine.Name
                        }
                    };

                    return View("AddEditRestaurant", restaurantViewModel);
                }
            }

            return new HttpNotFoundResult();
        }

        [HttpPost]
        public ActionResult EditRestaurant(RestaurantViewModel restaurantViewModel)
        {
            if (!ModelState.IsValid)
            {
                using (var lunchContext = new LunchContext())
                {
                    ViewBag.Cuisines = lunchContext.Cuisines.Select(c => new SelectListItem
                    {
                        Value = c.CuisineId.ToString(),
                        Text = c.Name
                    }).ToList();

                    return View("AddEditRestaurant", restaurantViewModel);
                }
            }

            using (var lunchContext = new LunchContext())
            {
                var restaurant = lunchContext.Restaurants.SingleOrDefault(p => p.RestaurantId == restaurantViewModel.RestaurantId);

                if (restaurant != null)
                {
                    restaurant.Name = restaurantViewModel.Name;
                    restaurant.CuisineId = restaurantViewModel.Cuisine.CuisineId.Value;
                    lunchContext.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return new HttpNotFoundResult();
        }

        [HttpPost]
        public ActionResult DeleteRestaurant(RestaurantViewModel restaurantViewModel)
        {
            using (var lunchContext = new LunchContext())
            {
                var restaurant = lunchContext.Restaurants.SingleOrDefault(p => p.RestaurantId == restaurantViewModel.RestaurantId);

                if (restaurant != null)
                {
                    lunchContext.Restaurants.Remove(restaurant);
                    lunchContext.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return new HttpNotFoundResult();
        }
    }
}