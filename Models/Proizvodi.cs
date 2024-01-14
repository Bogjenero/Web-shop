namespace WebApplication1.Models
{
    public class Proizvodi
    {
        public int Id { get; set; } 
        public string Model { get; set; } 
        public string Svojstva  { get; set; } 
        public string SlikeUrl { get; set; }  
        public string Godiste { get; set; }  

        public int Cijena { get; set; }

        public int Stanje {  get; set; }

        public string Placanja {  get; set; }
    }
}
