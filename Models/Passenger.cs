namespace _19070006046MidtermProject.Models
{
    public class Passenger
    {
        public int Id { get; set; }  // 🔥 Primary key olarak EF bunu tanır
        public string Name { get; set; }

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
