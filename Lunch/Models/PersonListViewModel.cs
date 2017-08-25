using System.Collections.Generic;

namespace Lunch.Models
{
    public class PersonListViewModel
    {
        public List<PersonViewModel> People { get; set; }
        public int TotalPeople { get; set; }
    }
}