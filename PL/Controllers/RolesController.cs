using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.Models;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<ApplicationRole> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<ApplicationRole> roleManager,
                                ILogger<ApplicationRole> logger,
                                UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationRole applicationRole)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(applicationRole);
                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach(var role in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, role.Description);
                }
            }
            return View(applicationRole);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
            {
                return NotFound();

            }
            return View(viewName, role);
        }

        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ApplicationRole appRole)
        {
            if (id != appRole.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = appRole.Name;
                    role.NormalizedName = appRole.NormalizedName;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    foreach (var item in result.Errors)
                        ModelState.AddModelError(string.Empty, item.Description);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            }
            return View(appRole);
        }

        public async Task<IActionResult> Delete(string id, ApplicationRole appRole)
        {
            if (id != appRole.Id)
            {
                return NotFound();
            }
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var item in result.Errors)
                    ModelState.AddModelError(string.Empty, item.Description);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role is null)
                return BadRequest();
            ViewBag.RoleId = roleId;
            var usersInRole = new List<UserInRoleViewModel>();
            foreach(var user in await _userManager.Users.ToListAsync())
            {
                var userInRole = new UserInRoleViewModel
                {
                    UserName = user.UserName,
                    UserId = user.Id,

                };
                if(await _userManager.IsInRoleAsync(user, role.Name))
                    userInRole.IsSelected= true;
                else
                    userInRole.IsSelected= false;

                usersInRole.Add(userInRole);
            }
            return View(usersInRole);

        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleViewModel> users, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
                return BadRequest();

            if(ModelState.IsValid)
            {
                foreach(var user in users)
                {
                    var appuser = await _userManager.FindByIdAsync(user.UserId);

                    if(appuser != null)
                    {
                        if (user.IsSelected && !(await _userManager.IsInRoleAsync(appuser, role.Name)))
                            await _userManager.AddToRoleAsync(appuser, role.Name);
                        else if (!user.IsSelected && (await _userManager.IsInRoleAsync(appuser, role.Name)))
                            await _userManager.RemoveFromRoleAsync(appuser, role.Name);
                    }
                }
                return RedirectToAction(nameof(Update), new {id = roleId});
            }
            return View(users);
        }

    }
}
