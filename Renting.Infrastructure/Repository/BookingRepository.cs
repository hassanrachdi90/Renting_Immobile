using Microsoft.EntityFrameworkCore;
using Renting.Application.Common.Interfaces;
using Renting.Application.Utility;
using Renting.Domain.Entities;
using Renting.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Renting.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _db;

        public BookingRepository(ApplicationDbContext db) :base(db) 
        {
            _db = db;
        }
       

        public void Update(Booking entity)
        {
            _db.Bookings.Update(entity);
        }

        public void UpdateStatus(int bookingId, string bookingStatus , int villaNumber=0)
        {
            var bookingFromDB = _db.Bookings.FirstOrDefault(m => m.Id == bookingId);
            if (bookingFromDB != null)
            {
                bookingFromDB.Status = bookingStatus;
                if (bookingStatus== SD.StatusChechedIn)
                {
                    bookingFromDB.VillaNumber= villaNumber;
                    bookingFromDB.ActualCheckInDate = DateTime.Now;
                }
                if (bookingStatus == SD.StatusCompleted)
                {
                    bookingFromDB.ActualCheckOutDate = DateTime.Now;
                }
            }
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromDB = _db.Bookings.FirstOrDefault(m => m.Id == bookingId);
            if (bookingFromDB != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    bookingFromDB.StripeSessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    bookingFromDB.StripePaymentIntentId = paymentIntentId;
                    bookingFromDB.PaymentDate= DateTime.Now;
                    bookingFromDB.IsPaymentSuccessful = true;
                }
            }
        }
    }
}
