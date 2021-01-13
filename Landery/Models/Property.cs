using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Landery.Models
{
    public class Property
    {
        [JsonPropertyName("id")]
        public Guid PropertyId { get; set; }
        [JsonPropertyName("landlord_id")]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Address { get; set; }
        [Range(1, 9)]
        [Required]
        public int Bedrooms { get; set; }
        [Range(1, 9)]
        [Required]
        public int Bathrooms { get; set; }
        [Required]
        public bool Pets { get; set; }

        [Range(0.0, Double.MaxValue)]
        public double Price { get; set; }
        public List<string> Amenities { get; set; }

        [JsonPropertyName("images")]
        public virtual ICollection<PropertyImage> PropertyImages { get; set; }
        [JsonIgnore]
        public virtual ICollection<Favorite> Favorites { get; set; }
        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }


        public static Property Create(string name, User user, double price, int bathrooms, int bedrooms, bool pets, string description, List<string> amenities)
        {
            return new Property
            {
                PropertyId = Guid.NewGuid(),
                UserId = user.UserId,
                Name = name,
                Description = description,
                Price = price,
                Bathrooms = bathrooms,
                Bedrooms = bedrooms,
                Pets = pets,
                Amenities = amenities
            };
        }

        public Property()
        {
            PropertyId = Guid.NewGuid();
        }
    }
}
