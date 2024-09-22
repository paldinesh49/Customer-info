using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication7.Models;
using WebApplication7.ViewModel;

namespace WebApplication7.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Register - GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Register - POST
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user already exists
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "User with this email already exists.");
                }
                else
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // Optionally log in the user after registration
                        // await _signInManager.SignInAsync(user, isPersistent: false);
                        return Json(new { success = true }); // Return a success response
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Return a JSON response with errors
            var errors = ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(e => e.Key, e => e.Value.Errors.Select(x => x.ErrorMessage).ToArray());

            return Json(new { success = false, errors });
        }

        [HttpPost]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return Json(new { exists = user != null });
        }


        // Login - GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login - POST
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Login successful" });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid login attempt." });
                }
            }

            return Json(new { success = false, message = "Invalid form submission." });
        }


        // Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
