using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly WebApplication1Context dbContext;

        public LoginController(WebApplication1Context dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Id,Username,Password,Mail,Role,LastLogintimestap")] LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (!IsValidEmail(user.Mail))
                {
                    ModelState.AddModelError(nameof(LoginViewModel.Mail), "Invalid email format.");
                    return View(user);
                }
                var user1 = dbContext.Korisnici.FirstOrDefault(u =>
                                u.KorisnickoIme == user.Username &&
                                u.Lozinka == user.Password &&
                                (string.IsNullOrEmpty(user.Role) || u.Role == user.Role) &&
                                (string.IsNullOrEmpty(user.Mail) || u.Email == user.Mail));
                if (user1 != null)
                {
                    user.LastLogintimestap = DateTime.Now;
                    dbContext.Add(user);
                    await dbContext.SaveChangesAsync();
                    if (user.Role == "Kupac")
                    {
                        return RedirectToAction("Proizvodi", "PrikazProizvoda");
                    }
                    else
                    {
                        return RedirectToAction("Create","Proizvodis");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Korisnik nije pronađen");
                }
            }
            return View(user);
        }
        private bool IsValidEmail(string email)
        {
            string emailRegex = "@";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
        }
        private async Task WriteToDatabaseAsync(LoginViewModel model)
        {
            var user = dbContext.Korisnici.FirstOrDefault(u => u.KorisnickoIme == model.Username);

            if (user != null)
            {
                model.LastLogintimestap = DateTime.Now;
                model.Role = user.Role;
                model.Mail = user.Email;
                model.Id = user.Id;

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
