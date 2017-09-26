using System.ComponentModel;

namespace Lunch.Models
{
    public class PersonViewModel
    {
        public int? PersonId { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Name")]
        public string FullName => FirstName + " " + LastName;
    }
}