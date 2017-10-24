using System.ComponentModel.DataAnnotations;

namespace Lunch.Models
{
    public class RestaurantViewModel
    {
        public int? RestaurantId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public CuisineViewModel Cuisine { get; set; }
    }
}