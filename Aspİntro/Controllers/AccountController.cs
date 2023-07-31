using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Models;
using Aspİntro.Utilities.File;
using Aspİntro.ViewModels.Account;
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

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

            await SendEmail(newUser.Email, link);

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

        public async Task SendEmail(string email, string url)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("hsmusayev@gmail.com", "HStudio Games");
            var subject = "Verification Email";
            var to = new EmailAddress(email, "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = $"<a href={url} >Click here</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    

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

            if(!signInResult.Succeeded)
            {
                if(signInResult.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Plese confirm your account");
                    return View();
                }
                ModelState.AddModelError("", "Email or Password is wrong");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if(await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
        }

    }


    }
