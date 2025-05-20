using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _19070006046MidtermProject.Data;
using _19070006046MidtermProject.Models;

namespace _19070006046MidtermProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FlightController(AppDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Uçuş Ekleme (korumalı)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFlight([FromBody] Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return Ok(new { status = "success", flightId = flight.Id });
        }

        // 2️⃣ Uçuş Sorgulama
        [HttpGet]
        public async Task<IActionResult> QueryFlights(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] DateTime dateFrom,
            [FromQuery] DateTime dateTo,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            var flightsQuery = _context.Flights
                .Include(f => f.Tickets)
                .Where(f =>
                    f.FromAirport == from &&
                    f.ToAirport == to
                     && f.DateFrom >= dateFrom
                     && f.DateTo <= dateTo
                     && f.Capacity > f.Tickets.Count
                ); 

            var total = await flightsQuery.CountAsync();

            var flights = await flightsQuery
                .Skip((page - 1) * size)
                .Take(size)
                .Select(f => new
                {
                    f.Id,
                    f.FromAirport,
                    f.ToAirport,
                    f.DateFrom,
                    f.DateTo,
                    f.Duration
                })
                .ToListAsync();

            return Ok(new
            {
                page,
                size,
                total,
                data = flights
            });
        }

        // ✅ 3️⃣ Bilet Satın Alma (Artık JSON body alıyor)
        [Authorize]
        [HttpPost("buy")]
        public async Task<IActionResult> BuyTicket([FromBody] BuyTicketRequest request)
        {
            var flight = await _context.Flights
                .Include(f => f.Tickets)
                .FirstOrDefaultAsync(f => f.Id == request.FlightId);

            if (flight == null)
                return NotFound(new { status = "flight not found" });

            if (flight.Capacity <= flight.Tickets.Count)
                return BadRequest(new { status = "sold out" });

            var passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.Name == request.PassengerName);
            if (passenger == null)
            {
                passenger = new Passenger { Name = request.PassengerName };
                _context.Passengers.Add(passenger);
                await _context.SaveChangesAsync();
            }

            var ticket = new Ticket
            {
                FlightId = flight.Id,
                PassengerId = passenger.Id,
                Date = DateTime.Now,
                IsCheckedIn = false,
                SeatNumber = request.SeatNumber // opsiyonel, kullanılmıyorsa kaldır
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "success",
                ticketId = ticket.Id,
                flightId = flight.Id,
                passenger = passenger.Name
            });
        }

        // 4️⃣ Check-In (herkese açık)
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromQuery] int flightId, [FromQuery] string passengerName)
        {
            var passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.Name == passengerName);
            if (passenger == null)
                return NotFound(new { status = "passenger not found" });

            var ticket = await _context.Tickets
                .Where(t => t.FlightId == flightId && t.PassengerId == passenger.Id)
                .FirstOrDefaultAsync();

            if (ticket == null)
                return NotFound(new { status = "ticket not found" });

            if (ticket.IsCheckedIn)
                return BadRequest(new { status = "already checked in", seat = ticket.SeatNumber });

            int seatNumber = await _context.Tickets
                .Where(t => t.FlightId == flightId && t.IsCheckedIn)
                .CountAsync() + 1;

            ticket.SeatNumber = seatNumber;
            ticket.IsCheckedIn = true;
            await _context.SaveChangesAsync();

            return Ok(new { status = "checked in", seat = seatNumber });
        }

        // 5️⃣ Yolcu Listesi (korumalı + paging)
        [Authorize]
        [HttpGet("passengers")]
        public async Task<IActionResult> GetPassengers([FromQuery] int flightId, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var query = _context.Tickets
                .Where(t => t.FlightId == flightId)
                .Include(t => t.Passenger);

            var total = await query.CountAsync();

            var passengers = await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(t => new
                {
                    t.Passenger.Name,
                    t.SeatNumber,
                    t.IsCheckedIn
                })
                .ToListAsync();

            return Ok(new
            {
                page,
                size,
                total,
                data = passengers
            });
        }
    }
}
