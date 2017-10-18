using System.Collections.Generic;
using System.ComponentModel;

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
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Name")]
        public string FullName => FirstName + " " + LastName;

        public List<FoodPreferenceViewModel> FoodPreferences { get; set; }
    }
}