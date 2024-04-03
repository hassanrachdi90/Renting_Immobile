using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Renting.Application.Common.Interfaces;
using Renting.Domain.Entities;
using Renting.Infrastructure.Data;
using Renting_Immobile.ViewModels;

namespace Renting_Immobile.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties:"Villa");
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
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
        public IActionResult Create(VillaNumberVM obj)
        {
            bool rommNimberExists = _unitOfWork.VillaNumber.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number) ;
            if (ModelState.IsValid && !rommNimberExists)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been created successfully .";
                return RedirectToAction(nameof(Index));
                
            }
            if (rommNimberExists)
            {
                TempData["error"] = "The villa number already exists";
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(obj);
            
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                VillaNumber= _unitOfWork.VillaNumber.Get(x=>x.Villa_Number==villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM VillaNumberVM)
        {
           
            if (ModelState.IsValid )
            {
                _unitOfWork.VillaNumber.Update(VillaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been update successfully .";
                return RedirectToAction(nameof(Index));

            }
            VillaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                         {
                                  Text = x.Name,
                                  Value = x.Id.ToString()
                         });
            return View(VillaNumberVM);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM VillaNumberVM)
        {
            VillaNumber? objFromId = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == VillaNumberVM.VillaNumber.Villa_Number);
            if (objFromId is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromId);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully .";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] ="The villa number could not be deleted .";
            return View();
        }
    }
}
