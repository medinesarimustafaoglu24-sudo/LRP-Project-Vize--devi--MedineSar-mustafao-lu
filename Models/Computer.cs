



namespace LRP_Project_Vize_MedineSarımustafaoğlu.Models
{
    public class Computer
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string Brand { get; set; }
        public string Processor { get; set; }
        public int Ram { get; set; }
        public string Specs { get; set; }

        public int? LabId { get; set; }
        public Lab? Lab { get; set; }

        // Foreign Key: Bilgisayarın hangi kullanıcıya/öğrenciye ait olduğunu tutar
        public int? StudentId { get; set; }
        public User? Student { get; set; }
    }
}