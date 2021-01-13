using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Landery.Infrastructure;
using Landery.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;


namespace Landery.Services
{
    public interface IBookingService
    {
        Booking BookProperty(Guid id, User user, DateTime startDate, DateTime endDate);
        IEnumerable<Booking> GetBookings(User user);
        IEnumerable<Booking> GetBookingsForProperty(Guid id);

    }

    public class BookingService : IBookingService
    {
        private readonly ILogger<BookingService> _logger;
        private readonly Context _context = new Context();

        public BookingService(ILogger<BookingService> logger)
        {
            _logger = logger;
        }
        public Booking BookProperty(Guid id, User user, DateTime startDate, DateTime endDate)
        {
            var property = _context.Properties.Find(id);

            if (property == null)
            {
                throw new Exception("property doesn't exist");
            }
            // check pentru startdate si endate
            List<Booking> bookings = GetBookingsForProperty(id).ToList();

            if (!checkDates(bookings, startDate, endDate))
            {
                throw new Exception("already booked");
            }
            Booking booking = Booking.Create(property, user, startDate, endDate);
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return booking;
        }

        private bool IsBewteenTwoDates(DateTime date, DateTime start, DateTime end)
        {
            return date >= start && date <= end;
        }

        private bool checkDates(List<Booking> bookings, DateTime startDate, DateTime endDate)
        {
          var check1 = !bookings.Any(m => IsBewteenTwoDates(startDate, m.StartDate, m.EndDate));
          var check2 = !bookings.Any(m => IsBewteenTwoDates(endDate, m.StartDate, m.EndDate));
            return check1 && check2;
        }

        public IEnumerable<Booking> GetBookings(User user)
        {
            return _context.Bookings.Where(b => b.UserId == user.UserId).Include(b => b.Property).ToList();
        }
        public IEnumerable<Booking> GetBookingsForProperty(Guid id)
        {
            return _context.Bookings.Where(b => b.PropertyId == id).ToList();
        }
    }
}
