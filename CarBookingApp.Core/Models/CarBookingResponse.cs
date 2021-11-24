using System;
using CarBookingApp.Core.Enums;
using CarBookingApp.Domain.Models;

namespace RentCarApp.Core.Models
{
    public class CarBookingResponse : CarBookingBase
    {
        public BookingResutFlag Flag { get; set; }

        public int? CarBookingId { get; set; }
    }
}