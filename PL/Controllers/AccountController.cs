using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.Helper;
using PL.Models;

namespace PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterVM registerVM)
        {
            if(ModelState.IsValid)
            {
                var User = new ApplicationUser
                {
                    Email = registerVM.Email,
                    UserName = registerVM.Email.Split('@')[0],
                    IsAgree = registerVM.IsAgree
                };
                var result = await _userManager.CreateAsync(User, registerVM.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("SignIn");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(registerVM);
        }
        #endregion

        #region LogIn

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM signInVM)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(signInVM.Email);
                
                if(user is null)
                {
                    ModelState.AddModelError(string.Empty, "Email Deos not exist");

                }
                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, signInVM.Password);
                if (isCorrectPassword) 
                {
                    var result = await _signInManager.PasswordSignInAsync(user, signInVM.Password, signInVM.Remember_Me, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(new SignInVM());
        }

        #endregion

        #region SignOut

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region ForgetPassword

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgetPasswordVM.Email);

                if(user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordLink = Url.Action("ResetPassword","Account",new {Email = forgetPasswordVM.Email, Token = token}, Request.Scheme);
                    var email = new Email
                    {
                        Title = "Reset Password",
                        Body = resetPasswordLink,
                        To = forgetPasswordVM.Email
                    };

                    EmailSettings.SendEmail(email);
                    return RedirectToAction("CompleteForgetPassword");
                }
                ModelState.AddModelError("", "Invalid Email");
            }
            return View();
        }

        public IActionResult CompleteForgetPassword()
        {
            return View();
        }

        #endregion

        public IActionResult ResetPassword(string email,string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
                if(user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
                    if(result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(new ResetPasswordVM());
        }
    }

}
