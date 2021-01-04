using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


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
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public bool Pets { get; set; }
        public double Price { get; set; }

        [JsonPropertyName("images")]
        public virtual ICollection<PropertyImage> PropertyImages { get; set; }
        [JsonIgnore]
        public virtual ICollection<Favorite> Favorites { get; set; }
        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }


        public static Property Create(string name, User user, double price, int bathrooms, int bedrooms, bool pets, string description)
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
                Pets = pets
            };
        }

        public Property()
        {
            PropertyId = Guid.NewGuid();
        }
    }
}
