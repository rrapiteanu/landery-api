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
    [Route("api/landlord")]
    [Authorize]

    public class LandlordController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public LandlordController(ILogger<AccountController> logger, IUserService userService, IPropertyService propertyService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userService = userService;
            _propertyService = propertyService;
            _jwtAuthManager = jwtAuthManager;
        }


        [HttpGet("listings")]
        public ActionResult Listings()
        {
            var currentUser = _userService.getUserByEmail(User.Identity.Name);
            var properties = _propertyService.GetPropertiesForUser(currentUser);
            return Ok(properties);
        }


        [HttpPost("create-listing")]
        public ActionResult CreateListing([FromBody] CreateListingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var currentUser = _userService.getUserByEmail(User.Identity.Name);

            Property property = _propertyService.AddProperty(currentUser, request.Name, request.Bathrooms,
                                            request.Bedrooms, request.Pets, request.Description,
                                            request.Price, request.Images, request.Amenities);

            return Ok(new CreateListingResult
            {
                PropertyId = property.PropertyId,
            });
        }

        public class CreateListingRequest
        {
            [Required]
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [Required]
            [JsonPropertyName("price")]
            public double Price { get; set; }

            [Required]
            [JsonPropertyName("bedrooms")]
            public int Bedrooms { get; set; }

            [Required]
            [JsonPropertyName("bathrooms")]
            public int Bathrooms { get; set; }

            [Required]
            [JsonPropertyName("pets")]
            public bool Pets { get; set; }

            [Required]
            [JsonPropertyName("description")]
            public string Description { get; set; }

            [Required]
            [JsonPropertyName("amenities")]
            public string[] Amenities { get; set; }

            [Required]
            [JsonPropertyName("images")]
            public string[] Images { get; set; }
        }

        public class CreateListingResult
        {
            [JsonPropertyName("id")]
            public Guid PropertyId { get; set; }
        }

    }
}