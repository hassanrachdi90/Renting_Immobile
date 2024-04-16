using Microsoft.AspNetCore.Mvc;
using Renting.Application.Common.Interfaces;
using Renting.Application.Utility;
using Renting.Domain.Entities;
using Renting.Infrastructure.Repository;
using Renting_Immobile.ViewModels;
using Stripe;
using System.Collections.Generic;
using System.Linq;


namespace Renting_Immobile.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        readonly DateTime previousMonthStartDate=new (DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            var totalBookings= _unitOfWork.Booking.GetAll(u=>u.Status!=SD.StatusPending || u.Status==SD.StatusCancelled);
            var countByCurrentMonth=totalBookings.Count(u=>u.BookingDate>=currentMonthStartDate && u.BookingDate<=DateTime.Now);
            var countByPrevioustMonth = totalBookings.Count(u=>u.BookingDate>= previousMonthStartDate && u.BookingDate<= currentMonthStartDate);
            
            return Json(GetRadialCartDataModel(totalBookings.Count(), countByCurrentMonth, countByPrevioustMonth));
        }
        public async Task<IActionResult> GetRegisteredUserChartData()
        {
            var totalUsers = _unitOfWork.User.GetAll();
            var countByCurrentMonth = totalUsers.Count(u => u.CreatedDAt >= currentMonthStartDate && u.CreatedDAt <= DateTime.Now);
            var countByPrevioustMonth = totalUsers.Count(u => u.CreatedDAt >= previousMonthStartDate && u.CreatedDAt <= currentMonthStartDate);
            

            return Json(GetRadialCartDataModel(totalUsers.Count(),countByCurrentMonth,countByPrevioustMonth));
        }
        public async Task<IActionResult> GetRevenueChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u => u.Status != SD.StatusPending || u.Status == SD.StatusCancelled);
            var totalRevenue = Convert.ToInt32(totalBookings.Sum(u => u.TotalCost));
            var countByCurrentMonth = totalBookings.Where(u => u.BookingDate >= currentMonthStartDate && u.BookingDate <= DateTime.Now).Sum(u => u.TotalCost);
            var countByPrevioustMonth = totalBookings.Where(u => u.BookingDate >= previousMonthStartDate && u.BookingDate <= currentMonthStartDate).Sum(u => u.TotalCost);


            return Json(GetRadialCartDataModel(totalRevenue, countByCurrentMonth, countByPrevioustMonth));
        }

        public async Task<IActionResult> GetBookingPieChartData()
        {
            var totalBookings = _unitOfWork.Booking.GetAll(u =>u.BookingDate>=DateTime.Now.AddDays(-30) &&(u.Status != SD.StatusPending || u.Status == SD.StatusCancelled));
            var customerWithOneBooking = totalBookings.GroupBy(b => b.UserId).Where(x => x.Count() == 1).Select(x=>x.Key).ToList();
            int bookingsByNewCustomer= customerWithOneBooking.Count();
            int bookingsByReturningCustomer = totalBookings.Count() - bookingsByNewCustomer;
            PieChartVM pieChartVM = new()
            {
                Labels=new string[] {"New Customer Bookings ", "Returning Customer Bookings" },
                Series=new decimal[] { bookingsByNewCustomer , bookingsByReturningCustomer }
            };
            return Json(pieChartVM);
        }

        public async Task<IActionResult> GetMemberAndBookingLineChartData()
        {
            var bookingData = _unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) && u.BookingDate.Date <= DateTime.Now).GroupBy(b => b.BookingDate.Date).Select(u => new
            {
                DateTime = u.Key,
                NewBookingCount = u.Count(),
            });
            var customerData = _unitOfWork.User.GetAll(u => u.CreatedDAt >= DateTime.Now.AddDays(-30) && u.CreatedDAt.Date <= DateTime.Now).GroupBy(b => b.CreatedDAt.Date).Select(u => new
            {
                DateTime = u.Key,
                NewCustomerCount = u.Count(),
            });

            var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime, (booking, customer) => new
            {
                booking.DateTime,
                booking.NewBookingCount,
                NewCustomerCount = customer.Select(x => x.NewCustomerCount).FirstOrDefault()
            });
            var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime, (customer, booking) => new
            {
                customer.DateTime,
                customer.NewCustomerCount,
                NewBookingCount = booking.Select(x => x.NewBookingCount).FirstOrDefault(),
            });

            //var mergedData = leftJoin.Union((rightJoin).OrderBy(x => x.DataTime).ToList());
            var mergedData = leftJoin .Select(b => new {DateTime = b.DateTime,NewBookingCount = b.NewBookingCount,NewCustomerCount = b.NewCustomerCount})
               .Union(rightJoin.OrderBy(x => x.DateTime)
               .Select(c => new {DateTime = c.DateTime,NewBookingCount = c.NewBookingCount,NewCustomerCount = c.NewCustomerCount}))
               .ToList();

            var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
            var newCustomerData = mergedData.Select(x => x.NewCustomerCount).ToArray();
            var categories = mergedData.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();

            List<CharData> charDataList = new()
            {
                new CharData
                {
                    Name="New Bookings",
                    Data=newBookingData
                },
                new CharData
                {
                    Name="New Members",
                    Data=newCustomerData
                }

            };
            LineChartVM lineChartVM = new()
            {
                Categories = categories,
                Series = charDataList
            };

            return Json(lineChartVM);
        }

 
        private static RadialBarChartVM GetRadialCartDataModel(int totalCount,double currentMonthCount,double prevMonthCount)
        {
            RadialBarChartVM radialBarChartVM = new();
            int increaseDecreaseRatio = 100;
            if (prevMonthCount != 0)
            {
                increaseDecreaseRatio = Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);
            }

            radialBarChartVM.TotalCount = totalCount;
            radialBarChartVM.CountInCurrentMonth =Convert.ToInt32( currentMonthCount);
            radialBarChartVM.HasRadioIncreased = currentMonthCount > prevMonthCount;
            radialBarChartVM.Series = new int[] { increaseDecreaseRatio };
            return radialBarChartVM;
        }

        
    }
}
