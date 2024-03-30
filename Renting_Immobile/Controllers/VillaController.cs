using Microsoft.AspNetCore.Mvc;
using Renting.Domain.Entities;
using Renting.Infrastructure.Data;

namespace Renting_Immobile.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db=db;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
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
                _db.Villas.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
                
            }
            return View();
            
        }
    }
}
