using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Landery.Models
{
    public class PropertyImage
    {
        [JsonIgnore]
        public Guid PropertyImageId { get; set; }
        public string Url { get; set; }
        [JsonIgnore]
        public Guid PropertyId { get; set; }
        [JsonIgnore]
        public virtual Property Property { get; set; }
        public static PropertyImage Create(string url, Property property)
        {
            return new PropertyImage
            {
                PropertyImageId = Guid.NewGuid(),
                PropertyId = property.PropertyId,
                Url = url,
            };
        }

        public PropertyImage()
        {
            PropertyImageId = Guid.NewGuid();
        }
    }
}
