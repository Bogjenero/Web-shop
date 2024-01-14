using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Korisnici
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string KorisnickoIme { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [Key]
        public string Lozinka { get; set; }
    }
}
