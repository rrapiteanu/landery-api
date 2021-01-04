using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Landery.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonIgnore]
        public virtual ICollection<Property> Properties { get; set; }
        [JsonIgnore]
        public virtual ICollection<Favorite> Favorites { get; set; }
        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }


        public static User Create(string email, string password, string firstName, string lastName)
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
            };
        }

        public User()
        {
            UserId = Guid.NewGuid();
        }

        public void Update(string firstName, string lastName, string password)
        {
            if (firstName != string.Empty)
                FirstName = firstName;
            if (lastName != string.Empty)
                LastName = lastName;
            if (password != string.Empty)
                Password = password;
        }
    }
}
