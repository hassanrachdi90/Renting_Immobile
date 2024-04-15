using Renting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renting.Application.Utility
{
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusChechedIn = "ChechedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static int VillaRoomsAvailable_Count(int villaId,List<VillaNumber>villaNumbersList,DateOnly checkInDate,int nights, List<Booking> bookings)
        {
            List<int> bookingIdDate = new();
            int finalAvailableRoomForAllNights = int.MaxValue;
            var roomsInVilla = villaNumbersList.Where(x=>x.VillaId == villaId).Count();
            for(int i= 0; i < nights; i++)
            {
                var villaBooked=bookings.Where(u=>u.CheckInDate<=checkInDate.AddDays(i)&& u.CheckOutDate > checkInDate.AddDays(i)&& u.VillaId==villaId);
                foreach(var booking in villaBooked)
                {
                    if (!bookingIdDate.Contains(booking.Id))
                    {
                        bookingIdDate.Add(booking.Id);
                    }
                }
                var totalAvialableRooms = roomsInVilla - bookingIdDate.Count;
                if(totalAvialableRooms == 0)
                {
                    return 0;
                }
                else
                {
                    if(finalAvailableRoomForAllNights> totalAvialableRooms)
                    {
                        finalAvailableRoomForAllNights = totalAvialableRooms;
                    }
                }
               
            }
            return finalAvailableRoomForAllNights;
        }


    }
}
