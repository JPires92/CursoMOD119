using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CursoMOD119.Data;
using CursoMOD119.Models;
using Microsoft.AspNetCore.Authorization;
using CursoMOD119.Lib;

namespace CursoMOD119.Controllers
{
    [Authorize(Policy = AppConstants.APP_POLICY)]
    public class StockMovementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockMovementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StockMovements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StockMovements.Include(s => s.Item);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StockMovements outlet "Saídas de stock"
        public IActionResult Outlet()
        {
            var stockMovements = _context.StockMovements
                .Where(stockMovement => stockMovement.Type == false)
                .Include(s => s.Item)
                .ToList();
            return View("Index", stockMovements);
        }

        // GET: StockMovements inlet "Entradas de stock"
        public IActionResult Inlet()
        {
            var stockMovements = _context.StockMovements
                .Where(stockMovement => stockMovement.Type == true)
                .Include(s => s.Item)
                .ToList();
            return View("Index", stockMovements);
        }


        // GET: StockMovements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StockMovements == null)
            {
                return NotFound();
            }

            var stockMovement = await _context.StockMovements
                .Include(s => s.Item)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (stockMovement == null)
            {
                return NotFound();
            }

            return View(stockMovement);
        }

        // GET: StockMovements/Create
        public IActionResult Create()
        {
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "Name");
            return View();
        }

        // POST: StockMovements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Type,Amount,MovementDate,ItemID")] StockMovement stockMovement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockMovement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ID", stockMovement.ItemID);
            return View(stockMovement);
        }

        // GET: StockMovements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StockMovements == null)
            {
                return NotFound();
            }

            var stockMovement = await _context.StockMovements.FindAsync(id);
            if (stockMovement == null)
            {
                return NotFound();
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ID", stockMovement.ItemID);
            return View(stockMovement);
        }

        // POST: StockMovements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Type,Amount,MovementDate,ItemID")] StockMovement stockMovement)
        {
            if (id != stockMovement.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockMovement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockMovementExists(stockMovement.ID))
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
            ViewData["ItemID"] = new SelectList(_context.Items, "ID", "ID", stockMovement.ItemID);
            return View(stockMovement);
        }

        // GET: StockMovements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StockMovements == null)
            {
                return NotFound();
            }

            var stockMovement = await _context.StockMovements
                .Include(s => s.Item)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (stockMovement == null)
            {
                return NotFound();
            }

            return View(stockMovement);
        }

        // POST: StockMovements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StockMovements == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StockMovements'  is null.");
            }
            var stockMovement = await _context.StockMovements.FindAsync(id);
            if (stockMovement != null)
            {
                _context.StockMovements.Remove(stockMovement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockMovementExists(int id)
        {
          return (_context.StockMovements?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
