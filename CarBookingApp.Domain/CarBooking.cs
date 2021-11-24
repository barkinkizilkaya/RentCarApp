using System;

using CarBookingApp.Domain.Models;

namespace CarBookingApp.Domain
{
    public class CarBooking : CarBookingBase
    {
       public int Id { get; set; }

        public Car Car { get; set; }

        public int? CarId { get; set; }
    }
}