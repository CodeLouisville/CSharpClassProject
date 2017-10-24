using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lunch.Models
{
    public class PersonViewModel
    {
        public PersonViewModel()
        {
            FoodPreferences = new List<FoodPreferenceViewModel>();
        }

        public int? PersonId { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [DisplayName("Name")]
        public string FullName => FirstName + " " + LastName;

        public List<FoodPreferenceViewModel> FoodPreferences { get; set; }
    }
}