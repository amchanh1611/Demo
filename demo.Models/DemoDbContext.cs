using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo.Models
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }

    }
}
