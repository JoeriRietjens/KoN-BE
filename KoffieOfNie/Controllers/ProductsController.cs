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
using System.Runtime.InteropServices;

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
        //[HttpGet]
        //public async Task<IActionResult> GetUserWithOrderWithProducts([FromQuery] int userId)
        //{
        //    var user = await _context.Users.FindAsync(userId);

        //    var order = await _context.Orders.FirstOrDefaultAsync(order => order.UserId == userId);

        //    var products = await _context.Products.Where(product => product.OrderId == order.Id).ToListAsync();

        //    //var result = _context.Products.Where( x => x.OrderId ==  Order.Id).ToList();
            
        //    return Ok();  
        //}
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.Users.Include(user => user.Orders).ThenInclude(order => order.products).ToListAsync();

            return Ok(result);
        }


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
            if (id != product.Id)
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
        public IActionResult post([FromBody] ProductDTO productDTO)
        {
            //Creates an user who placed the order.
            User user = new User() { Name = productDTO.username };  
            var result = _context.Users.Add(user);
            _context.SaveChanges();
            int userId = result.Entity.Id;

            //Creates an order with the id of the user that just has been made.
            Order order = new Order() { UserId = userId }; 
            var result2 = _context.Orders.Add(order);
            _context.SaveChanges();
            int orderId = result2.Entity.Id;

            //Creates an product with the orderId that just has been made.
            foreach (var i in productDTO.products)
            {
                Product product = new Product() { ProductType = i.coffeeType, Sugar = i.sugar, Milk = i.milk, OrderId = orderId };
                _context.Products.Add(product);
                _context.SaveChanges();
            }               
            
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
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
