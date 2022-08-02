using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustSite.Models
{
    public class ItemsContext : DbContext 
    {
        public DbSet<Item> Items { get; set; }
        public ItemsContext(DbContextOptions<ItemsContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        
    }
}
