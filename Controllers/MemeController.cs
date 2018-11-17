using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MemeBank.Models;

namespace MemeBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemeController : ControllerBase
    {
        private readonly MemeBankContext _context;

        public MemeController(MemeBankContext context)
        {
            _context = context;
        }

        // GET: api/Meme
        [HttpGet]
        public IEnumerable<MemeItem> GetMemeItem()
        {
            return _context.MemeItem;
        }

        // GET: api/Meme/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemeItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var memeItem = await _context.MemeItem.FindAsync(id);

            if (memeItem == null)
            {
                return NotFound();
            }

            return Ok(memeItem);
        }

        // PUT: api/Meme/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemeItem([FromRoute] int id, [FromBody] MemeItem memeItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != memeItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(memeItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemeItemExists(id))
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

        // POST: api/Meme
        [HttpPost]
        public async Task<IActionResult> PostMemeItem([FromBody] MemeItem memeItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MemeItem.Add(memeItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMemeItem", new { id = memeItem.Id }, memeItem);
        }

        // DELETE: api/Meme/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemeItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var memeItem = await _context.MemeItem.FindAsync(id);
            if (memeItem == null)
            {
                return NotFound();
            }

            _context.MemeItem.Remove(memeItem);
            await _context.SaveChangesAsync();

            return Ok(memeItem);
        }
        // GET: api/Meme/Tags
        [Route("tags")]
        [HttpGet]
        public async Task<List<string>> GetTags()
        {
            var memes = (from m in _context.MemeItem
                         select m.Tags).Distinct();

            var returned = await memes.ToListAsync();

            return returned;
        }

        private bool MemeItemExists(int id)
        {
            return _context.MemeItem.Any(e => e.Id == id);
        }
    }
}