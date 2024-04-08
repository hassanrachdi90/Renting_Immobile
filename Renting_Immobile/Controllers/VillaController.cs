using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renting.Application.Common.Interfaces;
using Renting.Application.Utility;
using Renting.Domain.Entities;
using Renting.Infrastructure.Data;
using Renting.Infrastructure.Repository;

namespace Renting_Immobile.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
                if (obj.Image !=null)
                {
                    string fileName=Guid.NewGuid().ToString()+Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Image\VillaImage");
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                    obj.ImageUrl = @"\Image\VillaImage\"+fileName;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }
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
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Image\VillaImage");
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImapgePath= Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImapgePath))
                        {
                            System.IO.File.Exists(oldImapgePath);
                        }

                    }
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                    obj.ImageUrl = @"\Image\VillaImage\" + fileName;
                }
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
                if (!string.IsNullOrEmpty(objFromId.ImageUrl))
                {
                    var oldImapgePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromId.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImapgePath))
                    {
                        System.IO.File.Exists(oldImapgePath);
                    }

                }
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
