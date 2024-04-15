using Microsoft.AspNetCore.Mvc;
using Renting.Application.Common.Interfaces;
using Renting.Application.Utility;
using Renting.Domain.Entities;
using Renting_Immobile.Models;
using Renting_Immobile.ViewModels;
using System.Diagnostics;

namespace Renting_Immobile.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity"),
                Nights=1,
                CheckInDate=DateOnly.FromDateTime(DateTime.Now)
            };
            return View(homeVM);
        }
        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
        {
            //Thread.Sleep(2000);
            var villaList= _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
            var villaNumberList=_unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas=_unitOfWork.Booking.GetAll(u=>u.Status==SD.StatusApproved || u.Status == SD.StatusChechedIn ).ToList();
            foreach (var villa in villaList)
            {
                //int roomAvailable = SD.VillaRoomsAvailable_Count(villa.Id, villaNumberList, bookedVillas, checkInDate, nights);
                int roomAvailable = SD.VillaRoomsAvailable_Count(villa.Id, villaNumberList, checkInDate, nights, bookedVillas);
                villa.IsAvailable=roomAvailable > 0 ? true : false ;

            }
            HomeVM homeVM = new()
            {
                CheckInDate = checkInDate,
                VillaList = villaList,
                Nights = nights,
               
            };
            return PartialView("_VillaList",homeVM );
        }
        public IActionResult Privacy()
        {
            return View();
        }

        
        public IActionResult Error()
        {
            return View();
        }
    }
}
