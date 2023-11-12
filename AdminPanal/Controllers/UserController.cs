using AdminPanal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace AdminPanal.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController( UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var User = await userManager.Users.Select(u => new UserViewModel 
            { 
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                DisplayName = u.DisplayName,
                Roles = userManager.GetRolesAsync(u).Result
            }).ToListAsync();

            return View(User);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var allRoles = await roleManager.Roles.ToListAsync();

            var UserVM = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList(),


            };
            return View(UserVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if(userRoles.Any(r=> r == role.Name) && !role.IsSelected)
                {
                    await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                {
                    await userManager.AddToRoleAsync(user, role.Name);
                }
            }
            return RedirectToAction(nameof(Index));


        }



    }
}
