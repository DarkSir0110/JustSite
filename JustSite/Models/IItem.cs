using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustSite.Models
{
    public interface IItem
    {
        string? Name { get; set; }
        int Amount { get; set; }
    }
}
