using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Renting.Domain.Entities;
using Renting.Infrastructure.Data;

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
            var villaNumbers = _db.VillaNumbers.ToList();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value=x.Id.ToString(),

            }) ;
            ViewData["VillaList"] = list;
            return View();
        }
        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {
           
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been created successfully .";
                return RedirectToAction("Index");
                
            }
            return View();
            
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj= _db.Villas.FirstOrDefault(x => x.Id == villaId);
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
                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa has been updated successfully .";
                return RedirectToAction("Index");

            }
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(x => x.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromId = _db.Villas.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromId is not null)
            {
                _db.Villas.Remove(objFromId);
                _db.SaveChanges();
                TempData["success"] = "The villa has been deleted successfully .";
                return RedirectToAction("Index");

            }
            TempData["error"] ="The villa could not be deleted .";
            return View();
        }
    }
}
