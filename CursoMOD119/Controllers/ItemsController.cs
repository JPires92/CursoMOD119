using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CursoMOD119.Data;
using CursoMOD119.Models;
using CursoMOD119.Lib;
using Microsoft.AspNetCore.Authorization;

namespace CursoMOD119.Controllers
{
    [Authorize(Policy = AppConstants.APP_POLICY)]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Get all Items
        public async Task<IActionResult> Index(string sort, string searchName, int? pageNumber)
        {
            if (_context.Items == null)
            {
                Problem("Entity set 'ApplicationDbContext.Items'  is null.");
            }


            ViewData["SearchName"] = searchName;

            var itemsSql = from i in _context.Items select i;


            if (!string.IsNullOrEmpty(searchName))
            {
                itemsSql = itemsSql.Where(i => i.Name.Contains(searchName));
            }


            switch (sort)
            {
                case "name_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.Name);
                    break;
                case "name_asc":
                    itemsSql = itemsSql.OrderBy(x => x.Name);
                    break;
                case "price_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.Price);
                    break;
                case "price_asc":
                    itemsSql = itemsSql.OrderBy(x => x.Price);
                    break;

                case "creationDate_asc":
                    itemsSql = itemsSql.OrderBy(x => x.CreationDate);
                    break;
                case "creationDate_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.CreationDate);
                    break;

                case "discontinued_asc":
                    itemsSql = itemsSql.OrderBy(x => x.Discontinued);
                    break;
                case "discontinued_desc":
                    itemsSql = itemsSql.OrderByDescending(x => x.Discontinued);
                    break;
            }

            if (sort == "name_desc")
            {
                ViewData["NameSort"] = "name_asc";
            }
            else
            {
                ViewData["NameSort"] = "name_desc";
            }

            if (sort == "price_desc")
            {
                ViewData["PriceSort"] = "price_asc";
            }
            else
            {
                ViewData["PriceSort"] = "price_desc";
            }

            if (sort == "creationDate_desc")
            {
                ViewData["CreationDateSort"] = "creationDate_asc";
            }
            else
            {
                ViewData["CreationDateSort"] = "creationDate_desc";
            }

            if (sort == "discontinued_desc")
            {
                ViewData["DiscontinuedSort"] = "discontinued_asc";
            }
            else
            {
                ViewData["DiscontinuedSort"] = "discontinued_desc";
            }

            int pageSize = 5;

            var items = await PaginatedList<Item>.CreateAsync(itemsSql, pageNumber ?? 1, pageSize);

            return View(items);

        }

        // GET: Get Available Items
        public IActionResult IndexAvailable()
        {
            if (_context.Items != null)
            {
                var availableItems = _context.Items
                    .Where(item => item.Discontinued == false)
                    .ToList();

                return View("Index", availableItems);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Items'  is null.");
            }                    
        }

        // Get: Get Discontinued Items
        public IActionResult IndexDiscontinued()
        {
            if (_context.Items != null)
            {
                var discontinuedItems = _context.Items
                    .Where(item => item.Discontinued == true)
                    .ToList();

                return View("Index", discontinuedItems);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Items'  is null.");
            }
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Price,CreationDate,Discontinued")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price,CreationDate,Discontinued")] Item item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return (_context.Items?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
