using JustSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace JustSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;

        ItemsContext db;
        public ItemsController(ItemsContext context)
        {
            this.db = context;
            if (!db.Items.Any())
            {
                db.Items.Add(new Item { Name = "item1", Amount = 10 });
                db.Items.Add(new Item { Name = "item2", Amount = 5 });
                db.Items.Add(new Item { Name = "item1", Amount = 5 });
                db.SaveChanges();
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Get()
        {
            return await db.Items.ToListAsync();
        }

        // GET services/get
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                  return new ObjectResult(null); 
            }
            
            Item item = await db.Items.FirstOrDefaultAsync(x => x.Id == id);

                return item;
        }
        // POST services/create()
        [HttpPost]
        public async Task<ActionResult<Guid>> Post(string name, int amount)
        {
            var items = db.Items
                .Where(x => x.Name != null)
                .Select(x=>x.Id)
                .Select(x => new Item{
                    Name = name, 
                    Amount = amount 
                });

            await db.AddAsync(items);
            await db.SaveChangesAsync();


            return new ObjectResult(db.Items.LastOrDefault().Id);
        }

        // POST services/update/{id}
        [HttpPost("{id}")]
        public async Task<ActionResult<Guid>> Post(string name, int? amount)
        {
            if (string.IsNullOrWhiteSpace(name) && !amount.HasValue)
            { 
                return BadRequest();
            }

            IEnumerable<Item>? items = db.Items
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .Select(id => new Item { Name = name, Amount = amount.Value});

            foreach (Item item in items)
            { 
                db.Attach(item);
                db.Entry(item)
                  .Property(x => x.Amount)
                  .IsModified = true;
            }

            await db.SaveChangesAsync();

            return Ok(items.Select(x => x.Id));
        
        }
        //POST services/booking 
        [HttpPost]
        public async Task<ActionResult<string>> Post(Guid id, int? amount)
        {
            int rem=0;
            string name = null;
            if (id!= Guid.Empty && amount.ToString() != null )
            {
                return BadRequest();
            }

            IEnumerable<Item> items = db.Items
                .Where(x => x.Id == id)
                .Select(x => new Item
                {
                    Name = name,
                    Amount = amount.Value
                });

            var sumItems = items.Select(x => x.Amount).Sum();

            foreach(Item item in items)
            {
                if(sumItems - amount <= 0)
                {
                    return "Превышен остаток";
                }

                int remaind = sumItems - item.Amount;

                rem = remaind;
                return Ok(remaind);
            }
            return $"Остаток товаров {rem.ToString()}";
        }
        //PUT services/put
        [HttpPut] 
        public async Task<ActionResult<Item>> Put(Item item)
        {
            if(item==null)
            {
                return BadRequest();
            }

            if(!db.Items.Any(x => x.Id == item.Id))
            {
                return NotFound();
            }
            
            db.Update(item);
            await db.SaveChangesAsync();

            return Ok(item);
        }
        // DELETE services/delete
        [HttpDelete]
        public async Task<ActionResult<Item>> Delete(Guid id)
        {
            Item? item = await db.Items.FirstOrDefaultAsync(x => x.Id == id);
            if(item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            await db.SaveChangesAsync();

            return Ok(item);
        }
    }
}
