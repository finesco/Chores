using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chores.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime TargetDate { get; set; }
        public int? PersonId { get; set; }
        public int ChoreId { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool Credited { get; set; }

        public Chore Chore { get; set; }
        public Person Person { get; set; }
    }
}
