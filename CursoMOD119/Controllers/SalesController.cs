using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CursoMOD119.Data;
using CursoMOD119.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NuGet.Packaging;
using CursoMOD119.ViewModels.Sales;
using CursoMOD119.ViewModels.Items;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Localization;

namespace CursoMOD119.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IHtmlLocalizer<SharedResource> _sharedLocalizer;

        public SalesController(ApplicationDbContext context, IToastNotification toastNotification, 
            IHtmlLocalizer<SharedResource> sharedLocalizer)
        {
            _context = context;
            _toastNotification = toastNotification;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sales.Include(s => s.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Client)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Name");
            ViewData["ItemIDs"] = new MultiSelectList(_context.Items, "ID", "Name");

            var saleViewModel = new SaleViewModel();
            var items = _context.Items.ToList();

            //saleViewModel.SelectableItems = items.Select(item => new SelectableItemViewModel
            //    {
            //        ID = item.ID,
            //        Name = item.Name,
            //        Price = item.Price,
            //        Selected = false
            //    }).ToList();

            saleViewModel.SelectableItems = items.ConvertAll<SelectableItemViewModel>(i => i);

            return View(saleViewModel);
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SaleDate,Amount,ClientID, SelectableItems")] SaleViewModel sale)
        {
            if (ModelState.IsValid)
            {
                List<Item> items = new List<Item>();

                foreach(var selectableItem in sale.SelectableItems)
                {
                    if (selectableItem.Selected)
                    {
                        Item? item = _context.Items.Find(selectableItem.ID);

                        if (item != null)
                            items.Add(item);
                    }   
                }
                Sale NewSale = sale;
                NewSale.Items = items;

                //Replaced by this 2 lines before (operators added in sale model)
                //Sale NewSale = new Sale
                //{
                //    SaleDate = sale.SaleDate,
                //    Amount = sale.Amount,
                //    ClientID = sale.ClientID,
                //    Items = items
                //};
                _context.Add(NewSale);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Name", sale.ClientID);
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                       .Include(s => s.Client)
                       .Include(i => i.Items)
                       .FirstOrDefaultAsync(m => m.ID == id);

            SaleViewModel saleViewModel = sale;
            var items = _context.Items.ToList();
            var selectableItems = items.Select(item => new SelectableItemViewModel
            {
                ID = item.ID,
                Name = item.Name,
                Price = item.Price,
                Selected = sale.Items.Contains(item)
            }).ToList();

            //saleViewModel.SelectableItems = items.ConvertAll<SelectableItemViewModel>(i => i);
            saleViewModel.SelectableItems = selectableItems;

            //var saleViewModel = new SaleViewModel()
            //{
            //    ID = sale.ID,
            //    Amount = sale.Amount,
            //    ClientID = sale.ClientID,
            //    SaleDate = sale.SaleDate,
            //    SelectableItems = selectableItems
            //};


            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Name");

            return View(saleViewModel);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SaleDate,Amount,ClientID, SelectableItems")] SaleViewModel sale)
        {
            if (id != sale.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Sale? UpdateSale =  await _context.Sales
                                   .Include(s => s.Client)
                                   .Include(i => i.Items)
                                   .FirstOrDefaultAsync(m => m.ID == sale.ID);
                try
                {
                    List<Item> itemsAdded = new List<Item>();
                    if (UpdateSale != null)
                    {
                        foreach (var itemSelected in sale.SelectableItems)
                        {
                            if (itemSelected.Selected)
                            {
                                Item? item = _context.Items.Find(itemSelected.ID);

                                if (item != null)
                                    itemsAdded.Add(item);
                            }
                        }

                    }

                    // Update the Sale entity with the new values
                    UpdateSale.SaleDate = sale.SaleDate;
                    UpdateSale.Amount = sale.Amount;
                    UpdateSale.ClientID = sale.ClientID;
                    UpdateSale.Items = itemsAdded;


                    _context.Update(UpdateSale);
                    await _context.SaveChangesAsync();

                    _toastNotification.AddSuccessToastMessage(
                                    string.Format(_sharedLocalizer["Edited sale successfully."].Value, sale.ID),
                                    new ToastrOptions { Title = _sharedLocalizer["Sale Edit"].Value });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.ID))
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
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Name", sale.ClientID);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .Include(s => s.Client)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sales == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sales'  is null.");
            }
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
          return (_context.Sales?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
