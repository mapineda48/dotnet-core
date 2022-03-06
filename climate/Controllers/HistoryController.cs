using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Climate.Models;

namespace Climate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/History
        [HttpGet]
        public async Task<ActionResult<IEnumerable<History>>> Gethistory()
        {
            var rows = await (from h in _context.Historys select h).ToListAsync();

            return await _context.Historys.ToListAsync();
        }

        // GET: api/History/5
        [HttpGet("{id}")]
        public async Task<ActionResult<History>> GetHistory(int id)
        {
            /*
             *It is simply a shortcut to create a new history for the client
             */
            if (id == -1)
            {
                return await PostHistory(new History());
            }

            var history = await _context.Historys.FindAsync(id);

            if (history == null)
            {
                return NotFound();
            }

            return history;
        }

        // POST: api/History
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<History>> PostHistory(History history)
        {
            _context.Historys.Add(history);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistory", new { id = history.Id }, history);
        }

    }
}
