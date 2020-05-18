using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL;
using BLL.Context;
using Microsoft.AspNetCore.Cors;

namespace KoffieOfNie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DBContext _context;

        public ProductsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public IActionResult post([FromBody] OrderDTO i )
        {
            
                User user = new User() { Name = i.name };
                var result = _context.Users.Add(user);
                _context.SaveChanges();
                int userId = result.Entity.Id;

                OrderList orderList = new OrderList();
                var result2 = _context.Orders.Add(orderList);
                _context.SaveChanges();
                int orderId = result2.Entity.Id;

                Product product = new Product() { ProductType = i.coffeeType, Sugar = i.sugar, Milk = i.milk, UserId = userId, OrderId = orderId};
                _context.Products.Add(product);
                _context.SaveChanges();

           
            
            return Ok();
        }

        /*
        public async Task<ActionResult<Product>> PostProducts([FromBody]Product product)
        {
            //_context.Products.Add(product);
            //await _context.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = product.id }, product);
        }
        */

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.id == id);
        }
    }
}
