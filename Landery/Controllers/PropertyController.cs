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
using System.Collections.Generic;

namespace Landery.Controllers
{
    [ApiController]
    [Route("api")]
    public class PropertyController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public PropertyController(ILogger<AccountController> logger, IUserService userService, IPropertyService propertyService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userService = userService;
            _propertyService = propertyService;
            _jwtAuthManager = jwtAuthManager;
        }


        [HttpGet("properties")]
        [Authorize]
        [AllowAnonymous]

        public ActionResult Properties()
        {
            if (User.Identity.IsAuthenticated)
            {
                // intoarce prop cu favorite

                var currentUser = _userService.getUserByEmail(User.Identity.Name);
                var properties = _propertyService.GetProperties();
                var favorites = _propertyService.GetFavorites(currentUser);

                var result = new List<PropertyResult>();
                foreach (var property in properties)
                {
                    var favorite = favorites.Find(fav => fav.PropertyId == property.PropertyId) != null;

                    result.Add(new PropertyResult {
                        PropertyId = property.PropertyId,
                        Price = property.Price,
                        Address = property.Address,
                        Favorite = favorite,
                        Pets = property.Pets,
                        Bathrooms = property.Bathrooms,
                        Bedrooms = property.Bedrooms,
                        Description = property.Description,
                        Name = property.Name,
                        UserId = property.UserId,
                        PropertyImages = property.PropertyImages
                    });
                }

                return Ok(result);
            }
            else
            {
                var properties = _propertyService.GetProperties();

                var result = new List<PropertyResult>();
                foreach (var property in properties)
                {
                    result.Add(new PropertyResult
                    {
                        PropertyId = property.PropertyId,
                        Price = property.Price,
                        Address = property.Address,
                        Favorite = false,
                        Pets = property.Pets,
                        Bathrooms = property.Bathrooms,
                        Bedrooms = property.Bedrooms,
                        Description = property.Description,
                        Name = property.Name,
                        UserId =property.UserId,
                        PropertyImages = property.PropertyImages
                    });
                }

                return Ok(result);
            }
        }

        [HttpGet("amenities")]
        public IEnumerable<string> Amenities()
        {
            return new[] { "pool", "washer" };
        }

        [HttpGet("favorites")]
        [Authorize]
        public ActionResult Favorites()
        {
            if (User.Identity.Name != "")
            {
                // intoarce prop cu favorite

                var currentUser = _userService.getUserByEmail(User.Identity.Name);
                var favorites = _propertyService.GetFavorites(currentUser);
                return Ok(favorites);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpGet("properties/{id}")]
        [Authorize]
        [AllowAnonymous]
        public ActionResult GetProperty(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                // intoarce prop cu favorite

                var currentUser = _userService.getUserByEmail(User.Identity.Name);
                var property = _propertyService.GetProperty(id);
                var favorites = _propertyService.GetFavorites(currentUser);
                var favorite = favorites.Find(fav => fav.PropertyId == property.PropertyId) != null;

                return Ok(new PropertyResult
                {
                    PropertyId = property.PropertyId,
                    Price = property.Price,
                    Address = property.Address,
                    Favorite = favorite,
                    Pets = property.Pets,
                    Bathrooms = property.Bathrooms,
                    Bedrooms = property.Bedrooms,
                    Description = property.Description,
                    Name = property.Name,
                    UserId = property.UserId,
                    PropertyImages = property.PropertyImages
                });
            }
            else
            {
                var property = _propertyService.GetProperty(id);
                return Ok(new PropertyResult
                {
                    PropertyId = property.PropertyId,
                    Price = property.Price,
                    Address = property.Address,
                    Favorite = false,
                    Pets = property.Pets,
                    Bathrooms = property.Bathrooms,
                    Bedrooms = property.Bedrooms,
                    Description = property.Description,
                    Name = property.Name,
                    UserId = property.UserId,
                    PropertyImages = property.PropertyImages
                });
            }
        }

        [HttpPost("properties/{id}/favorite")]
        [Authorize]
        public ActionResult FavoriteProperty(Guid id)
        {
            try
            {
                var currentUser = _userService.getUserByEmail(User.Identity.Name);
                _propertyService.FavoriteProperty(id, currentUser);
                return Ok();
            }
            catch (Exception)
            {
                return Unauthorized();
            }
       
        }

        [HttpPost("properties/{id}/unfavorite")]
        [Authorize]
        public ActionResult UnfavoriteProperty(Guid id)
        {
            try
            {
                var currentUser = _userService.getUserByEmail(User.Identity.Name);
                _propertyService.UnfavoriteProperty(id, currentUser);
                return Ok();
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
    }

    public class PropertyResult
    {
        [JsonPropertyName("favorite")]
        public bool Favorite { get; set; }
        [JsonPropertyName("id")]
        public Guid PropertyId { get; set; }
        [JsonPropertyName("landlord_id")]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public bool Pets { get; set; }
        public double Price { get; set; }
        [JsonPropertyName("images")]
        public virtual ICollection<PropertyImage> PropertyImages { get; set; }
    }
}