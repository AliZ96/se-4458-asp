namespace _19070006046MidtermProject.Models
{
    public class Ticket
    {
        public int Id { get; set; }  // 🔥 Primary key

        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; }

        public DateTime Date { get; set; }
        public bool IsCheckedIn { get; set; }
        public int? SeatNumber { get; set; }
    }
}
