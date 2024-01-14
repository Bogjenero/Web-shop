namespace WebApplication1.Models
{
    public class BillView
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Cijena { get; set; }
        public string Godiste {get; set;}
        public string Svojstva { get; set; }
        public DateTime DateBill { get; set; }

     }
}
