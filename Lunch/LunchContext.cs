using Lunch.Models;
using System.Data.Entity;

namespace Lunch
{
    public class LunchContext : DbContext
    {
        public LunchContext() : base("name=Lunch") { }

        public virtual DbSet<Person> People { get; set; }
    }
}