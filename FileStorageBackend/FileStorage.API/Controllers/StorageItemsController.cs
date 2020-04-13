using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileStorage.Data.Models;
using FileStorage.Data.Persistence;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageItemsController : ControllerBase
    {
        private readonly FileStorageContext _context;

        public StorageItemsController(FileStorageContext context)
        {
            _context = context;
        }

        // GET: api/StorageItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StorageItem>>> GetStorageItems()
        {
            return await _context.StorageItems.ToListAsync();
        }

        // GET: api/StorageItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StorageItem>> GetStorageItem(Guid id)
        {
            var storageItem = await _context.StorageItems.FindAsync(id);

            if (storageItem == null)
            {
                return NotFound();
            }

            return storageItem;
        }

        // PUT: api/StorageItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStorageItem(Guid id, StorageItem storageItem)
        {
            if (id != storageItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(storageItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StorageItemExists(id))
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

        // POST: api/StorageItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<StorageItem>> PostStorageItem(StorageItem storageItem)
        {
            _context.StorageItems.Add(storageItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStorageItem", new { id = storageItem.Id }, storageItem);
        }

        // DELETE: api/StorageItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StorageItem>> DeleteStorageItem(Guid id)
        {
            var storageItem = await _context.StorageItems.FindAsync(id);
            if (storageItem == null)
            {
                return NotFound();
            }

            _context.StorageItems.Remove(storageItem);
            await _context.SaveChangesAsync();

            return storageItem;
        }

        private bool StorageItemExists(Guid id)
        {
            return _context.StorageItems.Any(e => e.Id == id);
        }
    }
}
