using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Landery.Models
{
    public class Favorite
    {
        [JsonPropertyName("id")]
        public Guid FavoriteId { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public Guid PropertyId { get; set; }
        [JsonIgnore]
        public virtual Property Property { get; set; }


        public static Favorite Create(Property property, User user)
        {
            return new Favorite
            {
                FavoriteId = Guid.NewGuid(),
                PropertyId = property.PropertyId,
                UserId = user.UserId,
            };
        }

        public Favorite()
        {
            FavoriteId = Guid.NewGuid();
        }
    }
}
