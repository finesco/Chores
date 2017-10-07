using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chores.Models
{
    public class Chore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PointValue { get; set; }
        public int Frequency { get; set; }
        public bool AutoSchedule { get; set; }
        public int? PersonId { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
        public Person Person { get; set; }
    }
}
