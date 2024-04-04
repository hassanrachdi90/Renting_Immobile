using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Renting.Application.Common.Interfaces;
using Renting.Domain.Entities;
using Renting.Infrastructure.Data;
using Renting_Immobile.ViewModels;

namespace Renting_Immobile.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.Amenity.GetAll(includeProperties:"Villa");
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            AmenityVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
           
            if (ModelState.IsValid  )
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenitiy has been created successfully .";
                return RedirectToAction(nameof(Index));
                
            }
            
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(obj);
            
        }

        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Amenity= _unitOfWork.Amenity.Get(x=>x.Id== amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Update(AmenityVM AmenityVM)
        {
           
            if (ModelState.IsValid )
            {
                _unitOfWork.Amenity.Update(AmenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenitiy has been update successfully .";
                return RedirectToAction(nameof(Index));

            }
            AmenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                         {
                                  Text = x.Name,
                                  Value = x.Id.ToString()
                         });
            return View(AmenityVM);
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(x => x.Id == amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM AmenityVM)
        {
            Amenity? objFromId = _unitOfWork.Amenity.Get(x => x.Id == AmenityVM.Amenity.Id);
            if (objFromId is not null)
            {
                _unitOfWork.Amenity.Remove(objFromId);
                _unitOfWork.Save();
                TempData["success"] = "The amenitiy has been deleted successfully .";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] ="The amenitiy could not be deleted .";
            return View();
        }
    }
}
