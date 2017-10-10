namespace Lunch.Models
{
    public class RestaurantViewModel
    {
        public int? RestaurantId { get; set; }
        public string Name { get; set; }
        public CuisineViewModel Cuisine { get; set; }
    }
}