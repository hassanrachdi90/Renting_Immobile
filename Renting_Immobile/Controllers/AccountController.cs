using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Renting.Application.Common.Interfaces;
using Renting.Domain.Entities;
using Renting_Immobile.ViewModels;

namespace Renting_Immobile.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            
        }

        public IActionResult Login( string returnUrl=null)
        {
            returnUrl??= Url.Content("~/");
            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl
            };
            return View(loginVM);
        }
        public IActionResult Register()
        {
            if(! _roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                _roleManager.CreateAsync(new IdentityRole("Costumer")).Wait();
            }

            RegisterVM registerVM = new()
            {RoleList=_roleManager.Roles.Select(x=>new SelectListItem
            {
                Text=x.Name,
                Value=x.Name
            })
            };
           
            return View(registerVM);
        }
    }
}
