using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;



namespace WebApplication1.Controllers
{
    public class PrikazProizvodaController : Controller
    {
        private readonly WebApplication1Context dbContext;
        

        public PrikazProizvodaController(WebApplication1Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Proizvodi()
        {
            var proizvodi = await dbContext.Proizvodi.ToListAsync();
            var proizvodiWithFileNames = new List<Proizvodi>();

            foreach (var proizvod in proizvodi)
            {
                var filename = proizvod.SlikeUrl + ".jpg";
                var filename2 = proizvod.Placanja + ".jpg";
                proizvodiWithFileNames.Add(new Proizvodi { Id = proizvod.Id, Model = proizvod.Model, SlikeUrl = filename, Svojstva = proizvod.Svojstva, Godiste = proizvod.Godiste, Cijena = proizvod.Cijena, Placanja = filename2 });
            }

            ViewBag.ProizvodiWithFileNames = proizvodiWithFileNames;
            return View(proizvodi);
        }

        [HttpPost]
        public IActionResult DecreaseProperty(int ItemId )
        {
            var proizvod = dbContext.Proizvodi.FirstOrDefault(u => u.Id == ItemId );
      
            if (proizvod != null && proizvod.Stanje > 0)
            {
                proizvod.Stanje -= 1;

                dbContext.SaveChanges();            

                var bill = GenerateBill(proizvod);

                return View("Bill",bill);    
      
            }
            else
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
                                    <p>Ispričavamo se, nemamo taj automobil na stanju trenutno.</p>
                                    <p>Obavijestit ćemo vas putem email-a.</p>
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
        }

        private BillView GenerateBill(Proizvodi proizvod)
        {
            var bill = new BillView
            {
                Godiste = proizvod.Godiste,
                Svojstva = proizvod.Svojstva,
                Model = proizvod.Model,
                Cijena = proizvod.Cijena,
                DateBill = DateTime.Now,
            };

            dbContext.Bill.Add(bill);
            dbContext.SaveChanges();

            return bill;
        }
    }
}
