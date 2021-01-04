using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Landery.Infrastructure;
using Landery.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Landery.Models;

namespace Landery.Controllers
{
    [ApiController]
    [Route("api")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IBookingService _bookingService;

        private readonly IJwtAuthManager _jwtAuthManager;

        public BookingController(ILogger<AccountController> logger, IUserService userService, IPropertyService propertyService, IBookingService bookingService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userService = userService;
            _propertyService = propertyService;
            _bookingService = bookingService;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpGet("bookings")]
        [Authorize]
        public ActionResult Bookings()
        {
            if (User.Identity.Name != "")
            {
                var currentUser = _userService.getUserByEmail(User.Identity.Name);
                var bookings = _bookingService.GetBookings(currentUser);
                return Ok(bookings);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPost("properties/{id}/book")]
        [Authorize]
        public ActionResult BookProperty(Guid id, [FromBody] BookingRequest request)
        {
            try
            {
                var currentUser = _userService.getUserByEmail(User.Identity.Name);
                var booking = _bookingService.BookProperty(id, currentUser, request.StartDate, request.EndDate);
                return Ok();
            }
            catch (Exception)
            {

                return Unauthorized();
            }
        }

        [HttpGet("properties/{id}/bookings")]
        public ActionResult BookingsForProperty(Guid id)
        {
            var bookings = _bookingService.GetBookingsForProperty(id);
            return Ok(bookings);
        }

        public class BookingRequest
        {
            [Required]
            [JsonPropertyName("startDate")]
            public DateTime StartDate { get; set; }

            [Required]
            [JsonPropertyName("endDate")]
            public DateTime EndDate { get; set; }
        }

        public class BookingResult
        {
            [JsonPropertyName("id")]
            public Guid BookingId { get; set; }
        }
    }
}