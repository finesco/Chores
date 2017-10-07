using Chores.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chores.Data
{
    public class ChoreContext : DbContext
    {
        public ChoreContext(DbContextOptions<ChoreContext> options) : base (options)
        { }

        public DbSet<Chore> Chores { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
    }
}
