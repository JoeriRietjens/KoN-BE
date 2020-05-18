using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL;
using BLL.Context;

namespace KoffieOfNie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderListsController : ControllerBase
    {
        private readonly DBContext _context;

        public OrderListsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/OrderLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderList>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/OrderLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderList>> GetOrderList(int id)
        {
            var orderList = await _context.Orders.FindAsync(id);

            if (orderList == null)
            {
                return NotFound();
            }

            return orderList;
        }

        // PUT: api/OrderLists/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderList(int id, OrderList orderList)
        {
            if (id != orderList.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderListExists(id))
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

        // POST: api/OrderLists
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<OrderList>> PostOrderList(OrderList orderList)
        {
            _context.Orders.Add(orderList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderList", new { id = orderList.Id }, orderList);
        }

        // DELETE: api/OrderLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderList>> DeleteOrderList(int id)
        {
            var orderList = await _context.Orders.FindAsync(id);
            if (orderList == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orderList);
            await _context.SaveChangesAsync();

            return orderList;
        }

        private bool OrderListExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
