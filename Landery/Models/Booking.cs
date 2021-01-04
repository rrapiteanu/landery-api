using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Landery.Models
{
    public class Booking
    {
        [JsonPropertyName("id")]
        public Guid BookingId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public Guid PropertyId { get; set; }
        public virtual Property Property { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static Booking Create(Property property, User user, DateTime startDate, DateTime endDate)
        {
            return new Booking
            {
                BookingId = Guid.NewGuid(),
                PropertyId = property.PropertyId,
                UserId = user.UserId,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public Booking()
        {
            BookingId = Guid.NewGuid();
        }
    }
}
