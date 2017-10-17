using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lunch.Models
{
    public class FoodPreference
    {
        [Key]
        [Column(Order = 1)]
        public int PersonId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CuisineId { get; set; }

        public int Rating { get; set; }

        public virtual Person Person { get; set; }

        public virtual Cuisine Cuisine { get; set; }
    }
}