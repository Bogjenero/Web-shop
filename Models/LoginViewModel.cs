namespace WebApplication1.Models
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
        
        public DateTime LastLogintimestap { get; set; }
    }
}
