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
    public class KorisnicisController : Controller
    {
        private readonly WebApplication1Context _context;

        public KorisnicisController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: Korisnicis
        public async Task<IActionResult> Index()
        {
              return _context.Korisnici != null ? 
                          View(await _context.Korisnici.ToListAsync()) :
                          Problem("Entity set 'WebApplication1Context.Korisnici'  is null.");
        }

        // GET: Korisnicis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Korisnici == null)
            {
                return NotFound();
            }

            var korisnici = await _context.Korisnici
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnici == null)
            {
                return NotFound();
            }

            return View(korisnici);
        }

        // GET: Korisnicis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Korisnicis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ime,Prezime,KorisnickoIme,Email,Role,Lozinka")] Korisnici korisnici)
        {
            if (ModelState.IsValid)
            {
                if (!IsValidEmail(korisnici.Email))
                {
                    if (!IsValidPassword(korisnici.Lozinka))
                    {
                        ModelState.AddModelError(nameof(Korisnici.Lozinka), "Lozinka mora sadržavati najmanje jedno veliko slovo i biti duljine 6.");
                        ModelState.AddModelError(nameof(Korisnici.Email), "Email mora sadržavati znak @.");
                        return View(korisnici);
                    }
                    ModelState.AddModelError(nameof(Korisnici.Email), "Email mora sadržavati znak @.");
                    return View(korisnici);
                }
                else if(!IsValidPassword(korisnici.Lozinka))
                {
                    ModelState.AddModelError(nameof(Korisnici.Lozinka), "Lozinka mora sadržavati najmanje jedno veliko slovo i biti duljine 6.");
                    return View(korisnici);

                }
                if (_context.Korisnici.Any(u => u.Lozinka == korisnici.Lozinka) && _context.Korisnici.Any(u => u.Email == korisnici.Email))
                {
                    string alertHtml = @"
                        <!DOCTYPE html>
                        <html lang='en'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <style>
                                body {
                                    font-family: 'Arial', sans-serif;
                                    margin: 0;
                                    padding: 0;
                                    display: flex;
                                    align-items: center;
                                    justify-content: center;
                                    height: 100vh;
                                    background-color: #f4f4f4;
                                }

                                .overlay {
                                    position: fixed;
                                    top: 0;
                                    left: 0;
                                    width: 100%;
                                    height: 100%;
                                    background-color: rgba(0, 0, 0, 0.5);
                                    display: none;
                                    justify-content: center;
                                    align-items: center;
                                    z-index: 9999;
                                }

                                .alert-box {
                                    background-color: #3498db;
                                    color: #fff;
                                    padding: 20px;
                                    border-radius: 8px;
                                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
                                    text-align: center;
                                }

                                .close-btn {
                                    position: absolute;
                                    top: 10px;
                                    right: 10px;
                                    cursor: pointer;
                                    font-size: 20px;
                                    color: #fff;
                                }

                                button {
                                    padding: 10px 20px;
                                    font-size: 16px;
                                    background-color: #3498db;
                                    color: #fff;
                                    border: none;
                                    border-radius: 5px;
                                    cursor: pointer;
                                }

                                button:hover {
                                    background-color: #2980b9;
                                }
                            </style>
                        </head>
                        <body>

                            <div class='overlay' id='alertOverlay'>
                                <div class='alert-box'>
                                    <span class='close-btn' onclick='closeAlert()'>&times;</span>
                                    <p>Već postoji korisnik u bazi.</p>
                                    <button onclick='closeAlert()'>OK</button>
                                </div>
                            </div>

                            <script>
                                function showAlert() {
                                    var overlay = document.getElementById('alertOverlay');
                                    overlay.style.display = 'flex';
                                }

                                function closeAlert() {
                                    var overlay = document.getElementById('alertOverlay');
                                    overlay.style.display = 'none';
                                    window.history.back();
                                }

                                // Automatically show the alert when the page loads
                                window.onload = showAlert;
                            </script>
                        </body>
                        </html>";
                    return Content(alertHtml, "text/html");
                }

                _context.Add(korisnici);
                await _context.SaveChangesAsync();
                return   RedirectToAction("Login", "Login");
            }   
            return View(korisnici);
        }
        private bool IsValidEmail(string email)
        {
            string emailRegex = "@";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
        }

        private bool IsValidPassword(string password)
        {
            return password != null && password.Length >= 6 && password.Any(char.IsUpper);
        }
        // GET: Korisnicis/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Korisnici == null)
            {
                return NotFound();
            }

            var korisnici = await _context.Korisnici.FindAsync(id);
            if (korisnici == null)
            {
                return NotFound();
            }
            return View(korisnici);
        }

        // POST: Korisnicis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,KorisnickoIme,Email,Role,Lozinka")] Korisnici korisnici)
        {
            if (id != korisnici.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnici);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisniciExists(korisnici.Id))
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
            return View(korisnici);
        }

        // GET: Korisnicis/Delete/5
        public async Task<IActionResult> Delete(int?  id)
        {
            if (id == null || _context.Korisnici == null)
            {
                return NotFound();
            }

            var korisnici = await _context.Korisnici
                .FirstOrDefaultAsync(m => m.Id == id);
            if (korisnici == null)
            {
                return NotFound();
            }

            return View(korisnici);
        }

        // POST: Korisnicis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Korisnici == null)
            {
                return Problem("Entity set 'WebApplication1Context.Korisnici'  is null.");
            }
            var korisnici = await _context.Korisnici.FindAsync(id);
            if (korisnici != null)
            {
                _context.Korisnici.Remove(korisnici);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KorisniciExists(int? id)
        {
          return (_context.Korisnici?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
