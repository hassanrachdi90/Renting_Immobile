using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Renting.Domain.Entities;
using Renting.Infrastructure.Data;
using Renting_Immobile.ViewModels;

namespace Renting_Immobile.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext db)
        {
            _db=db;
        }
        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(x=>x.Villa).ToList();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem
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
            bool rommNimberExists = _db.VillaNumbers.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number) ;
            if (ModelState.IsValid && !rommNimberExists)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been created successfully .";
                return RedirectToAction(nameof(Index));
                
            }
            if (rommNimberExists)
            {
                TempData["error"] = "The villa number already exists";
            }
            obj.VillaList = _db.Villas.ToList().Select(x => new SelectListItem
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
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                VillaNumber=_db.VillaNumbers.FirstOrDefault(x=>x.Villa_Number==villaNumberId)
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
                _db.VillaNumbers.Update(VillaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been update successfully .";
                return RedirectToAction(nameof(Index));

            }
            VillaNumberVM.VillaList = _db.Villas.ToList().Select(x => new SelectListItem
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
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
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
            VillaNumber? objFromId = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == VillaNumberVM.VillaNumber.Villa_Number);
            if (objFromId is not null)
            {
                _db.VillaNumbers.Remove(objFromId);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully .";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] ="The villa number could not be deleted .";
            return View();
        }
    }
}
