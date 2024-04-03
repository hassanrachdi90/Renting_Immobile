using Microsoft.AspNetCore.Mvc;
using Renting.Application.Common.Interfaces;
using Renting.Domain.Entities;
using Renting.Infrastructure.Data;
using Renting.Infrastructure.Repository;

namespace Renting_Immobile.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }
        public IActionResult Create()
        {  
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if(obj.Name == obj.Description) 
            {
                ModelState.AddModelError("name", "The Description cannot exactly match the name .");
            
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been created successfully .";
                return RedirectToAction(nameof(Index));
                
            }
            return View();
            
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj= _unitOfWork.Villa.Get(x => x.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);  
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id>0)
            {
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been updated successfully .";
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(x => x.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromId = _unitOfWork.Villa.Get(x => x.Id == obj.Id);
            if (objFromId is not null)
            {
                _unitOfWork.Villa.Remove(objFromId);
                _unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been deleted successfully .";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] ="The villa could not be deleted .";
            return View();
        }
    }
}
