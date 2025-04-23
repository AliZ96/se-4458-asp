namespace _19070006046MidtermProject.Models
{
    public class Flight
    {
        public int Id { get; set; }  // 🔥 Primary key

        public string FromAirport { get; set; }
        public string ToAirport { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Duration { get; set; }
        public int Capacity { get; set; }

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();  // boş null hatası almamak için
    }
}
