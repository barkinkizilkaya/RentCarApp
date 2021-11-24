using System.Collections.Generic;

namespace CarBookingApp.Domain
{
    public class Car
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public List<CarBooking> CarBookings { get; set; }
    }
}