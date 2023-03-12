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
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index(string sort, string searchName, int? pageNumber)
        {
            if (_context.Clients == null)
            {
                Problem("Entity set 'ApplicationDbContext.Clients'  is null.");
            }

            ViewData["SearchName"] = searchName;
            ViewData["Sort"] = sort;
            ViewData["pageNumber"] = pageNumber;

            //var clientsSql = from i in _context.Clients select i;
            IQueryable<Client> clientsSql = _context.Clients;


            ///Método sem o MVC-grid
            //if (!string.IsNullOrEmpty(searchName))
            //{
            //    clientsSql = clientsSql.Where(i => i.Name.Contains(searchName) || i.Email.Contains(searchName) || i.Age.ToString().Contains(searchName));
            //}


            //switch (sort)
            //{
            //    case "name_desc":
            //        clientsSql = clientsSql.OrderByDescending(x => x.Name);
            //        break;
            //    case "name_asc":
            //        clientsSql = clientsSql.OrderBy(x => x.Name);
            //        break;
            //    case "age_desc":
            //        clientsSql = clientsSql.OrderByDescending(x => x.Age);
            //        break;
            //    case "age_asc":
            //        clientsSql = clientsSql.OrderBy(x => x.Age);
            //        break;

            //    case "email_asc":
            //        clientsSql = clientsSql.OrderBy(x => x.Email);
            //        break;
            //    case "email_desc":
            //        clientsSql = clientsSql.OrderByDescending(x => x.Email);
            //        break;

            //    case "active_asc":
            //        clientsSql = clientsSql.OrderBy(x => x.Active);
            //        break;
            //    case "active_desc":
            //        clientsSql = clientsSql.OrderByDescending(x => x.Active);
            //        break;
            //}

            //ViewData["NameSort"] = (sort == "name_desc") ? "name_asc" : "name_desc";
            //ViewData["AgeSort"] = (sort == "age_desc") ? "age_asc" : "age_desc";
            //ViewData["EmailSort"] = (sort == "email_desc") ? "email_asc" : "email_desc";
            //ViewData["ActiveSort"] = (sort == "active_desc") ? "active_asc" : "active_desc";

            //int pageSize = 5;

            //var clients = await PaginatedList<Client>.CreateAsync(clientsSql, pageNumber ?? 1, pageSize);

            return View(clientsSql.ToList());
           
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Age,Email,Active")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Age,Email,Active")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
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
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Clients?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
