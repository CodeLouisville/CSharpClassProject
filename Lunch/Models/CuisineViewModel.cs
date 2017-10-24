using System.ComponentModel.DataAnnotations;

namespace Lunch.Models
{
    public class CuisineViewModel
    {
        [Required(ErrorMessage = "Cuisine is required.")]
        public int? CuisineId { get; set; }

        public string Name { get; set; }
    }
}