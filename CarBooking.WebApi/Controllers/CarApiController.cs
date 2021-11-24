using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentCarApp.Core.Models;
using RentCarApp.Core.Services;

namespace CarBooking.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarApiController : ControllerBase
    {
        private ICarBookingRequestProcessor _carBookingProcessor;

        public CarApiController(ICarBookingRequestProcessor carBookingProcessor)
        {
            _carBookingProcessor = carBookingProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> BookCar(CarBookingRequest request)
        {
            if(ModelState.IsValid)
            {
               var result = _carBookingProcessor.BookCar(request);
                if (result.Flag == CarBookingApp.Core.Enums.BookingResutFlag.Success)
                { return Ok(result); }
              
                ModelState.AddModelError(nameof(CarBookingRequest.Date), "No rooms Avaible For given date");
            }

            return BadRequest(ModelState);
        }
    }
}