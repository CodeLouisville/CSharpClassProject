using System.Collections.Generic;

namespace Lunch.Models
{
    public class RestaurantListViewModel
    {
        public List<RestaurantViewModel> Restaurants { get; set; }
        public int TotalRestaurants { get; set; }
    }
}