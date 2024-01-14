using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProizvodisController : Controller
    {
        private readonly WebApplication1Context _dbContext;

        public ProizvodisController(WebApplication1Context dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Proizvodis
        public async Task<IActionResult> Index()
        {
              return _dbContext.Proizvodi != null ? 
                          View(await _dbContext.Proizvodi.ToListAsync()) :
                          Problem("Entity set 'WebApplication1Context.Proizvodi'  is null.");
        }

        // GET: Proizvodis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbContext.Proizvodi == null)
            {
                return NotFound();
            }

            var proizvodi = await _dbContext.Proizvodi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proizvodi == null)
            {
                return NotFound();
            }

            return View(proizvodi);
        }

        // GET: Proizvodis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proizvodis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Model,Svojstva,SlikeUrl,Godiste,Cijena,Placanja,Stanje")] Proizvodi proizvodi)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(proizvodi);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proizvodi);
        }

        // GET: Proizvodis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbContext.Proizvodi == null)
            {
                return NotFound();
            }

            var proizvodi = await _dbContext.Proizvodi.FindAsync(id);
            if (proizvodi == null)
            {
                return NotFound();
            }
            return View(proizvodi);
        }

        // POST: Proizvodis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Svojstva,SlikeUrl,Godiste,Cijena,Placanja,Stanje")] Proizvodi proizvodi)
        {
            if (id != proizvodi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(proizvodi);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProizvodiExists(proizvodi.Id))
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
            return View(proizvodi);
        }

        // GET: Proizvodis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.Proizvodi == null)
            {
                return NotFound();
            }

            var proizvodi = await _dbContext.Proizvodi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proizvodi == null)
            {
                return NotFound();
            }

            return View(proizvodi);
        }

        // POST: Proizvodis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Proizvodi == null)
            {
                return Problem("Entity set 'WebApplication1Context.Proizvodi'  is null.");
            }
            var proizvodi = await _dbContext.Proizvodi.FindAsync(id);
            if (proizvodi != null)
            {
                _dbContext.Proizvodi.Remove(proizvodi);
            }
            
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProizvodiExists(int id)
        {
          return (_dbContext.Proizvodi?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
