using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chores.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }

        public ICollection<Chore> Chores { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
