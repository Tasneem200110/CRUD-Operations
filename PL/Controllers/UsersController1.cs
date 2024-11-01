using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<ApplicationUser> user, ILogger<UsersController> logger) 
        {
            _user = user;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            List<ApplicationUser> users;
            if(string.IsNullOrEmpty(SearchValue))
            {
                users = await _user.Users.ToListAsync();

            }
            else
            {
                users = await _user.Users.Where(user => user.Email.Trim().ToLower().Contains(SearchValue.Trim().ToLower())).ToListAsync();
            }
            return View(users);
        }
        
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if(id is null)
            {
                return NotFound();
            }
            var user = await _user.FindByIdAsync(id);
            if(user is null)
            {
                return NotFound();

            }
            return View(viewName, user);
        }

        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ApplicationUser appUser)
        {
            if(id != appUser.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    var user = await _user.FindByIdAsync(id);
                    user.UserName = appUser.UserName;
                    user.NormalizedEmail = appUser.NormalizedEmail;
                    var result = await _user.UpdateAsync(user);
                    if(result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    foreach(var item in result.Errors)
                        ModelState.AddModelError(string.Empty, item.Description);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                
            }
            return View(appUser);
        }

        public async Task<IActionResult> Delete(string id, ApplicationUser appUser)
        {
            if (id != appUser.Id)
            {
                return NotFound();
            }
            try
            {
                var user = await _user.FindByIdAsync(id);
                var result = await _user.DeleteAsync(user);
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

    }
}
