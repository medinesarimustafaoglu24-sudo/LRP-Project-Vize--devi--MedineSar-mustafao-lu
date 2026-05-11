


namespace LRP_Project_Vize_MedineSarımustafaoğlu.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }

        // Zimmetli bilgisayarları bu öğrenci üzerinden görebilmek için ekledik
        public ICollection<Computer>? Computers { get; set; }
    }
}
