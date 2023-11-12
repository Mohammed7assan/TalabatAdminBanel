using AdminPanal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdminPanal.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager) {
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var role = await roleManager.Roles.ToListAsync();
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if(ModelState.IsValid)
            {
                var roleExsit = await roleManager.RoleExistsAsync(model.Name);
                if(!roleExsit)
                {
                    await roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                    return RedirectToAction("Index",await roleManager.Roles.ToListAsync());
                }
                else
                {
                    ModelState.AddModelError("Name", "Role is Exists");
                    return View(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public  async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var mapRole = new RoleViewModel
            {
                Name = role.Name
            };
            return View(mapRole);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id,RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.Id);
                var roleExists = await roleManager.RoleExistsAsync(model.Name);
                if(!roleExists)
                {
                   
                    role.Name = model.Name;
                    await roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                else
                {

                    ModelState.AddModelError("Name", "Role is Exists");
                    return View(nameof(Index), await roleManager.Roles.ToListAsync());

                }

                
            }
            return RedirectToAction(nameof(Index));

        }

    }
}
