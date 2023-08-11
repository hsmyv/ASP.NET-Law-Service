using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Models;
using Aspİntro.Services.Interfaces;
using Aspİntro.Utilities.File;
using Aspİntro.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Aspİntro.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            AppUser newUser = new()
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(newUser, UserRoles.Member.ToString());
            //await _signInManager.SignInAsync(newUser, false); //automatic login
            //return RedirectToAction("Index", "Home");
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = newUser.Id, token = code }, Request.Scheme, Request.Host.ToString());

            string html = $"<a href={link} >Click here</a>";
            string content = "Email for register confirmation";
            await _emailService.SendEmailAsync(newUser.Email, newUser.UserName, link, html, content);

            return RedirectToAction(nameof(EmailVerification));
        }

        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            if (userId == null || token == null) return BadRequest();
            AppUser user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult EmailVerification()
        {
            return View();
        }

        //public async Task SendEmail(string email, string url)
        //{
        //    var apiKey = "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";
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress("hsmusayev@gmail.com", "HStudio Games");
        //    var subject = "Verification Email";
        //    var to = new EmailAddress(email, "Example User");
        //    var plainTextContent = "and easy to do anywhere, even with C#";
        //    var htmlContent = $"<a href={url} >Click here</a>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);
        //}


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);

            if (user is null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);

            }
            if (user is null)
            {
                ModelState.AddModelError("", "Email or Password Incorrect");
                return View();
            }

            if (!user.IsActivated)
            {
                ModelState.AddModelError("", "Contact with admin");
                return View(loginVM);
            }
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Plese confirm your account");
                    return View();
                }
                ModelState.AddModelError("", "Email or Password is wrong");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "Admin")]
        public async Task CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (!ModelState.IsValid) return View(forgotPasswordVM);

            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if(user is null)
            {
                ModelState.AddModelError("", "This email has not been registered");
                return View(forgotPasswordVM);
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token = code}, Request.Scheme, Request.Host.ToString());
            var html = $"<a href={link} >Click here</a>";
            var content = "Email for forgot password";
            await _emailService.SendEmailAsync(user.Email, user.UserName,link, html, content);
            return RedirectToAction(nameof(ForgotPasswordConfirm));
        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var resetPasswordModel = new ResetPasswordVM { Email = email, Token = token };
            return View(resetPasswordModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid) return View(resetPasswordVM);

            var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
            if (user is null) return NotFound();
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);
           if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);

                }
                return View(resetPasswordVM);
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgotPasswordConfirm()
        {
            return View();
        }




    }


}
