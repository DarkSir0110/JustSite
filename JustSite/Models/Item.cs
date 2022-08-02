using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustSite.Models
{
    public class Item : IItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public int Amount { get; set; }
 
    }

    

}
