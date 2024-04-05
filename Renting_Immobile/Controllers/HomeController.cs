using Microsoft.AspNetCore.Mvc;
using Renting.Application.Common.Interfaces;
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
