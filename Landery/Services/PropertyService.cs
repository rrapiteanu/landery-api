using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Landery.Infrastructure;
using Landery.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;


namespace Landery.Services
{
    public interface IPropertyService
    {
        Property AddProperty(User user, string name, int bathrooms,
                            int bedrooms, bool pets, string description,
                            double price, IList<string> images, IList<string> amenities);
        List<Property> GetProperties();
        List<Property> GetFavorites(User user);

        List<Property> GetPropertiesForUser(User user);
        bool FavoriteProperty(Guid id, User user);
        bool UnfavoriteProperty(Guid id, User user);
        Property GetProperty(Guid id);
    }

    public class PropertyService : IPropertyService
    {
        private readonly ILogger<PropertyService> _logger;
        private readonly Context _context = new Context();

        public PropertyService(ILogger<PropertyService> logger)
        {
            _logger = logger;
        }

        public Property AddProperty(User user, string name, int bathrooms,
                    int bedrooms, bool pets, string description,
                    double price, IList<string> images, IList<string> amenities)     
        {
            Property property = Property.Create(name, user, price, bathrooms, bedrooms, pets, description);
            _context.Properties.Add(property);

            foreach (var image in images)
            {
                var new_image = PropertyImage.Create(image, property);
                _context.Images.Add(new_image);
            }

            _context.SaveChanges();
            return property;
        }

        public List<Property> GetProperties()
        {
            return _context.Properties.Include(property => property.PropertyImages).ToList();
        }

        public List<Property> GetFavorites(User user)
        {
            var favorites = _context.Favorites.Where(fav => fav.UserId == user.UserId).ToList();
            var properties = _context.Properties.Include(property => property.PropertyImages).ToList();
            var result = properties.Where(property => favorites.Any(fav => fav.PropertyId == property.PropertyId)).ToList();
            return result;
        }

        public List<Property> GetPropertiesForUser(User user)
        {
            return _context.Properties.Where(p => p.UserId == user.UserId).Include(property => property.PropertyImages).ToList();
        }


        public Property GetProperty(Guid id)
        {
            var property = _context.Properties.Find(id);
            _context.Entry(property).Collection(p => p.PropertyImages).Load();
            return property;
        }

        public bool FavoriteProperty(Guid id, User user)
        {
            bool canFavorite = _context.Favorites.FirstOrDefault(fav => fav.PropertyId == id && fav.UserId == user.UserId) == default(Favorite);
            if (canFavorite)
            {
                var property = _context.Properties.Find(id);

                if(property == null)
                {
                    throw new Exception("property doesn't exist");
                }

                Favorite favorite = Favorite.Create(property,user);
                _context.Favorites.Add(favorite);
                _context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("duplicates");
            }
       
        }

        public bool UnfavoriteProperty(Guid id, User user)
        {
            Favorite favorite = _context.Favorites.FirstOrDefault(fav => fav.PropertyId == id && fav.UserId == user.UserId);
            if (favorite != default(Favorite))
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("is not favorite");
            }

        }
    }
}
